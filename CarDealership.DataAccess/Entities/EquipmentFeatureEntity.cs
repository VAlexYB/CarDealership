namespace CarDealership.DataAccess.Entities
{
    public class EquipmentFeatureEntity : BaseEntity
    {
        public Guid EquipmentId { get; set; }
        public virtual EquipmentEntity? Equipment { get; set; }

        public Guid FeatureId { get; set; }
        public virtual FeatureEntity? Feature { get; set; }
    }
}
