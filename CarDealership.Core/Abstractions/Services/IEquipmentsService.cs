using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Services
{
    public interface IEquipmentsService : IGenericService<Equipment, BaseFilter>
    {
        public Task<Guid> CreateOrEditAsync(Equipment equipment, List<Guid> featureIds);

        public Task RemoveFeatureFromEquipment(Guid equipmentId, Guid featureId);
    }
}
