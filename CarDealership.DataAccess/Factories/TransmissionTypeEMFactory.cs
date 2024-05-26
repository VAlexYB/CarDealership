using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class TransmissionTypeEMFactory : IEntityModelFactory<TransmissionType, TransmissionTypeEntity>
    {
        private readonly IEntityModelFactory<Engine, EngineEntity> _engineEMFactory;
        
        public TransmissionTypeEMFactory(IEntityModelFactory<Engine, EngineEntity> engineEMFactory)
        {
            _engineEMFactory = engineEMFactory ?? throw new ArgumentNullException(nameof(engineEMFactory));
        }

        public TransmissionTypeEntity CreateEntity(TransmissionType model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var engineEntities = model.Engines
                .Select(engine => _engineEMFactory.CreateEntity(engine))
                .ToList();

            var entity = new TransmissionTypeEntity
            {
                Id = model.Id,
                Value = model.Value,
                IsDeleted = model.IsDeleted,
                Engines = engineEntities
            };

            return entity;
        }

        public TransmissionType CreateModel(TransmissionTypeEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var transmissionTypeCreateResult = TransmissionType.Create(
                entity.Id,
                entity.Value,
                entity.IsDeleted
            );

            if(transmissionTypeCreateResult.IsFailure)
            {
                throw new InvalidOperationException(transmissionTypeCreateResult.Error);
            }

            var transmissionType = transmissionTypeCreateResult.Value;

            foreach (var engineEntity in entity.Engines)
            {
                var engineModel = _engineEMFactory.CreateModel(engineEntity);
                transmissionType.AddEngine(engineModel);
            }

            return transmissionType;
        }
    }
}
