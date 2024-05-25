using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class BodyTypeEMFactory : IEntityModelFactory<BodyType, BodyTypeEntity>
    {
        private readonly AutoConfigEMFactory _autoConfigEMFactory;

        public BodyTypeEMFactory(AutoConfigEMFactory autoConfigEMFactory)
        {
            _autoConfigEMFactory = autoConfigEMFactory ?? throw new ArgumentNullException(nameof(autoConfigEMFactory));
        }

        public BodyTypeEntity CreateEntity(BodyType model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var configurations = model.Configurations.Select(config => _autoConfigEMFactory.CreateEntity(config)).ToList();

            var entity = new BodyTypeEntity
            {
                Id = model.Id,
                Value = model.Value,
                Price = model.Price,
                Configurations = configurations,
                IsDeleted = model.IsDeleted
            };

            return entity;
        }

        public BodyType CreateModel(BodyTypeEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));


            var bodyTypeResult = BodyType.Create(
                entity.Id,
                entity.Value,
                entity.Price,
                entity.IsDeleted
            );

            if (bodyTypeResult.IsFailure)
            {
                throw new InvalidOperationException(bodyTypeResult.Error);
            }

            var bodyType = bodyTypeResult.Value;

            foreach (var config in entity.Configurations)
            {
                bodyType.AddConfiguration(_autoConfigEMFactory.CreateModel(config));
            }

            return bodyType;
        }
    }
}
