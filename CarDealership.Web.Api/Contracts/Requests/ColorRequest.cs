namespace CarDealership.Web.Api.Contracts.Requests
{
    public class ColorRequest : BaseRequest
    {
        public string Value { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
