using CarDealership.DataAccess.Attributes;

namespace CarDealership.DataAccess.Entities
{
    public class ColorEntity : BaseEntity
    {
        [Unique]
        public string Value { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public virtual List<AutoConfigurationEntity> Configurations { get; set; } = [];
    }
}
