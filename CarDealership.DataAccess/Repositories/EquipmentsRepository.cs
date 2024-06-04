﻿using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;
using Microsoft.EntityFrameworkCore;
using EquipmentFeatureEntity = CarDealership.DataAccess.Entities.EquipmentFeatureEntity;

namespace CarDealership.DataAccess.Repositories
{
    public class EquipmentsRepository : BaseRepository<Equipment, EquipmentEntity, BaseFilter>, IEquipmentsRepository
    {
        protected readonly DbSet<EquipmentFeatureEntity> _equipFeaturesSet;
        public EquipmentsRepository(CarDealershipDbContext context, IEntityModelFactory<Equipment, EquipmentEntity> factory) : base(context, factory)
        {
            _equipFeaturesSet = _context.Set<EquipmentFeatureEntity>();
        }


        public async Task RemoveFeatureFromEquipment(Guid equipmentId, Guid featureId)
        {
            var equipFeature = await _equipFeaturesSet.FirstOrDefaultAsync(ef => ef.EquipmentId == equipmentId && ef.FeatureId == featureId);

            if(equipFeature == null)
            {
                throw new InvalidOperationException("Такая сущность не найдена"); 
            }

            _equipFeaturesSet.Remove(equipFeature);
            await _context.SaveChangesAsync();
        }

        public override async Task<Guid> UpdateAsync(Equipment model)
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
    }
}
