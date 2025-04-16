using CarDealership.DataAccess.Attributes;

namespace CarDealership.DataAccess.Entities
{
    public class EngineTypeEntity : BaseEntity
    {
        [Unique]
        public string Value { get; set; } = string.Empty;

        public virtual List<EngineEntity> Engines { get; set; } = [];
    }
}
