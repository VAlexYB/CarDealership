using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class EngineTypeEMFactory : IEntityModelFactory<EngineType, EngineTypeEntity>
    {
        private readonly EngineEMFactory _engineEMFactory;

        public EngineTypeEMFactory(EngineEMFactory engineEMFactory)
        {
            _engineEMFactory = engineEMFactory ?? throw new ArgumentNullException(nameof(engineEMFactory));
        }

        public EngineTypeEntity CreateEntity(EngineType model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var engineEntities = model.Engines
                .Select(engine => _engineEMFactory.CreateEntity(engine))
                .ToList();

            var entity = new EngineTypeEntity
            {
                Id = model.Id,
                Value = model.Value,
                IsDeleted = model.IsDeleted,
                Engines = engineEntities
            };

            return entity;
        }

        public EngineType CreateModel(EngineTypeEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var engineTypeCreateResult = EngineType.Create(
                entity.Id,
                entity.Value,
                entity.IsDeleted
            );

            if (engineTypeCreateResult.IsFailure)
            {
                throw new InvalidOperationException(engineTypeCreateResult.Error);
            }

            var engineType = engineTypeCreateResult.Value;

            foreach (var engineEntity in entity.Engines)
            {
                var engineModel = _engineEMFactory.CreateModel(engineEntity);
                engineType.AddEngine(engineModel);
            }

            return engineType;
        }
    }
}
