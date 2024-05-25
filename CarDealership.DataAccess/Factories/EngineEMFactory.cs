using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class EngineEMFactory : IEntityModelFactory<Engine, EngineEntity>
    {
        private readonly AutoConfigEMFactory _autoConfigEMFactory;
        private readonly EngineTypeEMFactory _engineTypeEMFactory;
        private readonly TransmissionTypeEMFactory _transmissionTypeEMFactory;

        public EngineEMFactory(
            AutoConfigEMFactory autoConfigEMFactory,
            EngineTypeEMFactory engineTypeEMFactory,
            TransmissionTypeEMFactory transmissionTypeEMFactory)
        {
            _autoConfigEMFactory = autoConfigEMFactory ?? throw new ArgumentNullException(nameof(autoConfigEMFactory));
            _engineTypeEMFactory = engineTypeEMFactory ?? throw new ArgumentNullException(nameof(engineTypeEMFactory));
            _transmissionTypeEMFactory = transmissionTypeEMFactory ?? throw new ArgumentNullException(nameof(transmissionTypeEMFactory));
        }

        public EngineEntity CreateEntity(Engine model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var configurationEntities = model.Configurations
                .Select(configuration => _autoConfigEMFactory.CreateEntity(configuration))
                .ToList();

            var engineTypeEntity = model.EngineType != null ? _engineTypeEMFactory.CreateEntity(model.EngineType) : null;
            var transmissionTypeEntity = model.TransmissionType != null ? _transmissionTypeEMFactory.CreateEntity(model.TransmissionType) : null;

            var entity = new EngineEntity
            {
                Id = model.Id,
                Power = model.Power,
                Consumption = model.Consumption,
                Price = model.Price,
                EngineTypeId = model.EngineTypeId,
                EngineType = engineTypeEntity,
                TransmissionTypeId = model.TransmissionTypeId,
                IsDeleted = model.IsDeleted,
                TransmissionType = transmissionTypeEntity,
                Configurations = configurationEntities
            };

            return entity;
        }


        public Engine CreateModel(EngineEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var engineTypeModel = entity.EngineType != null ? _engineTypeEMFactory.CreateModel(entity.EngineType) : null;
            var transmissionTypeModel = entity.TransmissionType != null ? _transmissionTypeEMFactory.CreateModel(entity.TransmissionType) : null;

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
