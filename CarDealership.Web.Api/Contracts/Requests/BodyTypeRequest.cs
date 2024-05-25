namespace CarDealership.Web.Api.Contracts.Requests
{
    public class BodyTypeRequest : BaseRequest
    {
        public string? Value { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
