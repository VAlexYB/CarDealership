namespace CarDealership.DataAccess.Entities
{
    public class CountryEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public List<BrandEntity> Brands { get; set; } = [];
    }
}
