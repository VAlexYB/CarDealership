namespace CarDealership.Web.Api.Contracts.Requests
{
    public class AutoModelRequest : BaseRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public Guid BrandId { get; set; }
    }
}
