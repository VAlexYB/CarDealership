using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.DataAccess
{
    public class CarConfigurator
    {
        private readonly DbSet<AutoConfigurationEntity> _configDbSet;


        private Guid? _selectedBrandId;
        private Guid? _selectedAutoModelId;
        private Guid? _selectedEquipmentId;
        private Guid? _selectedBodyTypeId;
        private Guid? _selectedEngineId;
        private Guid? _selectedColorId;

        public CarConfigurator(CarDealershipDbContext context)
        {
            _configDbSet = context.Set<AutoConfigurationEntity>();
        }

        public void SelectBrand(Guid brandId)
        {
            _selectedBrandId = brandId;
            _selectedAutoModelId = null;
            _selectedEquipmentId = null;
            _selectedBodyTypeId = null;
            _selectedEngineId = null;
            _selectedColorId = null;
    }

        public void SelectModel(Guid modelId)
        {
            _selectedAutoModelId = modelId;
            _selectedEquipmentId = null;
            _selectedBodyTypeId = null;
            _selectedEngineId = null;
            _selectedColorId = null;
        }

        public void SelectEquipment(Guid equipmentId)
        {
            _selectedEquipmentId = equipmentId;
            _selectedBodyTypeId = null;
            _selectedEngineId = null;
            _selectedColorId = null;
        }

        public void SelectBodyType(Guid bodyTypeId)
        {
            _selectedBodyTypeId = bodyTypeId;
            _selectedEngineId = null;
            _selectedColorId = null;
        }

        public void SelectEngine(Guid engineId)
        {
            _selectedEngineId = engineId;
            _selectedColorId = null;
        }
        
        public void SelectColor(Guid colorId)
        {
            _selectedColorId = colorId;
        }

        public async Task<List<AutoConfigurationEntity>> GetFilteredConfigurations()
        {
            IQueryable<AutoConfigurationEntity> query = _configDbSet.AsQueryable();

            if (_selectedBrandId.HasValue)
            {
                query = query.Where(config => config.AutoModel.BrandId == _selectedBrandId);
            }

           
            if (_selectedAutoModelId.HasValue)
            {
                query = query.Where(config => config.AutoModelId == _selectedAutoModelId);
            }

            if (_selectedEquipmentId.HasValue)
            {
                query = query.Where(config => config.EquipmentId == _selectedEquipmentId);
            }

            if (_selectedBodyTypeId.HasValue)
            {
                query = query.Where(config => config.BodyTypeId == _selectedBodyTypeId);
            }

            if (_selectedEngineId.HasValue)
            {
                query = query.Where(config => config.EngineId == _selectedEngineId);
            }

            if (_selectedColorId.HasValue)
            {
                query = query.Where(config => config.ColorId == _selectedColorId);
            }

            return await query.ToListAsync();
        }
    }
}
