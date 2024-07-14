using CarDealership.Application;
using CarDealership.Application.Auth;
using CarDealership.DataAccess;
using CarDealership.Infrastructure;
using CarDealership.Infrastructure.Auth;
using CarDealership.Infrastructure.Messaging;
using CarDealership.Web.Api;
using CarDealership.Web.Api.Exstensions;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Quartz;
using System.Reflection;

var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    var config = builder.Configuration;
    var services = builder.Services;

    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
    var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
    services.AddApiAuthentication(Options.Create(jwtOptions));

    services.AddControllers();

    services.AddEndpointsApiExplorer();

    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Car Dealership API", Version = "v1" });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
        Console.WriteLine($"XML Path: {xmlPath}");
    });

   services.AddDbContext<CarDealershipDbContext>(
        options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(CarDealershipDbContext)));
        });

    services
        .AddDataAccess()
        .AddBusinessLogic()
        .AddControllersSupport()
        .ConfigureSupportingServices();

    services.AddSingleton(new RabbitMQMessageSender(
        config["RabbitMQ:HostName"],
        config["RabbitMQ:UserName"],
        config["RabbitMQ:Password"]
    ));

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

    var scheduler = app.Services.GetService<IScheduler>();
    scheduler.Start().Wait();

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