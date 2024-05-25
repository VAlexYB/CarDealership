namespace CarDealership.Web.Api.Contracts.Requests
{
    public class BrandRequest : BaseRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid CountryId { get; set; }
    }
}
