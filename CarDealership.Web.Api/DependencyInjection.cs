using CarDealership.Application.Auth;
using CarDealership.Infrastructure;
using CarDealership.Web.Api.Factories;
using CarDealership.Web.Api.Factories.Abstract;

namespace CarDealership.Web.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddControllersSupport(this IServiceCollection services)
        {
            services.AddTransient<IAutoConfigRMFactory, AutoConfigRMFactory>();
            services.AddTransient<IAutoModelRMFactory, AutoModelRMFactory>();
            services.AddTransient<IBodyTypeRMFactory, BodyTypeRMFactory>();
            services.AddTransient<IBrandRMFactory, BrandRMFactory>();
            services.AddTransient<ICarRMFactory, CarRMFactory>();
            services.AddTransient<IColorRMFactory, ColorRMFactory>();
            services.AddTransient<ICountryRMFactory, CountryRMFactory>();
            services.AddTransient<IDriveTypeRMFactory, DriveTypeRMFactory>();
            services.AddTransient<IEngineRMFactory, EngineRMFactory>();
            services.AddTransient<IEngineTypeRMFactory, EngineTypeRMFactory>();
            services.AddTransient<IEquipmentRMFactory,EquipmentRMFactory>();
            services.AddTransient<IFeatureRMFactory, FeatureRMFactory>();
            services.AddTransient<ITransmissionTypeRMFactory, TransmissionTypeRMFactory>();
            services.AddTransient<IOrderRMFactory, OrderRMFactory>();
            services.AddTransient<IDealRMFactory, DealRMFactory>();

            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
