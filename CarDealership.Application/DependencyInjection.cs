using CarDealership.Application.Auth;
using CarDealership.Application.Services;
using CarDealership.Core.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CarDealership.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddTransient<IAutoConfigsService, AutoConfigsService>();
            services.AddTransient<IAutoModelsService, AutoModelsService>();
            services.AddTransient<IBodyTypesService, BodyTypesService>();
            services.AddTransient<IBrandsService, BrandsService>();
            services.AddTransient<ICarsService, CarsService>();
            services.AddTransient<IColorsService, ColorsService>();
            services.AddTransient<ICountriesService, CountriesService>();
            services.AddTransient<IDriveTypesService, DriveTypesService>();
            services.AddTransient<IEnginesService, EnginesService>();
            services.AddTransient<IEngineTypesService, EngineTypesService>();
            services.AddTransient<IEquipmentsService, EquipmentsService>();
            services.AddTransient<IFeaturesService, FeaturesService>();
            services.AddTransient<ITransmissionTypesService, TransmissionTypesService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IRolesService, RolesService>();
            services.AddTransient<IOrdersService, OrdersService>();
            services.AddTransient<IDealsService, DealsService>();            

            return services;
        }
    }
}
