namespace CarDealership.Web.Api.Contracts.Requests
{
    public class EquipFeatureRemoveRequest
    {
        public Guid FeatureId { get; set; }
        public Guid EquipmentId { get; set; }
    }
}
