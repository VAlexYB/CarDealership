﻿using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Services
{
    public interface IEquipmentsService : IGenericService<Equipment, BaseFilter>
    {
        Task<Guid> CreateOrEditAsync(Equipment equipment, List<Guid> featureIds);

        Task RemoveFeatureFromEquipment(Guid equipmentId, Guid featureId);
        Task AddFeatureToEquipment(Guid equipmentId, Guid featureId);
    }
}
