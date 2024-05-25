using CarDealership.Core.Abstractions.Repositories;
using CarDealership.DataAccess.Factories;
using CarDealership.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CarDealership.DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            services.AddScoped<IAutoModelsRepository, AutoModelsRepository>();
            services.AddScoped<AutoModelEMFactory>();
            services.AddScoped<BrandEMFactory>();
            services.AddScoped<AutoConfigEMFactory>();
            services.AddScoped<EquipmentEMFactory>();
            return services;
        }
    }
}
