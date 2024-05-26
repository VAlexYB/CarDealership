using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;

namespace CarDealership.DataAccess.Repositories
{
    public class EnginesRepository : BaseRepository<Engine, EngineEntity, BaseFilter>, IEnginesRepository
    {
        public EnginesRepository(CarDealershipDbContext context, IEntityModelFactory<Engine, EngineEntity> factory) : base(context, factory)
        {
        }
    }
}
