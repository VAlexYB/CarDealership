using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class EquipmentEMFactory : IEntityModelFactory<Equipment, EquipmentEntity>
    {
        private readonly AutoModelEMFactory _autoModelEMFactory;
        private readonly EquipFeatureEMFactory _equipmentFeatureEMFactory;

        public EquipmentEMFactory(AutoModelEMFactory autoModelEMFactory, EquipFeatureEMFactory equipmentFeatureEMFactory)
        {
            _autoModelEMFactory = autoModelEMFactory ?? throw new ArgumentNullException(nameof(autoModelEMFactory));
            _equipmentFeatureEMFactory = equipmentFeatureEMFactory ?? throw new ArgumentNullException(nameof(equipmentFeatureEMFactory));
        }

        public EquipmentEntity CreateEntity(Equipment model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var autoModelEntity = model.AutoModel != null ? _autoModelEMFactory.CreateEntity(model.AutoModel) : null;
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
                AutoModel = autoModelEntity,
                equipmentFeatures = featureEntities
            };

            return entity;
        }

        public Equipment CreateModel(EquipmentEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var autoModel = entity.AutoModel != null ? _autoModelEMFactory.CreateModel(entity.AutoModel) : null;
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
