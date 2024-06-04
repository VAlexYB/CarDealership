using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;

namespace CarDealership.DataAccess.Repositories
{
    public class DealsRepository : BaseRepository<Deal, DealEntity, BaseFilter>, IDealsRepository
    {
        public DealsRepository(CarDealershipDbContext context, IEntityModelFactory<Deal, DealEntity> factory) : base(context, factory)
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
    }
}
