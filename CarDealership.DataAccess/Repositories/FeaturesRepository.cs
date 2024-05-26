using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;

namespace CarDealership.DataAccess.Repositories
{
    public class FeaturesRepository : BaseRepository<Feature, FeatureEntity, BaseFilter>, IFeaturesRepository
    {
        public FeaturesRepository(CarDealershipDbContext context, IEntityModelFactory<Feature, FeatureEntity> factory) : base(context, factory)
        {
        }
    }
}
