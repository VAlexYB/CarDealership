using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;
using CarDealership.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;
using DriveType = CarDealership.Core.Models.DriveType;

namespace CarDealership.DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            services.AddTransient<IAutoConfigsRepository, AutoConfigsRepository>();
            services.AddTransient<IAutoModelsRepository, AutoModelsRepository>();
            services.AddTransient<IBodyTypesRepository, BodyTypesRepository>();
            services.AddTransient<IBrandsRepository, BrandsRepository>();
            services.AddTransient<ICarsRepository, CarsRepository>();
            services.AddTransient<IColorsRepository, ColorsRepository>();
            services.AddTransient<ICountriesRepository, CountriesRepository>();
            services.AddTransient<IDriveTypesRepository, DriveTypesRepository>();
            services.AddTransient<IEnginesRepository, EnginesRepository>();
            services.AddTransient<IEngineTypesRepository, EngineTypesRepository>();
            services.AddTransient<IEquipFeaturesRepository, EquipFeaturesRepository>();
            services.AddTransient<IEquipmentsRepository, EquipmentsRepository>();
            services.AddTransient<IFeaturesRepository, FeaturesRepository>();
            services.AddTransient<ITransmissionTypesRepository, TransmissionTypesRepository>();

            //services.AddScoped<IEntityModelFactory<>();
            services.AddTransient<IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity>, AutoConfigEMFactory>();
            services.AddTransient<IEntityModelFactory<AutoModel, AutoModelEntity>, AutoModelEMFactory>();
            services.AddTransient<IEntityModelFactory<BodyType, BodyTypeEntity>, BodyTypeEMFactory>();
            services.AddTransient<IEntityModelFactory<Brand, BrandEntity>, BrandEMFactory>();
            services.AddTransient<IEntityModelFactory<Car, CarEntity>, CarEMFactory>();
            services.AddTransient<IEntityModelFactory<Color, ColorEntity>, ColorEMFactory>();
            services.AddTransient<IEntityModelFactory<Country, CountryEntity>, CountryEMFactory>();
            services.AddTransient<IEntityModelFactory<DriveType, DriveTypeEntity>, DriveTypeEMFactory>();
            services.AddTransient<IEntityModelFactory<Engine, EngineEntity>, EngineEMFactory>();
            services.AddTransient<IEntityModelFactory<EngineType, EngineTypeEntity>, EngineTypeEMFactory>();
            services.AddTransient<IEntityModelFactory<EquipmentFeature, EquipmentFeatureEntity>, EquipFeatureEMFactory>();
            services.AddTransient<IEntityModelFactory<Equipment, EquipmentEntity>, EquipmentEMFactory>();
            services.AddTransient<IEntityModelFactory<Feature, FeatureEntity>, FeatureEMFactory>();
            services.AddTransient<IEntityModelFactory<TransmissionType, TransmissionTypeEntity>, TransmissionTypeEMFactory>();
            return services;
        }
    }
}
