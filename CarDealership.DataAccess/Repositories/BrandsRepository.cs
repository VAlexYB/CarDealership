using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;

namespace CarDealership.DataAccess.Repositories
{
    public class BrandsRepository : BaseRepository<Brand, BrandEntity, BaseFilter>, IBrandsRepository
    {
        public BrandsRepository(CarDealershipDbContext context, IEntityModelFactory<Brand, BrandEntity> factory) : base(context, factory)
        {
        }

        public override async Task<Guid> UpdateAsync(Brand model)
        {
            var entity = _factory.CreateEntity(model);
            var existEntity = await _dbSet.FindAsync(entity.Id);

            if (existEntity == null) throw new InvalidOperationException();
            _context.Entry(existEntity).CurrentValues.SetValues(entity);
            if (existEntity.CountryId != entity.CountryId)
            {
                existEntity.CountryId = entity.CountryId;
            }

            await _context.SaveChangesAsync();
            return existEntity.Id;
        }
    }
}
