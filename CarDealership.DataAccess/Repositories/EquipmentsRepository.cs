using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Filters;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Extensions;
using CarDealership.DataAccess.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using EquipmentFeatureEntity = CarDealership.DataAccess.Entities.EquipmentFeatureEntity;

namespace CarDealership.DataAccess.Repositories
{
    public class EquipmentsRepository : BaseRepository<Equipment, EquipmentEntity, EquipmentsFilter>, IEquipmentsRepository
    {
        protected readonly DbSet<EquipmentFeatureEntity> _equipFeaturesSet;
        public EquipmentsRepository(CarDealershipDbContext context, IEntityModelFactory<Equipment, EquipmentEntity> factory,  IDistributedCache cache) : base(context, factory, cache)
        {
            _equipFeaturesSet = _context.Set<EquipmentFeatureEntity>();
        }


        public async Task RemoveFeatureFromEquipment(Guid equipmentId, Guid featureId)
        {
            try
            {
                var equipFeature = await _equipFeaturesSet.FirstOrDefaultAsync(ef => ef.EquipmentId == equipmentId && ef.FeatureId == featureId);

                if (equipFeature == null)
                {
                    throw new InvalidOperationException("Такая сущность не найдена");
                }

                _equipFeaturesSet.Remove(equipFeature);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override async Task<Guid> UpdateAsync(Equipment model)
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

                await _context.SaveChangesAsync();
                return existEntity.Id;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public override async Task<List<Equipment>> GetFilteredAsync(EquipmentsFilter filter)
        {
            try
            {
                var entities = await _dbSet
                .AsNoTracking()
                .Where(e => !e.IsDeleted)
                .WhereIf(filter.AutoModelId.HasValue, e => e.AutoModelId == filter.AutoModelId)
                .OrderBy(e => e.Id)
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
