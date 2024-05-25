
namespace CarDealership.Web.Api.Contracts.Responses
{
    public class ColorResponse : BaseResponse
    {
        public string Value { get; set; }
        public decimal Price { get; set; }
        public ColorResponse(Guid id) : base(id)
        {
        }
    }
}
