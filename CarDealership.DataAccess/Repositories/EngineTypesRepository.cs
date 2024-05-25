using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;

namespace CarDealership.DataAccess.Repositories
{
    public class EngineTypesRepository : BaseRepository<EngineType, EngineTypeEntity, BaseFilter>, IEngineTypesRepository
    {
        public EngineTypesRepository(CarDealershipDbContext context, EngineTypeEMFactory factory) : base(context, factory)
        {
        }
    }
}
