
namespace CarDealership.Web.Api.Contracts.Responses
{
    public class CarResponse : BaseResponse
    {
        public string VIN { get; set; }
        public AutoConfigurationResponse Configuration { get; set; }

        public CarResponse(Guid id) : base(id)
        {
        }
    }
}
