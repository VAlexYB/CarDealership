using CarDealership.Application;
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
using Newtonsoft.Json;
using CarDealership.Web.Api.Endpoints.Grpc;
using CarDealership.DataAccess.Extensions;
using CarDealership.Web.Api.Middlewares;
using NLog;

var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(5177, listenOptions =>
        {
            listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
        });

        options.ListenAnyIP(7243, listenOptions =>
        {
            listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
        });
    });

    var config = builder.Configuration;
    var services = builder.Services;

    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
    var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
    services.AddApiAuthentication(Options.Create(jwtOptions));

    services.AddControllers().AddNewtonsoftJson();

    services.AddGrpc();
    
    JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
    {
        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };

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
    
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("Redis");
    });

    services
        .AddDataAccess()
        .AddBusinessLogic()
        .AddControllersSupport()
        .ConfigureSupportingServices();

    services.AddSingleton<IRabbitMQMessageSender>(provider => new RabbitMQMessageSender(
        config["RabbitMQ:HostName"],
        config["RabbitMQ:UserName"],
        config["RabbitMQ:Password"]
    ));

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.ApplyMigrations();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Dealership API V1");
        });
    }


    app.UseHttpsRedirection();

    app.UseCookiePolicy(new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.Strict,
        HttpOnly = HttpOnlyPolicy.None,
        Secure = CookieSecurePolicy.None
    });

    app.UseCors(x =>
    {
        x.WithHeaders().AllowAnyHeader();
        x.WithOrigins("http://localhost:3000");
        x.WithMethods().AllowAnyMethod();
        x.AllowCredentials();
    });

    app.UseMiddleware<ExceptionMiddleware>();

    app.UseAuthentication();
    app.UseAuthorization();


    app.MapControllers();
    app.MapGrpcService<DataGrpcService>();

    var scheduler = app.Services.GetService<IScheduler>();
    scheduler.Start().Wait();

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    logger.Fatal(ex, $"Фатальная ошибка запуска приложения. Message: {ex.Message}");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}