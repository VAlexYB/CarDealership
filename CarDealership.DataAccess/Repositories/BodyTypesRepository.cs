using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;
using Microsoft.Extensions.Caching.Distributed;

namespace CarDealership.DataAccess.Repositories
{
    public class BodyTypesRepository : BaseRepository<BodyType, BodyTypeEntity, BaseFilter>, IBodyTypesRepository
    {
        public BodyTypesRepository(CarDealershipDbContext context, IEntityModelFactory<BodyType, BodyTypeEntity> factory,  IDistributedCache cache) : base(context, factory, cache)
        {
        }
    }
}
