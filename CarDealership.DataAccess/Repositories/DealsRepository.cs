using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Filters;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Extensions;
using CarDealership.DataAccess.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace CarDealership.DataAccess.Repositories
{
    public class DealsRepository : BaseRepository<Deal, DealEntity, DealsFilter>, IDealsRepository
    {
        public DealsRepository(CarDealershipDbContext context, IEntityModelFactory<Deal, DealEntity> factory, IDistributedCache cache) : base(context, factory, cache)
        {
        }

        public override async Task<Guid> UpdateAsync(Deal model)
        {
            var entity = _factory.CreateEntity(model);
            var existEntity = await _dbSet.FindAsync(entity.Id);

            if (existEntity == null) throw new InvalidOperationException();
            _context.Entry(existEntity).CurrentValues.SetValues(entity);
            if (existEntity.CarId != entity.CarId)
            {
                existEntity.CarId = entity.CarId;
            }

            if (existEntity.ManagerId != entity.ManagerId)
            {
                existEntity.ManagerId = entity.ManagerId;
            }

            if (existEntity.CustomerId != entity.CustomerId)
            {
                existEntity.CustomerId = entity.CustomerId;
            }
            await _context.SaveChangesAsync();
            return existEntity.Id;
        }

        public override async Task<List<Deal>> GetFilteredAsync(DealsFilter filter)
        {
            try
            {
                var entities = await _dbSet
                .AsNoTracking()
                .Where(d => !d.IsDeleted)
                .WhereIf(filter.CustomerId.HasValue, d => d.CustomerId == filter.CustomerId)
                .WhereIf(filter.ManagerId.HasValue, d => d.ManagerId == filter.ManagerId)
                .WhereIf(filter.DealStatus.HasValue, d => d.Status == filter.DealStatus)
                .OrderBy(x => x.Id)
                .ToListAsync();

                return entities.Select(entity => _factory.CreateModel(entity)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Deal>> GetDealsWithoutManager()
        {
            try
            {
                var entities = await _dbSet
                .AsNoTracking()
                .Where(d => !d.IsDeleted)
                .Where(d => d.ManagerId == null)
                .OrderBy(x => x.Id)
                .ToListAsync();

                return entities.Select(entity => _factory.CreateModel(entity)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
