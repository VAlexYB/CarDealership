using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.DataAccess.Repositories
{
    public class BaseRepository<M, E, F> : IGenericRepository<M, F>
        where M : BaseModel
        where E : BaseEntity 
        where F : BaseFilter
    {
        protected readonly CarDealershipDbContext _context;
        protected readonly IEntityModelFactory<M, E> _factory;
        protected readonly DbSet<E> _dbSet;
        public BaseRepository(CarDealershipDbContext context, IEntityModelFactory<M, E> factory)
        {
            _context = context;
            _factory = factory;
            _dbSet = context.Set<E>();  
        }


        // при необходимости можно сделать переопределение, пока virtual не ставлю

        public async Task<List<M>> GetAllAsync()
        {
            var entities =  await _dbSet
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .ToListAsync();

            return entities.Select(entity => _factory.CreateModel(entity)).ToList();
        }

        public async Task<List<M>> GetFilteredAsync(F filter)
        {
            var entities = await _dbSet
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .ToListAsync();

            return entities.Select(entity => _factory.CreateModel(entity)).ToList();
        }

        public async Task<M> GetByIdAsync(Guid entityId)
        {
            var entity =  await _dbSet.FindAsync(entityId);

            return _factory.CreateModel(entity);
        }

        public async Task InsertAsync(M model)
        {
            var entity = _factory.CreateEntity(model);
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(M model)
        {
            var entity = _factory.CreateEntity(model);
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid entityId)
        {
            var entity = await _dbSet.FindAsync(entityId);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid entityId)
        {
            return await _dbSet
                .AsNoTracking()
                .AnyAsync(e => e.Id == entityId);
        }
    }
}
