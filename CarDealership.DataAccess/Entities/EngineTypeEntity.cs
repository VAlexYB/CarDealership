namespace CarDealership.DataAccess.Entities
{
    public class EngineTypeEntity : BaseEntity
    {
        public string Value { get; set; } = string.Empty;

        public virtual List<EngineEntity> Engines { get; set; } = [];
    }
}
