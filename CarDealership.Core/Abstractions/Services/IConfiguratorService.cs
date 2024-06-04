using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Services
{
    public interface IConfiguratorService
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
