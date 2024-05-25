using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class EquipFeatureEMFactory : IEntityModelFactory<EquipmentFeature, EquipmentFeatureEntity>
    {
        private readonly EquipmentEMFactory _equipmentEMFactory;
        private readonly FeatureEMFactory _featureEMFactory;

        public EquipFeatureEMFactory(EquipmentEMFactory equipmentEMFactory, FeatureEMFactory featureEMFactory)
        {
            _equipmentEMFactory = equipmentEMFactory ?? throw new ArgumentNullException(nameof(equipmentEMFactory));
            _featureEMFactory = featureEMFactory ?? throw new ArgumentNullException(nameof(featureEMFactory));
        }

        public EquipmentFeatureEntity CreateEntity(EquipmentFeature model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var equipmentEntity = model.Equipment != null ? _equipmentEMFactory.CreateEntity(model.Equipment) : null;
            var featureEntity = model.Feature != null ? _featureEMFactory.CreateEntity(model.Feature) : null;

            var entity = new EquipmentFeatureEntity
            {
                Id = model.Id,
                EquipmentId = model.EquipmentId,
                Equipment = equipmentEntity,
                FeatureId = model.FeatureId,
                Feature = featureEntity
            };

            return entity;
        }

        public EquipmentFeature CreateModel(EquipmentFeatureEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var equipmentModel = entity.Equipment != null ? _equipmentEMFactory.CreateModel(entity.Equipment) : null;
            var featureModel = entity.Feature != null ? _featureEMFactory.CreateModel(entity.Feature) : null;

            var equipmentFeatureCreateResult = EquipmentFeature.Create(
                entity.Id,
                entity.EquipmentId,
                entity.FeatureId,
                equipmentModel,
                featureModel
            );

            if (equipmentFeatureCreateResult.IsFailure)
            {
                throw new InvalidOperationException(equipmentFeatureCreateResult.Error);
            }

            var equipmentFeature = equipmentFeatureCreateResult.Value;
            return equipmentFeature;
        }
    }
}
