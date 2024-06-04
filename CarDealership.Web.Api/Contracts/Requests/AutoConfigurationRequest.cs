namespace CarDealership.Web.Api.Contracts.Requests
{
    public class AutoConfigurationRequest : BaseRequest
    {
        public decimal Price { get; set; }
        public Guid AutoModelId { get; set; }
        public Guid BodyTypeId { get; set; }
        public Guid DriveTypeId { get; set; }
        public Guid EngineId { get; set; }
        public Guid ColorId { get; set; }
        public Guid EquipmentId { get; set; }
    }
}
