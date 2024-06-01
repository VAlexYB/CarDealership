using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Repositories
{
    public interface IEquipmentsRepository : IGenericRepository<Equipment, BaseFilter>
    {
        public Task RemoveFeatureFromEquipment(Guid equipmentId, Guid featureId);
    }
}
