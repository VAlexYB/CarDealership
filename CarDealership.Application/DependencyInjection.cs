using CarDealership.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace CarDealership.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<IAutoModelsService, IAutoModelsService>();

            return services;
        }
    }
}
