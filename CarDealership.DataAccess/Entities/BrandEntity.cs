namespace CarDealership.DataAccess.Entities
{
    public class BrandEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public virtual List<AutoModelEntity> Models { get; set; } = [];

        public Guid CountryId { get; set; }
        public virtual CountryEntity? Country { get; set; }
    }
}
