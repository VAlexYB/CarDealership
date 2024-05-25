namespace CarDealership.DataAccess.Entities
{
    public class EngineEntity : BaseEntity
    {
        public int Power { get; set; } //лошадиные силы
        public int Consumption { get; set; } //эффективная мощность
        public decimal Price { get; set; }

        public Guid EngineTypeId { get; set; }
        public EngineTypeEntity? EngineType { get; set; }

        public Guid TransmissionTypeId { get; set; }
        public TransmissionTypeEntity? TransmissionType { get; set; }

        public List<AutoConfigurationEntity> Configurations { get; set; } = [];
    }
}
