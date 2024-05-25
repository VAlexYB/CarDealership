using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class FeatureEMFactory : IEntityModelFactory<Feature, FeatureEntity>
    {
        private readonly EquipFeatureEMFactory _equipmentFeatureEMFactory;

        public FeatureEMFactory(EquipFeatureEMFactory equipmentFeatureEMFactory)
        {
            _equipmentFeatureEMFactory = equipmentFeatureEMFactory ?? throw new ArgumentNullException(nameof(equipmentFeatureEMFactory));
        }

        public FeatureEntity CreateEntity(Feature model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var equipmentFeatureEntities = model.FeatureEquipments
                .Select(equipmentFeature => _equipmentFeatureEMFactory.CreateEntity(equipmentFeature))
                .ToList();

            var entity = new FeatureEntity
            {
                Id = model.Id,
                Description = model.Description,
                IsDeleted = model.IsDeleted,
                featureEquipments = equipmentFeatureEntities
            };

            return entity;
        }

        public Feature CreateModel(FeatureEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var featureCreateResult = Feature.Create(
                entity.Id,
                entity.Description,
                entity.IsDeleted
            );

            if (featureCreateResult.IsFailure)
            {
                throw new InvalidOperationException(featureCreateResult.Error);
            }

            var feature = featureCreateResult.Value;

            foreach (var equipmentFeatureEntity in entity.featureEquipments)
            {
                var equipmentFeatureModel = _equipmentFeatureEMFactory.CreateModel(equipmentFeatureEntity);
                feature.AddFeatureEquipment(equipmentFeatureModel);
            }

            return feature;
        }
    }
}
