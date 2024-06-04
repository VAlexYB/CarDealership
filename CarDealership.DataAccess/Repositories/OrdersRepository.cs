using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Filters;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Extensions;
using CarDealership.DataAccess.Factories;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.DataAccess.Repositories
{
    public class OrdersRepository : BaseRepository<Order, OrderEntity, OrdersFilter>, IOrdersRepository
    {
        public OrdersRepository(CarDealershipDbContext context, IEntityModelFactory<Order, OrderEntity> factory) : base(context, factory)
        {
        }

        public override async Task<Guid> UpdateAsync(Order model)
        {
            var entity = _factory.CreateEntity(model);
            var existEntity = await _dbSet.FindAsync(entity.Id);

            if (existEntity == null) throw new InvalidOperationException();
            _context.Entry(existEntity).CurrentValues.SetValues(entity);
            if (existEntity.AutoConfigurationId != entity.AutoConfigurationId)
            {
                existEntity.AutoConfigurationId = entity.AutoConfigurationId;
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

        public override async Task<List<Order>> GetFilteredAsync(OrdersFilter filter)
        {
            var entities = await _dbSet
                .AsNoTracking()
                .Where(d => !d.IsDeleted)
                .WhereIf(filter.CustomerId.HasValue, d => d.CustomerId == filter.CustomerId)
                .WhereIf(filter.ManagerId.HasValue, d => d.ManagerId == filter.ManagerId)
                .WhereIf(filter.OrderStatus.HasValue, d => d.Status == filter.OrderStatus)
                .OrderBy(x => x.Id)
                .ToListAsync();

            return entities.Select(entity => _factory.CreateModel(entity)).ToList();
        }
    }
}
