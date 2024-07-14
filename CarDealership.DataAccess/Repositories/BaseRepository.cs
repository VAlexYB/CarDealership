using System.Net.Cache;
using System.Text.Json;
using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

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
        protected readonly IDistributedCache _cache;
        public BaseRepository(CarDealershipDbContext context, IEntityModelFactory<M, E> factory, IDistributedCache cache)
        {
            _context = context;
            _factory = factory;
            _dbSet = context.Set<E>();
            _cache = cache;
        }


        // при необходимости можно сделать переопределение, пока virtual не ставлю

        public virtual async Task<List<M>> GetAllAsync()
        {
            try
            {
                var key = $"{typeof(M).Name}_All";
                
                var cachedData = await _cache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    var cachedModels = JsonSerializer.Deserialize<List<M>>(cachedData);
                    if (cachedModels != null)
                    {
                        Console.WriteLine("Данные получены из кэша");
                        return cachedModels;
                    }
                }
                
                var entities = await _dbSet
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Id)
                .ToListAsync();

                var models =  entities.Select(entity => _factory.CreateModel(entity)).ToList();

                if (models.Count != 0)
                {
                    await _cache.SetStringAsync(key, JsonSerializer.Serialize(models));
                }
                
                return models;
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
                var key = $"{typeof(M).Name}_{entityId}";
                
                var cachedData = await _cache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    var cachedModels = JsonSerializer.Deserialize<M>(cachedData);
                    if (cachedModels != null)
                    {
                        Console.WriteLine("Данные получены из кэша");
                        return cachedModels;
                    }
                }
                var entity = await _dbSet.FindAsync(entityId);
                if (entity != null)
                {
                    await _cache.SetStringAsync(key, JsonSerializer.Serialize(entity));
                }

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
                await _cache.RemoveAsync($"{typeof(M).Name}_All");
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
                await _cache.RemoveAsync($"{typeof(M).Name}_{existEntity.Id}");
                await _cache.RemoveAsync($"{typeof(M).Name}_All");
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
                    entity.DeletedDate = DateTime.UtcNow;
                    //_dbSet.Remove(entity);
                }
                await _context.SaveChangesAsync();
                await _cache.RemoveAsync($"{typeof(M).Name}_All");
                await _cache.RemoveAsync($"{typeof(M).Name}_{entityId}");
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
                var key = $"{typeof(M).Name}_{entityId}";
                
                var cachedData = await _cache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    var cachedModels = JsonSerializer.Deserialize<M>(cachedData);
                    if (cachedModels != null)
                    {
                        Console.WriteLine("Данные получены из кэша");
                        return true;
                    }
                }
                
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
