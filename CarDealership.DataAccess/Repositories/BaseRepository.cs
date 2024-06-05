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

        public virtual async Task<List<M>> GetAllAsync()
        {
            try
            {
                var entities = await _dbSet
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Id)
                .ToListAsync();

                return entities.Select(entity => _factory.CreateModel(entity)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<List<M>> GetFilteredAsync(F filter)
        {
            try
            {
                var entities = await _dbSet
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Id)
                .ToListAsync();

                return entities.Select(entity => _factory.CreateModel(entity)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<M> GetByIdAsync(Guid entityId)
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

        public async Task<Guid> InsertAsync(M model)
        {
            try
            {
                var entity = _factory.CreateEntity(model);
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity.Id;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public virtual async Task<Guid> UpdateAsync(M model)
        {
            try
            {
                var entity = _factory.CreateEntity(model);
                var existEntity = await _dbSet.FindAsync(entity.Id);
                if (existEntity == null) throw new InvalidOperationException("");
                _context.Entry(existEntity).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return entity.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Guid> DeleteAsync(Guid entityId)
        {
            try
            {
                var entity = await _dbSet.FindAsync(entityId);
                if (entity != null)
                {
                    entity.IsDeleted = true;
                    //_dbSet.Remove(entity);
                }
                await _context.SaveChangesAsync();
                return entityId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ExistsAsync(Guid entityId)
        {
            try
            {
                return await _dbSet
                .AsNoTracking()
                .AnyAsync(e => e.Id == entityId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
