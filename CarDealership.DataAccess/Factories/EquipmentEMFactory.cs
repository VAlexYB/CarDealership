using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class EquipmentEMFactory : IEntityModelFactory<Equipment, EquipmentEntity>
    {
        private readonly IEntityModelFactory<Core.Models.EquipmentFeature, Entities.EquipmentFeatureEntity> _equipmentFeatureEMFactory;

        public EquipmentEMFactory(
            IEntityModelFactory<Core.Models.EquipmentFeature, Entities.EquipmentFeatureEntity> equipmentFeatureEMFactory
        )
        {
            _equipmentFeatureEMFactory = equipmentFeatureEMFactory ?? throw new ArgumentNullException(nameof(equipmentFeatureEMFactory));
        }

        public EquipmentEntity CreateEntity(Equipment model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var featureEntities = model.EquipmentFeatures
                .Select(feature => _equipmentFeatureEMFactory.CreateEntity(feature))
                .ToList();

            var entity = new EquipmentEntity
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                ReleaseYear = model.ReleaseYear,
                AutoModelId = model.AutoModelId,
                IsDeleted = model.IsDeleted,
                equipmentFeatures = featureEntities
            };

            return entity;
        }

        public Equipment CreateModel(EquipmentEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var autoModel = entity.AutoModel != null ? AutoModel.Create(
                entity.AutoModelId,
                entity.AutoModel.Name,
                entity.AutoModel.Price,
                entity.AutoModel.BrandId,
                entity.AutoModel.IsDeleted
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

            return equipment;
        }
    }
}
