using CarDealership.Core.Filters;
using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Repositories
{
    public interface IEquipmentsRepository : IGenericRepository<Equipment, EquipmentsFilter>
    {
        public Task RemoveFeatureFromEquipment(Guid equipmentId, Guid featureId);
    }
}
