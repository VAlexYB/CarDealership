namespace CarDealership.DataAccess.Entities
{
    public class CarEntity : BaseEntity
    {
        public string VIN { get; set; } = string.Empty;
        public Guid AutoConfigurationId { get; set; }
        public virtual AutoConfigurationEntity? AutoConfiguration { get; set; }
    }
}
