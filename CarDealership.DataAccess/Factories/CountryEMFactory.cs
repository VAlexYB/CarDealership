using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class CountryEMFactory : IEntityModelFactory<Country, CountryEntity>
    {
        private readonly BrandEMFactory _brandEMFactory;

        public CountryEMFactory(BrandEMFactory brandEMFactory)
        {
            _brandEMFactory = brandEMFactory ?? throw new ArgumentNullException(nameof(brandEMFactory));
        }

        public CountryEntity CreateEntity(Country model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var brandEntities = model.Brands
                .Select(brand => _brandEMFactory.CreateEntity(brand))
                .ToList();

            var entity = new CountryEntity
            {
                Id = model.Id,
                Name = model.Name,
                IsDeleted = model.IsDeleted,
                Brands = brandEntities
            };

            return entity;
        }

        public Country CreateModel(CountryEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var countryCreateResult = Country.Create(
                entity.Id,
                entity.Name,
                entity.IsDeleted
            );

            if (countryCreateResult.IsFailure)
            {
                throw new InvalidOperationException(countryCreateResult.Error);
            }

            var country = countryCreateResult.Value;

            foreach (var brandEntity in entity.Brands)
            {
                var brandModel = _brandEMFactory.CreateModel(brandEntity);
                country.AddBrand(brandModel);
            }

            return country;
        }
    }
}
