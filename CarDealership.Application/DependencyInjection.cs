using CarDealership.Application.Services;
using CarDealership.Core.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CarDealership.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<IAutoModelsService, AutoModelsService>();

            return services;
        }
    }
}
