using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Exceptions;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Attributes;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;

namespace CarDealership.DataAccess.Repositories
{
    public class PromotionsRepository : BaseRepository<Promotion, PromotionEntity, BaseFilter>, IPromotionsRepository
    {
        public PromotionsRepository(CarDealershipDbContext context, IEntityModelFactory<Promotion, PromotionEntity> factory, IDistributedCache cache)
            : base(context, factory, cache)
        {
        }

        public async Task<Promotion> GetByPromocode(string promocode)
        {
            PromotionEntity entity = await _dbSet
                .Include(p => p.AppliableConfigs)
                .ThenInclude(c => c.AutoModel)
                .ThenInclude(m => m.Brand)
                .Include(p => p.AppliableConfigs)
                .ThenInclude(c => c.Equipment)
                .Include(p => p.AppliableConfigs)
                .ThenInclude(c => c.Engine)
                .Include(p => p.AppliableConfigs)
                .ThenInclude(c => c.BodyType)
                .FirstOrDefaultAsync(p => p.Promocode == promocode);

            if (entity == null)
            {
                throw new ClientInformationException("Указанный промокод не обнаружен");
            }

            return _factory.CreateModel(entity);
        }

        public override async Task<Guid> InsertAsync(Promotion model)
        {
            var promotionEntity = _factory.CreateEntity(model);

            await AttributesHelper<PromotionEntity>.EnsureAttributesUniqueness(_dbSet, promotionEntity, _context);

            foreach(var config in promotionEntity.AppliableConfigs)
            {
                _context.Attach(config);
            }

            await _dbSet.AddAsync(promotionEntity);
            await _context.SaveChangesAsync();
            await _cache.RemoveAsync($"{typeof(Promotion).Name}_All");
            return promotionEntity.Id;
        }

        public override async Task<Guid> UpdateAsync(Promotion model)
        {
            var entity = _factory.CreateEntity(model);
            var existEntity = await _dbSet
                .FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (existEntity == null) throw new InvalidOperationException("На редактирование пришла сущность, не существующая в системе");
            await AttributesHelper<PromotionEntity>.EnsureAttributesUniqueness(_dbSet, entity, _context);
            _context.Entry(existEntity).CurrentValues.SetValues(entity);

            foreach (var config in entity.AppliableConfigs)
            {
                var tracked = await _context.AutoConfigurations.FirstOrDefaultAsync(c => c.Id == config.Id);
                existEntity.AppliableConfigs.Add(tracked ?? config);  
            }

            foreach (var participant in entity.Participants)
            {
                var tracked = await _context.Users.FirstOrDefaultAsync(u => u.Id == participant.Id);
                existEntity.Participants.Add(tracked ?? participant);
            }

            await _context.SaveChangesAsync();
            await _cache.RemoveAsync($"{typeof(Promotion).Name}_{existEntity.Id}");
            await _cache.RemoveAsync($"{typeof(Promotion).Name}_All");
            return entity.Id;
        }

        public override async Task<List<Promotion>> GetAllAsync()
        {
            DateTime now = DateTime.UtcNow;

            var entities = await _dbSet
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Where(x => x.StartDate <= now && now <= x.EndDate)
            .OrderBy(x => x.Id)
            .ToListAsync();

            return entities.Select(entity => _factory.CreateModel(entity)).ToList();
        }
    }
}
