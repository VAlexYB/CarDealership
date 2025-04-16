using CarDealership.DataAccess.Attributes;

namespace CarDealership.DataAccess.Entities
{
    public class CountryEntity : BaseEntity
    {
        [Unique]
        public string Name { get; set; } = string.Empty;

        public virtual List<BrandEntity> Brands { get; set; } = [];
    }
}
