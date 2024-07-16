using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class EquipmentEMFactory : IEntityModelFactory<Equipment, EquipmentEntity>
    {
        private readonly IEntityModelFactory<Core.Models.EquipmentFeature, Entities.EquipmentFeatureEntity> _equipmentFeatureEMFactory;
        private readonly IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity> _autoConfigurationEMFactory;

        public EquipmentEMFactory(
            IEntityModelFactory<Core.Models.EquipmentFeature, Entities.EquipmentFeatureEntity> equipmentFeatureEMFactory,
            IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity> autoConfigurationEMFactory
        )
        {
            _equipmentFeatureEMFactory = equipmentFeatureEMFactory ?? throw new ArgumentNullException(nameof(equipmentFeatureEMFactory));
            _autoConfigurationEMFactory = autoConfigurationEMFactory ?? throw new ArgumentNullException(nameof(autoConfigurationEMFactory));
        }

        public EquipmentEntity CreateEntity(Equipment model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var featureEntities = model.EquipmentFeatures
                .Select(feature => _equipmentFeatureEMFactory.CreateEntity(feature))
                .ToList();

            var configurationEntities = model.Configurations
                .Select(configuration => _autoConfigurationEMFactory.CreateEntity(configuration))
                .ToList();

            var entity = new EquipmentEntity
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                ReleaseYear = model.ReleaseYear,
                AutoModelId = model.AutoModelId,
                IsDeleted = model.IsDeleted,
                equipmentFeatures = featureEntities,
                Configurations = configurationEntities
            };

            return entity;
        }

        public Equipment CreateModel(EquipmentEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var equipmentBrandEntity = entity.AutoModel?.Brand;

            var brand = equipmentBrandEntity != null ? Brand.Create(
                equipmentBrandEntity.Id,
                equipmentBrandEntity.Name,
                equipmentBrandEntity.CountryId,
                equipmentBrandEntity.IsDeleted
            ).Value : null;

            var autoModel = entity.AutoModel != null ? AutoModel.Create(
                entity.AutoModelId,
                entity.AutoModel.Name,
                entity.AutoModel.Price,
                entity.AutoModel.BrandId,
                entity.AutoModel.IsDeleted,
                brand
            ).Value : null;

            var equipmentCreateResult = Equipment.Create(
                entity.Id,
                entity.Name,
                entity.Price,
                entity.ReleaseYear,
                entity.AutoModelId,
                entity.IsDeleted,
                autoModel
            );

            if (equipmentCreateResult.IsFailure)
            {
                throw new InvalidOperationException(equipmentCreateResult.Error);
            }

            var equipment = equipmentCreateResult.Value;

            foreach (var featureEntity in entity.equipmentFeatures)
            {
                var featureModel = _equipmentFeatureEMFactory.CreateModel(featureEntity);
                equipment.AddEquipmentFeature(featureModel);
            }

            foreach (var config in entity.Configurations)
            {
                var configModel = _autoConfigurationEMFactory.CreateModel(config);
                equipment.AddConfiguration(configModel);
            }

            return equipment;
        }
    }
}
