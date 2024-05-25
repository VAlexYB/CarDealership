
namespace CarDealership.Web.Api.Contracts.Responses
{
    public class CountryResponse : BaseResponse
    {
        public string Name { get; set; }
        public CountryResponse(Guid id) : base(id)
        {
        }
    }
}
