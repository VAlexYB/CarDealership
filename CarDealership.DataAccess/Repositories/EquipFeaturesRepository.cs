using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;

namespace CarDealership.DataAccess.Repositories
{
    public class EquipFeaturesRepository : BaseRepository<EquipmentFeature, EquipmentFeatureEntity, BaseFilter>, IEquipFeaturesRepository
    {
        public EquipFeaturesRepository(CarDealershipDbContext context, IEntityModelFactory<EquipmentFeature, EquipmentFeatureEntity> factory) : base(context, factory)
        {
        }
    }
}
