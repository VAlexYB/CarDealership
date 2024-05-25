using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class EquipmentsService : BaseService<Equipment, BaseFilter>, IEquipmentsService
    {
        private readonly IEquipFeaturesRepository _equipFeaturesRepository;
        public EquipmentsService(IEquipmentsRepository repository, IEquipFeaturesRepository equipFeaturesRepository) : base(repository)
        {
            _equipFeaturesRepository = equipFeaturesRepository ?? throw new ArgumentNullException(nameof(equipFeaturesRepository));
        }

        //public void RemoveFeature(Guid featureId)
        //{
            
        //    await _equipFeaturesRepository.DeleteAsync() надо прописать логику удаления фичи у комплектации
        //}

    }
}
