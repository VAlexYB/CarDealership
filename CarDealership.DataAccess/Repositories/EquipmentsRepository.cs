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
            var equipFeature = await _equipFeaturesSet.FirstOrDefaultAsync(ef => ef.EquipmentId == equipmentId && ef.FeatureId == featureId);

            if (equipFeature == null)
            {
                throw new InvalidOperationException("При удалении связки фичи с комплектацией не найдена соответствиующая запись в БД");
            }

            _equipFeaturesSet.Remove(equipFeature);
            await _context.SaveChangesAsync();
        }

        public override async Task<Guid> UpdateAsync(Equipment model)
        {
            var entity = _factory.CreateEntity(model);
            var existEntity = await _dbSet.FindAsync(entity.Id);

            if (existEntity == null) throw new InvalidOperationException("На редактирование пришла комплектация, не существующая в системе");
            _context.Entry(existEntity).CurrentValues.SetValues(entity);
            if (existEntity.AutoModelId != entity.AutoModelId)
            {
                existEntity.AutoModelId = entity.AutoModelId;
            }

            await _context.SaveChangesAsync();
            await _cache.RemoveAsync($"{model.GetType().Name}_{existEntity.Id}");
            await _cache.RemoveAsync($"{model.GetType().Name}_All");
            return existEntity.Id;
        }

        public override async Task<List<Equipment>> GetFilteredAsync(EquipmentsFilter filter)
        {
            var entities = await _dbSet
            .AsNoTracking()
            .Where(e => !e.IsDeleted)
            .WhereIf(filter.AutoModelId.HasValue, e => e.AutoModelId == filter.AutoModelId)
            .WhereIf(filter.Features.Any(), e => filter.Features.All(featureId => e.equipmentFeatures.Any(ef => ef.FeatureId == featureId)))
            .OrderBy(e => e.Id)
            .ToListAsync();

            return entities.Select(entity => _factory.CreateModel(entity)).ToList();
        }
    }
}
