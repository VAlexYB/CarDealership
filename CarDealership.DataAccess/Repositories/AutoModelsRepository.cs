using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.DataAccess.Repositories
{
    public class AutoModelsRepository : BaseRepository<AutoModel, AutoModelEntity, BaseFilter>, IAutoModelsRepository
    {
        public AutoModelsRepository(CarDealershipDbContext context, IEntityModelFactory<AutoModel, AutoModelEntity> factory) : base(context, factory)
        {
        }

        public override async Task<Guid> UpdateAsync(AutoModel model)
        {
            var entity = _factory.CreateEntity(model);
            var existEntity = await _dbSet.FindAsync(entity.Id);

            if (existEntity == null) throw new InvalidOperationException();
            _context.Entry(existEntity).CurrentValues.SetValues(entity);
            if(existEntity.BrandId != entity.BrandId)
            {
                existEntity.BrandId = entity.BrandId;
            }
            
            await _context.SaveChangesAsync();
            return existEntity.Id;
        }
    }
}
