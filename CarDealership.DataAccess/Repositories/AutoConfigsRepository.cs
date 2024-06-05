using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Filters;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Extensions;
using CarDealership.DataAccess.Factories;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.DataAccess.Repositories
{
    public class AutoConfigsRepository : BaseRepository<AutoConfiguration, AutoConfigurationEntity, ConfigurationsFilter>, IAutoConfigsRepository
    {
        public AutoConfigsRepository(CarDealershipDbContext context, IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity> factory) : base(context, factory)
        {
        }

        public override async Task<Guid> UpdateAsync(AutoConfiguration model)
        {
            try
            {
                var entity = _factory.CreateEntity(model);
                var existEntity = await _dbSet.FindAsync(entity.Id);

                if (existEntity == null) throw new InvalidOperationException();
                _context.Entry(existEntity).CurrentValues.SetValues(entity);
                if (existEntity.AutoModelId != entity.AutoModelId)
                {
                    existEntity.AutoModelId = entity.AutoModelId;
                }

                if (existEntity.BodyTypeId != entity.BodyTypeId)
                {
                    existEntity.BodyTypeId = entity.BodyTypeId;
                }

                if (existEntity.DriveTypeId != entity.DriveTypeId)
                {
                    existEntity.DriveTypeId = entity.DriveTypeId;
                }

                if (existEntity.EngineId != entity.EngineId)
                {
                    existEntity.EngineId = entity.EngineId;
                }

                if (existEntity.ColorId != entity.ColorId)
                {
                    existEntity.ColorId = entity.ColorId;
                }

                if (existEntity.EquipmentId != entity.EquipmentId)
                {
                    existEntity.EquipmentId = entity.EquipmentId;
                }

                await _context.SaveChangesAsync();
                return existEntity.Id;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public override async Task<List<AutoConfiguration>> GetFilteredAsync(ConfigurationsFilter filter)
        {
            try
            {
                var entities = await _dbSet
               .AsNoTracking()
               .Where(ac => !ac.IsDeleted)
               .WhereIf(filter.BrandId.HasValue, ac => ac.AutoModel.BrandId == filter.BrandId)
               .WhereIf(filter.AutoModelId.HasValue, ac => ac.AutoModelId == filter.AutoModelId)
               .WhereIf(filter.EquipmentId.HasValue, ac => ac.EquipmentId == filter.EquipmentId)
               .WhereIf(filter.BodyTypeId.HasValue, ac => ac.BodyTypeId == filter.BodyTypeId)
               .WhereIf(filter.EngineId.HasValue, ac => ac.EngineId == filter.EngineId)
               .WhereIf(filter.ColorId.HasValue, ac => ac.ColorId == filter.ColorId)
               .WhereIf(filter.DriveTypeId.HasValue, ac => ac.DriveTypeId == filter.DriveTypeId)
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
