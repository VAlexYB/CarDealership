namespace CarDealership.Web.Api.Contracts.Requests
{
    public class EquipmentRequest : BaseRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ReleaseYear { get; set; } = string.Empty;
        public Guid AutoModelId { get; set; }
        public List<Guid> FeatureIds { get; set; } = [];
    }
}
