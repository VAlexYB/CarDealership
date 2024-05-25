
namespace CarDealership.Web.Api.Contracts.Responses
{
    public class BodyTypeResponse : BaseResponse
    {
        public string Value { get; set; }
        public decimal Price { get; set; }
        public BodyTypeResponse(Guid id) : base(id)
        {
        }
    }
}
