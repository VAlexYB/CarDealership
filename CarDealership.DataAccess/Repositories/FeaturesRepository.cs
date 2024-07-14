using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;
using Microsoft.Extensions.Caching.Distributed;

namespace CarDealership.DataAccess.Repositories
{
    public class FeaturesRepository : BaseRepository<Feature, FeatureEntity, BaseFilter>, IFeaturesRepository
    {
        public FeaturesRepository(CarDealershipDbContext context, IEntityModelFactory<Feature, FeatureEntity> factory, IDistributedCache cache) : base(context, factory, cache)
        {
        }
    }
}
