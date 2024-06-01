using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using DriveType = CarDealership.Core.Models.DriveType;

namespace CarDealership.DataAccess.Factories
{
    public class CarEMFactory : IEntityModelFactory<Car, CarEntity>
    {
        public CarEntity CreateEntity(Car model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var entity = new CarEntity
            { 
                Id = model.Id,
                IsDeleted = model.IsDeleted,
                AutoConfigurationId = model.AutoConfigurationId,
                VIN = model.VIN
            };

            return entity;
        }

        public Car CreateModel(CarEntity entity)
        {
            if(entity == null) throw new ArgumentNullException(nameof(entity));

            var carModelEntity = entity.AutoConfiguration?.AutoModel;
            var carBrandEntity = entity.AutoConfiguration?.AutoModel?.Brand;
            var carBodyTypeEntity = entity.AutoConfiguration?.BodyType;
            var carDriveTypeEntity = entity.AutoConfiguration?.DriveType;
            var carEngineTypeEntity = entity.AutoConfiguration?.Engine?.EngineType;
            var carTransmissionTypeEntity = entity.AutoConfiguration?.Engine?.TransmissionType;
            var carEngineEntity = entity.AutoConfiguration?.Engine;
            var carColorEntity = entity.AutoConfiguration?.Color;

            var carBrand = carBrandEntity != null ? Brand.Create(
                carBrandEntity.Id,
                carBrandEntity.Name,
                carBrandEntity.CountryId,
                carBrandEntity.IsDeleted
            ).Value : null;


            var carModel = carModelEntity != null ? AutoModel.Create(
                carModelEntity.Id,
                carModelEntity.Name,
                carModelEntity.Price,
                carModelEntity.BrandId,
                carModelEntity.IsDeleted,
                carBrand
            ).Value : null;

            var carBodyType = carBodyTypeEntity != null ? BodyType.Create(
                carBodyTypeEntity.Id,
                carBodyTypeEntity.Value,
                carBodyTypeEntity.Price,
                carBodyTypeEntity.IsDeleted
            ).Value : null;

            var carDriveType = carDriveTypeEntity != null ? DriveType.Create(
                carDriveTypeEntity.Id,
                carDriveTypeEntity.Value,
                carDriveTypeEntity.Price,
                carDriveTypeEntity.IsDeleted
            ).Value : null;

            var carEngineType = carEngineTypeEntity != null ? EngineType.Create(
                carEngineTypeEntity.Id,
                carEngineTypeEntity.Value,
                carEngineTypeEntity.IsDeleted
            ).Value : null;

            var carTransmissionType = carTransmissionTypeEntity != null ? TransmissionType.Create(
                carTransmissionTypeEntity.Id,
                carTransmissionTypeEntity.Value,
                carTransmissionTypeEntity.IsDeleted
            ).Value : null;

            var carEngine = carEngineEntity != null ? Engine.Create(
                carEngineEntity.Id,
                carEngineEntity.Power,
                carEngineEntity.Consumption,
                carEngineEntity.Price,
                carEngineEntity.EngineTypeId,
                carEngineEntity.TransmissionTypeId,
                carEngineEntity.IsDeleted,
                carEngineType,
                carTransmissionType
            ).Value : null;

            var carColor = carColorEntity != null ? Color.Create(
                carColorEntity.Id,
                carColorEntity.Value,
                carColorEntity.Price,
                carColorEntity.IsDeleted
            ).Value : null;

            var autoConfigModel = entity.AutoConfiguration != null ? AutoConfiguration.Create(
                entity.AutoConfigurationId,
                entity.AutoConfiguration.Price,
                entity.AutoConfiguration.AutoModelId,
                entity.AutoConfiguration.BodyTypeId,
                entity.AutoConfiguration.DriveTypeId,
                entity.AutoConfiguration.EngineId,
                entity.AutoConfiguration.ColorId,
                entity.AutoConfiguration.IsDeleted,
                carModel,
                carBodyType,
                carDriveType,
                carEngine,
                carColor
            ).Value : null;

            var carCreateResult = Car.Create(
                entity.Id,
                entity.VIN,
                entity.AutoConfigurationId,
                entity.IsDeleted,
                autoConfigModel
            );

            if(carCreateResult.IsFailure)
            {
                throw new InvalidOperationException(carCreateResult.Error);
            }

            var car = carCreateResult.Value;

            return car;
        }
    }
}
