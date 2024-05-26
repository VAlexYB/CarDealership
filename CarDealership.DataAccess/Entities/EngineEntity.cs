namespace CarDealership.DataAccess.Entities
{
    public class EngineEntity : BaseEntity
    {
        public int Power { get; set; } //лошадиные силы
        public int Consumption { get; set; } //эффективная мощность
        public decimal Price { get; set; }

        public Guid EngineTypeId { get; set; }
        public virtual EngineTypeEntity? EngineType { get; set; }

        public Guid TransmissionTypeId { get; set; }
        public virtual TransmissionTypeEntity? TransmissionType { get; set; }

        public virtual List<AutoConfigurationEntity> Configurations { get; set; } = [];
    }
}
