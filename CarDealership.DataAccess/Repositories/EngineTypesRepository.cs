using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;
using Microsoft.Extensions.Caching.Distributed;

namespace CarDealership.DataAccess.Repositories
{
    public class EngineTypesRepository : BaseRepository<EngineType, EngineTypeEntity, BaseFilter>, IEngineTypesRepository
    {
        public EngineTypesRepository(CarDealershipDbContext context, IEntityModelFactory<EngineType, EngineTypeEntity> factory, IDistributedCache cache) : base(context, factory, cache)
        {
        }
    }
}
