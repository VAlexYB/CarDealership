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
    public class AutoModelsRepository : BaseRepository<AutoModel, AutoModelEntity, AutoModelsFilter>, IAutoModelsRepository
    {
        public AutoModelsRepository(CarDealershipDbContext context, IEntityModelFactory<AutoModel, AutoModelEntity> factory, IDistributedCache cache) : base(context, factory, cache)
        {
        }

        public override async Task<Guid> UpdateAsync(AutoModel model)
        {
            try
            {
                var entity = _factory.CreateEntity(model);
                var existEntity = await _dbSet.FindAsync(entity.Id);

                if (existEntity == null) throw new InvalidOperationException();
                _context.Entry(existEntity).CurrentValues.SetValues(entity);
                if (existEntity.BrandId != entity.BrandId)
                {
                    existEntity.BrandId = entity.BrandId;
                }

                await _context.SaveChangesAsync();
                return existEntity.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async override Task<List<AutoModel>> GetFilteredAsync(AutoModelsFilter filter)
        {
            try
            {
                var entities = await _dbSet
                .AsNoTracking()
                .Where(am => !am.IsDeleted)
                .WhereIf(filter.BrandId.HasValue, am => am.BrandId == filter.BrandId)
                .OrderBy(am => am.Id)
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
