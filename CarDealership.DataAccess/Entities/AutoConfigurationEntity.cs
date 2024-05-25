namespace CarDealership.DataAccess.Entities
{
    public class AutoConfigurationEntity : BaseEntity
    {
        public decimal Price { get; set; }

        public Guid AutoModelId { get; set; }
        public AutoModelEntity? AutoModel { get; set; }

        public Guid BodyTypeId { get; set; }
        public BodyTypeEntity? BodyType { get; set; }

        public Guid DriveTypeId { get; set; }
        public DriveTypeEntity? DriveType { get; set; }

        public Guid EngineId { get; set; }
        public EngineEntity? Engine { get; set; }

        public Guid ColorId { get; set; }
        public ColorEntity? Color { get; set;}

        public List<CarEntity> Cars { get; set; } = [];
    }
}
