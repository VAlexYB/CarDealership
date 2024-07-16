using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Filters;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Extensions;
using CarDealership.DataAccess.Factories;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace CarDealership.DataAccess.Repositories
{
    public class OrdersRepository : BaseRepository<Order, OrderEntity, OrdersFilter>, IOrdersRepository
    {
        public OrdersRepository(CarDealershipDbContext context, IEntityModelFactory<Order, OrderEntity> factory,  IDistributedCache cache) : base(context, factory, cache)
        {
        }

        public override async Task<Order> GetByIdAsync(Guid entityId)
        {
            try
            {
                var entity = await _dbSet.FindAsync(entityId);
                return _factory.CreateModel(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public override async Task<List<Order>> GetAllAsync()
        {
            try
            {
                var entities = await _dbSet
                .AsNoTracking()
                .Where(d => !d.IsDeleted)
                .OrderBy(x => x.Id)
                .ToListAsync();

                return entities.Select(entity => _factory.CreateModel(entity)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override async Task<Guid> UpdateAsync(Order model)
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

                if (existEntity.ManagerId != entity.ManagerId)
                {
                    existEntity.ManagerId = entity.ManagerId;
                }

                if (existEntity.CustomerId != entity.CustomerId)
                {
                    existEntity.CustomerId = entity.CustomerId;
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

        public override async Task<List<Order>> GetFilteredAsync(OrdersFilter filter)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Order>> GetOrdersWithoutManager()
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
