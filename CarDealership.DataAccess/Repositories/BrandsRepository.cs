﻿using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;
using Microsoft.Extensions.Caching.Distributed;

namespace CarDealership.DataAccess.Repositories
{
    public class BrandsRepository : BaseRepository<Brand, BrandEntity, BaseFilter>, IBrandsRepository
    {
        public BrandsRepository(CarDealershipDbContext context, IEntityModelFactory<Brand, BrandEntity> factory, IDistributedCache cache) : base(context, factory, cache)
        {
        }

        public override async Task<Guid> UpdateAsync(Brand model)
        {
            try
            {
                var entity = _factory.CreateEntity(model);
                var existEntity = await _dbSet.FindAsync(entity.Id);

                if (existEntity == null) throw new InvalidOperationException();
                _context.Entry(existEntity).CurrentValues.SetValues(entity);
                if (existEntity.CountryId != entity.CountryId)
                {
                    existEntity.CountryId = entity.CountryId;
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
    }
}
