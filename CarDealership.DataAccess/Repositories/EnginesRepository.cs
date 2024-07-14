using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;
using Microsoft.Extensions.Caching.Distributed;

namespace CarDealership.DataAccess.Repositories
{
    public class EnginesRepository : BaseRepository<Engine, EngineEntity, BaseFilter>, IEnginesRepository
    {
        public EnginesRepository(CarDealershipDbContext context, IEntityModelFactory<Engine, EngineEntity> factory,  IDistributedCache cache) : base(context, factory, cache)
        {
        }

        public override async Task<Guid> UpdateAsync(Engine model)
        {
            try
            {
                var entity = _factory.CreateEntity(model);
                var existEntity = await _dbSet.FindAsync(entity.Id);

                if (existEntity == null) throw new InvalidOperationException();
                _context.Entry(existEntity).CurrentValues.SetValues(entity);
                if (existEntity.EngineTypeId != entity.EngineTypeId)
                {
                    existEntity.EngineTypeId = entity.EngineTypeId;
                }

                if (existEntity.TransmissionTypeId != entity.TransmissionTypeId)
                {
                    existEntity.TransmissionTypeId = entity.TransmissionTypeId;
                }
                await _context.SaveChangesAsync();
                return existEntity.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
