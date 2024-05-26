namespace CarDealership.DataAccess.Entities
{
    public class AutoModelEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public virtual List<AutoConfigurationEntity> Configurations { get; set; } = [];
        public virtual List<EquipmentEntity> Equipments { get; set; } = [];
        public Guid BrandId { get; set; }
        public virtual BrandEntity? Brand { get; set; }
    }
}
