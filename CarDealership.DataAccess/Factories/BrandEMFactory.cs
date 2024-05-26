using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class BrandEMFactory : IEntityModelFactory<Brand, BrandEntity>
    {
        private readonly IEntityModelFactory<AutoModel, AutoModelEntity> _autoModelEMFactory;

        public BrandEMFactory(
            IEntityModelFactory<AutoModel, AutoModelEntity> autoModelEMFactory)
        {
            _autoModelEMFactory = autoModelEMFactory ?? throw new ArgumentNullException(nameof(autoModelEMFactory));
        }

        public BrandEntity CreateEntity(Brand model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var models = model.Models.Select(model => _autoModelEMFactory.CreateEntity(model)).ToList();

            var entity = new BrandEntity
            {
                Name = model.Name,
                CountryId = model.CountryId,
                Models = models,
                IsDeleted = model.IsDeleted,
            };

            return entity;
        }

        public Brand CreateModel(BrandEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var country = entity.Country != null ? Country.Create(
                entity.CountryId,
                entity.Country.Name,
                entity.Country.IsDeleted
            ).Value : null;

            var bodyTypeResult = Brand.Create(
                entity.Id,
                entity.Name,
                entity.CountryId,
                entity.IsDeleted,
                country
            );

            if (bodyTypeResult.IsFailure)
            {
                throw new InvalidOperationException(bodyTypeResult.Error);
            }

            var bodyType = bodyTypeResult.Value;

            foreach(var model in entity.Models)
            {
                bodyType.AddModel(_autoModelEMFactory.CreateModel(model));
            }

            return bodyType;
        }
    }
}
