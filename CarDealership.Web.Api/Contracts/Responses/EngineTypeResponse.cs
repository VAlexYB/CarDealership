
namespace CarDealership.Web.Api.Contracts.Responses
{
    public class EngineTypeResponse : BaseResponse
    {
        public string Value { get; set; }

        public EngineTypeResponse(Guid id) : base(id)
        {
        }
    }
}
