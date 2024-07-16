namespace CarDealership.DataAccess.Entities
{
    public class AutoConfigurationEntity : BaseEntity
    {
        public decimal Price { get; set; }

        public Guid AutoModelId { get; set; }
        public virtual AutoModelEntity? AutoModel { get; set; }

        public Guid BodyTypeId { get; set; }
        public virtual BodyTypeEntity? BodyType { get; set; }

        public Guid DriveTypeId { get; set; }
        public virtual DriveTypeEntity? DriveType { get; set; }

        public Guid EngineId { get; set; }
        public virtual EngineEntity? Engine { get; set; }

        public Guid ColorId { get; set; }
        public virtual ColorEntity? Color { get; set; }

        public Guid EquipmentId { get; set; }
        public virtual EquipmentEntity? Equipment { get; set; }

        public virtual List<CarEntity> Cars { get; set; } = [];
        public virtual List<OrderEntity> Orders { get; set; } = [];
    }
}
