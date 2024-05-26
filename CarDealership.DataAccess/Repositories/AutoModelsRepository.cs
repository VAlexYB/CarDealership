using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;

namespace CarDealership.DataAccess.Repositories
{
    public class AutoModelsRepository : BaseRepository<AutoModel, AutoModelEntity, BaseFilter>, IAutoModelsRepository
    {
        public AutoModelsRepository(CarDealershipDbContext context, IEntityModelFactory<AutoModel, AutoModelEntity> factory) : base(context, factory)
        {
        }
    }
}
