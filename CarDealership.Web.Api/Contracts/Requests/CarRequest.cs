namespace CarDealership.Web.Api.Contracts.Requests
{
    public class CarRequest : BaseRequest
    {
        public string VIN { get; set; } = string.Empty;
        public Guid AutoConfigurationId { get; set; }
    }
}
