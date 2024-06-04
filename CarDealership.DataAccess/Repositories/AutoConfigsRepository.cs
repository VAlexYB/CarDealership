using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;

namespace CarDealership.DataAccess.Repositories
{
    public class AutoConfigsRepository : BaseRepository<AutoConfiguration, AutoConfigurationEntity, BaseFilter>, IAutoConfigsRepository
    {
        public AutoConfigsRepository(CarDealershipDbContext context, IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity> factory) : base(context, factory)
        {
        }

        public override async Task<Guid> UpdateAsync(AutoConfiguration model)
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
    }
}
