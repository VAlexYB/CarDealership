using CarDealership.Core.Abstractions;
using CarDealership.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CarDealership.DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            services.AddScoped<IAutoModelsRepository, AutoModelsRepository>();

            return services;
        }
    }
}
