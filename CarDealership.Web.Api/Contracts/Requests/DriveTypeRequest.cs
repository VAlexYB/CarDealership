namespace CarDealership.Web.Api.Contracts.Requests
{
    public class DriveTypeRequest : BaseRequest
    {
        public string Value { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
