
namespace CarDealership.Web.Api.Contracts.Responses
{
    public class DriveTypeResponse : BaseResponse
    {
        public string Value { get; set; }
        public decimal Price { get; set; }

        public DriveTypeResponse(Guid id) : base(id)
        {
        }
    }
}
