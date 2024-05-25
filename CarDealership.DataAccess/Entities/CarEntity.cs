namespace CarDealership.DataAccess.Entities
{
    public class CarEntity : BaseEntity
    {
        public string VIN { get; set; } = string.Empty;
        public Guid AutoConfigurationId { get; set; }
        public AutoConfigurationEntity? AutoConfiguration { get; set; }
    }
}
