namespace CarDealership.DataAccess.Entities
{
    public class AutoModelEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public List<AutoConfigurationEntity> Configurations { get; set; } = [];
        public List<EquipmentEntity> Equipments { get; set; } = [];
        public Guid BrandId { get; set; }
        public BrandEntity? Brand { get; set; }
    }
}
