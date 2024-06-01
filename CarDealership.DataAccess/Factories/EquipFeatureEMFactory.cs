using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class EquipFeatureEMFactory : IEntityModelFactory<Core.Models.EquipmentFeature, Entities.EquipmentFeatureEntity>
    {
        public Entities.EquipmentFeatureEntity CreateEntity(Core.Models.EquipmentFeature model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var entity = new Entities.EquipmentFeatureEntity
            {
                Id = model.Id,
                EquipmentId = model.EquipmentId,
                FeatureId = model.FeatureId
            };

            return entity;
        }

        public Core.Models.EquipmentFeature CreateModel(Entities.EquipmentFeatureEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var equipmentModel = entity.Equipment != null ? Equipment.Create(
                entity.EquipmentId,
                entity.Equipment.Name,
                entity.Equipment.Price,
                entity.Equipment.ReleaseYear,
                entity.Equipment.AutoModelId,
                entity.Equipment.IsDeleted
            ).Value : null;

            var featureModel = entity.Feature != null ? Feature.Create(
                entity.FeatureId,
                entity.Feature.Description,
                entity.Feature.IsDeleted
            ).Value : null;

            var equipmentFeatureCreateResult = Core.Models.EquipmentFeature.Create(
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
