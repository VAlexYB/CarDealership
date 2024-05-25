using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class BrandEMFactory : IEntityModelFactory<Brand, BrandEntity>
    {
        private readonly CountryEMFactory _countryEMFactory;
        private readonly AutoModelEMFactory _autoModelEMFactory;

        public BrandEMFactory(CountryEMFactory countryEMFactory, AutoModelEMFactory autoModelEMFactory)
        {
            _countryEMFactory = countryEMFactory ?? throw new ArgumentNullException(nameof(countryEMFactory));
            _autoModelEMFactory = autoModelEMFactory ?? throw new ArgumentNullException(nameof(autoModelEMFactory));
        }

        public BrandEntity CreateEntity(Brand model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var countryEntity = model.Country != null ? _countryEMFactory.CreateEntity(model.Country) : null;

            var models = model.Models.Select(model => _autoModelEMFactory.CreateEntity(model)).ToList();

            var entity = new BrandEntity
            {
                Name = model.Name,
                CountryId = model.CountryId,
                Country = countryEntity,
                Models = models,
                IsDeleted = model.IsDeleted,
            };

            return entity;
        }

        public Brand CreateModel(BrandEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var country = entity.Country != null ? _countryEMFactory.CreateModel(entity.Country) : null;

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
