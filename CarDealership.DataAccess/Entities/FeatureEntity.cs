namespace CarDealership.DataAccess.Entities
{
    public class FeatureEntity : BaseEntity
    {
        public string Description { get; set; } = string.Empty;

        public virtual List<EquipmentFeatureEntity> featureEquipments { get; set; } = [];
    }
}
