using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using DriveType = CarDealership.Core.Models.DriveType;

namespace CarDealership.DataAccess.Factories
{
    public class AutoConfigEMFactory : IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity>
    {
        private readonly IEntityModelFactory<Car, CarEntity> _carEMFactory;
        private readonly IEntityModelFactory<EquipmentFeature, EquipmentFeatureEntity> _equipmentFeatureEMFactory;
        public AutoConfigEMFactory(
            IEntityModelFactory<Car, CarEntity> carEMFactory,
            IEntityModelFactory<EquipmentFeature, EquipmentFeatureEntity> equipmentFeatureEMFactory
        )
        {
            _carEMFactory = carEMFactory ?? throw new ArgumentNullException(nameof(carEMFactory));
            _equipmentFeatureEMFactory = equipmentFeatureEMFactory ?? throw new ArgumentNullException(nameof(equipmentFeatureEMFactory));

        }

        public AutoConfiguration CreateModel(AutoConfigurationEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var carBrandEntity = entity.AutoModel?.Brand;
            var carEngineTypeEntity = entity.Engine?.EngineType;
            var carTransmissionTypeEntity = entity.Engine?.TransmissionType;

            var carBrand = carBrandEntity != null ? Brand.Create(
               carBrandEntity.Id,
               carBrandEntity.Name,
               carBrandEntity.CountryId,
               carBrandEntity.IsDeleted
           ).Value : null;

            var autoModel = entity.AutoModel != null ? AutoModel.Create(
                    entity.AutoModelId,
                    entity.AutoModel.Name,
                    entity.AutoModel.Price,
                    entity.AutoModel.BrandId,
                    entity.AutoModel.IsDeleted,
                    carBrand
            ).Value: null;

            var bodyType = entity.BodyType != null ? BodyType.Create(
                    entity.BodyTypeId,
                    entity.BodyType.Value,
                    entity.BodyType.Price,
                    entity.BodyType.IsDeleted
            ).Value : null;

            var driveType = entity.DriveType != null ? DriveType.Create(
                entity.DriveTypeId,
                entity.DriveType.Value,
                entity.DriveType.Price,
                entity.DriveType.IsDeleted
               
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

            var engine = entity.Engine != null ? Engine.Create(
                entity.EngineId,
                entity.Engine.Power,
                entity.Engine.Consumption,
                entity.Engine.Price,
                entity.Engine.EngineTypeId,
                entity.Engine.TransmissionTypeId,
                entity.Engine.IsDeleted,
                carEngineType,
                carTransmissionType
            ).Value : null;

            var color = entity.Color != null ? Color.Create(
                entity.ColorId,
                entity.Color.Value,
                entity.Color.Price,
                entity.Color.IsDeleted
            ).Value : null;

            var equipment = entity.Equipment != null ? Equipment.Create(
                entity.EquipmentId,
                entity.Equipment.Name,
                entity.Equipment.Price,
                entity.Equipment.ReleaseYear,
                entity.Equipment.AutoModelId,
                entity.Equipment.IsDeleted,
                autoModel
            ).Value : null;

            if(equipment != null)
            {
                foreach ( var featureEntity in entity.Equipment.equipmentFeatures) 
                {
                    var featureModel = _equipmentFeatureEMFactory.CreateModel(featureEntity);
                    equipment.AddEquipmentFeature(featureModel);
                }
            }

            var autoConfigurationResult = AutoConfiguration.Create(
                entity.Id,
                entity.Price,
                entity.AutoModelId,
                entity.BodyTypeId,
                entity.DriveTypeId,
                entity.EngineId,
                entity.ColorId,
                entity.EquipmentId,
                entity.IsDeleted,
                autoModel,
                bodyType,
                driveType,
                engine,
                color,
                equipment
            );

            if (autoConfigurationResult.IsFailure)
            {
                throw new InvalidOperationException(autoConfigurationResult.Error);
            }

            var autoConfiguration = autoConfigurationResult.Value;

            foreach (var carEntity in entity.Cars)
            {
                autoConfiguration.AddCar(_carEMFactory.CreateModel(carEntity));
            }

            return autoConfiguration;
        }

        public AutoConfigurationEntity CreateEntity(AutoConfiguration model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var carEntities = model.Cars.Select(car => _carEMFactory.CreateEntity(car)).ToList();

            var entity = new AutoConfigurationEntity
            {
                Id = model.Id,
                Price = model.Price,
                AutoModelId = model.AutoModelId,
                BodyTypeId = model.BodyTypeId,
                DriveTypeId = model.DriveTypeId,
                EngineId = model.EngineId,
                ColorId = model.ColorId,
                EquipmentId = model.EquipmentId,
                IsDeleted = model.IsDeleted,
                Cars = carEntities
            };

            return entity;
        }
    }
}
