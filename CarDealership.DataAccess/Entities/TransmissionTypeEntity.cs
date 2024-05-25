namespace CarDealership.DataAccess.Entities
{
    public class TransmissionTypeEntity : BaseEntity
    {
        public string Value { get; set; } = string.Empty;

        public List<EngineEntity> Engines { get; set; } = [];
    }
}
