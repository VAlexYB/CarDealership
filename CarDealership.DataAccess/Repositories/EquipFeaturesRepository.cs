using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;

namespace CarDealership.DataAccess.Repositories
{
    public class EquipFeaturesRepository : BaseRepository<Core.Models.EquipmentFeature, Entities.EquipmentFeatureEntity, BaseFilter>, IEquipFeaturesRepository
    {
        public EquipFeaturesRepository(CarDealershipDbContext context, IEntityModelFactory<Core.Models.EquipmentFeature, Entities.EquipmentFeatureEntity> factory) : base(context, factory)
        {
        }
    }
}
