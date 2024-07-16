namespace CarDealership.Web.Api.Contracts.Requests
{
    public class EquipFeatureChangeRequest
    {
        public Guid FeatureId { get; set; }
        public Guid EquipmentId { get; set; }
    }
}
