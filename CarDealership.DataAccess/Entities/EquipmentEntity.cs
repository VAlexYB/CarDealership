namespace CarDealership.DataAccess.Entities
{
    public class EquipmentEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        
        public decimal Price { get; set; }

        public string ReleaseYear { get; set; } = string.Empty;

        public Guid AutoModelId { get; set; }
        public virtual AutoModelEntity? AutoModel { get; set; }

        public virtual List<EquipmentFeatureEntity> equipmentFeatures { get; set; } = [];

        public virtual List<AutoConfigurationEntity> Configurations { get; set; } = [];
    }
}
