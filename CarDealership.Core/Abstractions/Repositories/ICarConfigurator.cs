using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Repositories
{
    public interface ICarConfigurator
    {
        Task<List<AutoConfiguration>> GetFilteredConfigurations();
        void SelectBrand(Guid brandId);

        void SelectModel(Guid modelId);

        void SelectEquipment(Guid equipmentId);

        void SelectBodyType(Guid bodyTypeId);

        void SelectEngine(Guid engineId);

        void SelectColor(Guid colorId);
    }
}
