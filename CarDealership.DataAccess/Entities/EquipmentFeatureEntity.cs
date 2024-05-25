namespace CarDealership.DataAccess.Entities
{
    public class EquipmentFeatureEntity : BaseEntity
    {
        public Guid EquipmentId { get; set; }
        public EquipmentEntity? Equipment { get; set; }

        public Guid FeatureId { get; set; }
        public FeatureEntity? Feature { get; set; }
    }
}
