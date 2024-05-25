namespace CarDealership.Web.Api.Contracts.Requests
{
    public class EngineRequest : BaseRequest
    {
        public int Power { get; set; } 
        public int Consumption { get; set; } 
        public decimal Price { get; set; }
        public Guid EngineTypeId { get; set; }
        public Guid TransmissionTypeId { get; set; }
    }
}
