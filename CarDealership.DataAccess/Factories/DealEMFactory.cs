using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using DriveType = CarDealership.Core.Models.DriveType;

namespace CarDealership.DataAccess.Factories
{
    public class DealEMFactory : IEntityModelFactory<Deal, DealEntity>
    {
        private readonly IEntityModelFactory<EquipmentFeature, EquipmentFeatureEntity> _equipmentFeatureEMFactory;

        public DealEMFactory(IEntityModelFactory<EquipmentFeature, EquipmentFeatureEntity> equipmentFeatureEMFactory)
        {
            _equipmentFeatureEMFactory = equipmentFeatureEMFactory;
        }

        public DealEntity CreateEntity(Deal model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var deal = new DealEntity
            {
                Id = model.Id,
                DealDate = model.DealDate,
                Status = model.Status,
                CarId = model.CarId,
                CustomerId = model.CustomerId,
                ManagerId = model.ManagerId,
            };

            return deal;
        }

        public Deal CreateModel(DealEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));


            var carConfigEntity = entity.Car?.AutoConfiguration;

            var carBrandEntity = carConfigEntity?.AutoModel?.Brand;
            var carEngineTypeEntity = carConfigEntity?.Engine?.EngineType;
            var carTransmissionTypeEntity = carConfigEntity?.Engine?.TransmissionType;

            var carBrand = carBrandEntity != null ? Brand.Create(
               carBrandEntity.Id,
               carBrandEntity.Name,
               carBrandEntity.CountryId,
               carBrandEntity.IsDeleted
           ).Value : null;

            var autoModel = carConfigEntity?.AutoModel != null ? AutoModel.Create(
                    carConfigEntity.AutoModelId,
                    carConfigEntity.AutoModel.Name,
                    carConfigEntity.AutoModel.Price,
                    carConfigEntity.AutoModel.BrandId,
                    carConfigEntity.AutoModel.IsDeleted,
                    carBrand
            ).Value : null;

            var bodyType = carConfigEntity?.BodyType != null ? BodyType.Create(
                    carConfigEntity.BodyTypeId,
                    carConfigEntity.BodyType.Value,
                    carConfigEntity.BodyType.Price,
                    carConfigEntity.BodyType.IsDeleted
            ).Value : null;

            var driveType = carConfigEntity?.DriveType != null ? DriveType.Create(
                carConfigEntity.DriveTypeId,
                carConfigEntity.DriveType.Value,
                carConfigEntity.DriveType.Price,
                carConfigEntity.DriveType.IsDeleted
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

            var engine = carConfigEntity?.Engine != null ? Engine.Create(
                carConfigEntity.EngineId,
                carConfigEntity.Engine.Power,
                carConfigEntity.Engine.Consumption,
                carConfigEntity.Engine.Price,
                carConfigEntity.Engine.EngineTypeId,
                carConfigEntity.Engine.TransmissionTypeId,
                carConfigEntity.Engine.IsDeleted,
                carEngineType,
                carTransmissionType
            ).Value : null;

            var color = carConfigEntity?.Color != null ? Color.Create(
                carConfigEntity.ColorId,
                carConfigEntity.Color.Value,
                carConfigEntity.Color.Price,
                carConfigEntity.Color.IsDeleted
            ).Value : null;

            var carEquipment = carConfigEntity?.Equipment != null ? Equipment.Create(
                 carConfigEntity.EquipmentId,
                 carConfigEntity.Equipment.Name,
                 carConfigEntity.Equipment.Price,
                 carConfigEntity.Equipment.ReleaseYear,
                 carConfigEntity.Equipment.AutoModelId,
                 carConfigEntity.Equipment.IsDeleted,
                 autoModel
             ).Value : null;

            if (carEquipment != null)
            {
                foreach (var featureEntity in carConfigEntity.Equipment.equipmentFeatures)
                {
                    var featureModel = _equipmentFeatureEMFactory.CreateModel(featureEntity);
                    carEquipment.AddEquipmentFeature(featureModel);
                }
            }

            var carConfig = carConfigEntity != null ? AutoConfiguration.Create(
                carConfigEntity.Id,
                carConfigEntity.Price,
                carConfigEntity.AutoModelId,
                carConfigEntity.BodyTypeId,
                carConfigEntity.DriveTypeId,
                carConfigEntity.EngineId,
                carConfigEntity.ColorId,
                carConfigEntity.EquipmentId,
                carConfigEntity.IsDeleted,
                autoModel,
                bodyType,
                driveType,
                engine,
                color,
                carEquipment
            ).Value : null;

            var car = entity.Car != null ? Car.Create(
                entity.Car.Id,
                entity.Car.VIN, 
                entity.Car.AutoConfigurationId,
                entity.Car.IsDeleted,
                carConfig
            ).Value : null;

            var dealCreateResult = Deal.Create(
                entity.Id,
                entity.DealDate,
                entity.Status,
                entity.Price,
                entity.CarId,
                entity.ManagerId,
                entity.CustomerId,
                entity.IsDeleted,
                car
            );

            if(dealCreateResult.IsFailure)
            {
                throw new InvalidOperationException(dealCreateResult.Error);
            }

            var deal = dealCreateResult.Value;
            return deal;
        }
    }
}
