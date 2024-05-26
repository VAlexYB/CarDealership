using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class EngineEMFactory : IEntityModelFactory<Engine, EngineEntity>
    {
        private readonly IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity> _autoConfigEMFactory;

        public EngineEMFactory(
            IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity> autoConfigEMFactory
        )
        {
            _autoConfigEMFactory = autoConfigEMFactory ?? throw new ArgumentNullException(nameof(autoConfigEMFactory));
        }

        public EngineEntity CreateEntity(Engine model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var configurationEntities = model.Configurations
                .Select(configuration => _autoConfigEMFactory.CreateEntity(configuration))
                .ToList();

            var entity = new EngineEntity
            {
                Id = model.Id,
                Power = model.Power,
                Consumption = model.Consumption,
                Price = model.Price,
                EngineTypeId = model.EngineTypeId,
                TransmissionTypeId = model.TransmissionTypeId,
                IsDeleted = model.IsDeleted,
                Configurations = configurationEntities
            };

            return entity;
        }


        public Engine CreateModel(EngineEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var engineTypeModel = entity.EngineType != null ? EngineType.Create(
                entity.EngineTypeId,
                entity.EngineType.Value,
                entity.EngineType.IsDeleted
            ).Value : null;

            var transmissionTypeModel = entity.TransmissionType != null ? TransmissionType.Create(
                entity.TransmissionTypeId,
                entity.TransmissionType.Value,
                entity.TransmissionType.IsDeleted
            ).Value : null;

            var engineCreateResult = Engine.Create(
                entity.Id,
                entity.Power,
                entity.Consumption,
                entity.Price,
                entity.EngineTypeId,
                entity.TransmissionTypeId,
                entity.IsDeleted,
                engineTypeModel,
                transmissionTypeModel
            );

            if (engineCreateResult.IsFailure)
            {
                throw new InvalidOperationException(engineCreateResult.Error);
            }

            var engine = engineCreateResult.Value;

            foreach (var configurationEntity in entity.Configurations)
            {
                var configurationModel = _autoConfigEMFactory.CreateModel(configurationEntity);
                engine.AddConfiguration(configurationModel);
            }

            return engine;
        }
    }
}
