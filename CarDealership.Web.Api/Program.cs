using CarDealership.Application;
using CarDealership.Application.Auth;
using CarDealership.DataAccess;
using CarDealership.Web.Api;
using CarDealership.Web.Api.Exstensions;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
    var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
    builder.Services.AddApiAuthentication(Options.Create(jwtOptions));

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Car Dealership API", Version = "v1" });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
        Console.WriteLine($"XML Path: {xmlPath}");
    });

    builder.Services.AddDbContext<CarDealershipDbContext>(
        options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(CarDealershipDbContext)));
        });

    builder.Services
        .AddDataAccess()
        .AddBusinessLogic()
        .AddControllersSupport();



    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Dealership API V1");
    });

    app.UseHttpsRedirection();

    app.UseCookiePolicy(new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.Strict,
        HttpOnly = HttpOnlyPolicy.Always,
        Secure = CookieSecurePolicy.Always
    });

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.UseCors(x =>
    {
        x.WithHeaders().AllowAnyHeader();
        x.WithOrigins("http://localhost:3000");
        x.WithMethods().AllowAnyMethod();
        x.AllowCredentials();
    });

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Ошибка при запуске программы");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}