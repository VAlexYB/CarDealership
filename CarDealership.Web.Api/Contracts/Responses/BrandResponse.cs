
namespace CarDealership.Web.Api.Contracts.Responses
{
    public class BrandResponse : BaseResponse
    {
        public string Name { get; set; }
        public Guid CountryId { get; set; }
        public string Country { get; set; }
        public BrandResponse(Guid id) : base(id)
        {
        }
    }
}
