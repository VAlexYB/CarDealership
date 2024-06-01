namespace CarDealership.DataAccess.Entities
{
    public class DriveTypeEntity : BaseEntity
    {
        public string Value { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public virtual List<AutoConfigurationEntity> Configurations { get; set; } = [];
    }
}
