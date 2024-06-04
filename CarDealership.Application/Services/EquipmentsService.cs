using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Filters;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class EquipmentsService : BaseService<Equipment, EquipmentsFilter>, IEquipmentsService
    {
        private readonly IEquipFeaturesRepository _equipFeaturesRepository;
        private readonly IEquipmentsRepository _equipmentsRepository;
        public EquipmentsService
        (
            IEquipmentsRepository equipmentsRepository,
            IEquipFeaturesRepository equipFeaturesRepository
        ) : base(equipmentsRepository)
        {
            _equipFeaturesRepository = equipFeaturesRepository ?? throw new ArgumentNullException(nameof(equipFeaturesRepository));
            _equipmentsRepository = equipmentsRepository ?? throw new ArgumentNullException(nameof(equipmentsRepository));
        }

        public async Task<Guid> CreateOrEditAsync(Equipment model, List<Guid> featureIds)
        {
            bool _exist = await _equipmentsRepository.ExistsAsync(model.Id);
            Guid equipmentId = Guid.Empty;
            if (_exist)
            {
                equipmentId = await _equipmentsRepository.UpdateAsync(model);
            }
            else
            {
                equipmentId = await _equipmentsRepository.InsertAsync(model);

                foreach (var featureId in featureIds)
                {
                    var equipmentFeatureCreateResult = EquipmentFeature.Create(Guid.NewGuid(), equipmentId, featureId);
                    if (equipmentFeatureCreateResult.IsFailure)
                    {
                        throw new InvalidOperationException(equipmentFeatureCreateResult.Error);
                    }
                    var equipmentFeature = equipmentFeatureCreateResult.Value;
                    await _equipFeaturesRepository.InsertAsync(equipmentFeature);
                }
            }
            return equipmentId;
        }

        public async Task RemoveFeatureFromEquipment(Guid equipmentId, Guid featureId)
        {
            await _equipmentsRepository.RemoveFeatureFromEquipment(equipmentId, featureId);
        }

        public async Task AddFeatureToEquipment(Guid equipmentId, Guid featureId)
        {
            var equipmentFeatureCreateResult = EquipmentFeature.Create(Guid.NewGuid(), equipmentId, featureId);
            if (equipmentFeatureCreateResult.IsFailure)
            {
                throw new InvalidOperationException(equipmentFeatureCreateResult.Error);
            }
            var equipmentFeature = equipmentFeatureCreateResult.Value;
            await _equipFeaturesRepository.InsertAsync(equipmentFeature);
        }

    }
}
