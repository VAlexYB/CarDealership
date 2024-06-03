using CarDealership.Core.Abstractions.Repositories;
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
    }
}
