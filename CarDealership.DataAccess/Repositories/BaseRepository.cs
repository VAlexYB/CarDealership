using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Attributes;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Extensions;
using CarDealership.DataAccess.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

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

        public virtual async Task<List<M>> GetAllAsync()
        {
            var key = $"{typeof(M).Name}_All";
                
            var cachedData = await _cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(cachedData))
            {
                var cachedModels = JsonConvert.DeserializeObject<List<M>>(cachedData);
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
                await _cache.SetStringAsync(key, JsonConvert.SerializeObject(models));
            }
                
            return models;
        }

        public virtual async Task<List<M>> GetFilteredAsync(F filter)
        {
            var entities = await _dbSet
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .WhereIf(filter.Guids != null, x => filter.Guids.Contains(x.Id))
            .OrderBy(x => x.Id)
            .ToListAsync();

            return entities.Select(entity => _factory.CreateModel(entity)).ToList();
        }

        public virtual async Task<M> GetByIdAsync(Guid entityId)
        {
            var key = $"{typeof(M).Name}_{entityId}";
                
            var cachedData = await _cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(cachedData))
            {
                var cachedModels = JsonConvert.DeserializeObject<M>(cachedData);
                if (cachedModels != null)
                {
                    Console.WriteLine("Данные получены из кэша");
                    return cachedModels;
                }
            }
            var entity = await _dbSet.FindAsync(entityId);
            if (entity != null)
            {
                await _cache.SetStringAsync(key, JsonConvert.SerializeObject(entity));
            }

            return _factory.CreateModel(entity);           
        }

        public virtual async Task<Guid> InsertAsync(M model)
        {
            var entity = _factory.CreateEntity(model);

            await AttributesHelper<E>.EnsureAttributesUniqueness(_dbSet, entity, _context);

            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            await _cache.RemoveAsync($"{typeof(M).Name}_All");
            return entity.Id;
        }

        public virtual async Task<Guid> UpdateAsync(M model)
        {
            var entity = _factory.CreateEntity(model);
            var existEntity = await _dbSet.FindAsync(entity.Id);
            if (existEntity == null) throw new InvalidOperationException("На редактирование пришла сущность, не существующая в системе");
            await AttributesHelper<E>.EnsureAttributesUniqueness(_dbSet, entity, _context);
            _context.Entry(existEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            await _cache.RemoveAsync($"{typeof(M).Name}_{existEntity.Id}");
            await _cache.RemoveAsync($"{typeof(M).Name}_All");
            return entity.Id;
        }

        public async Task<Guid> DeleteAsync(Guid entityId)
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

        public async Task<bool> ExistsAsync(Guid entityId)
        {
            var key = $"{typeof(M).Name}_{entityId}";
                
            var cachedData = await _cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(cachedData))
            {
                var cachedModels = JsonConvert.DeserializeObject<M>(cachedData);
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

        //public IQueryable<M> Query()
        //{
        //    var projection = SqlProjectionFactory.CreateProjectionExpression<E, M>();
        //    return _dbSet
        //        .Select(projection)
        //        .AsQueryable();
        //}
    }
}
