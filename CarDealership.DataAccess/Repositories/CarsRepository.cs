using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;
using Microsoft.Extensions.Caching.Distributed;

namespace CarDealership.DataAccess.Repositories
{
    public class CarsRepository : BaseRepository<Car, CarEntity, BaseFilter>, ICarsRepository
    {
        public CarsRepository(CarDealershipDbContext context, IEntityModelFactory<Car, CarEntity> factory,  IDistributedCache cache) : base(context, factory, cache)
        {
        }

        public override async Task<Guid> UpdateAsync(Car model)
        {
            try
            {
                var entity = _factory.CreateEntity(model);
                var existEntity = await _dbSet.FindAsync(entity.Id);

                if (existEntity == null) throw new InvalidOperationException();
                _context.Entry(existEntity).CurrentValues.SetValues(entity);
                if (existEntity.AutoConfigurationId != entity.AutoConfigurationId)
                {
                    existEntity.AutoConfigurationId = entity.AutoConfigurationId;
                }

                await _context.SaveChangesAsync();
                await _cache.RemoveAsync($"{model.GetType().Name}_{existEntity.Id}");
                await _cache.RemoveAsync($"{model.GetType().Name}_All");
                return existEntity.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
