using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class ConfiguratorService : IConfiguratorService
    {
        private readonly ICarConfigurator _configurator;
        public ConfiguratorService(ICarConfigurator configurator) 
        {
            _configurator = configurator;
        }
        public async Task<List<AutoConfiguration>> GetFilteredConfigurations()
        {
            return await _configurator.GetFilteredConfigurations();
        }

        public void SelectBodyType(Guid bodyTypeId)
        {
            _configurator.SelectBodyType(bodyTypeId);
        }

        public void SelectBrand(Guid brandId)
        {
            _configurator.SelectBrand(brandId);
        }

        public void SelectColor(Guid colorId)
        {
            _configurator.SelectColor(colorId);
        }

        public void SelectEngine(Guid engineId)
        {
            _configurator.SelectEngine(engineId);
        }

        public void SelectEquipment(Guid equipmentId)
        {
            _configurator.SelectEquipment(equipmentId);
        }

        public void SelectModel(Guid modelId)
        {
            _configurator.SelectModel(modelId);
        }
    }
}
