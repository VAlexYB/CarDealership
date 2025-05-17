namespace CarDealership.Web.Api.Contracts.Requests
{
    public class UsePromocodeRequest
    {
        public Guid CustomerId { get; set; }
        public Guid AutoConfigurationId {  get; set; }
        public string Promocode { get; set; }
    }
}
