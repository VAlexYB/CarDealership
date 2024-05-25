
namespace CarDealership.Web.Api.Contracts.Responses
{
    public class FeatureResponse : BaseResponse
    {
        public string Description { get; set; }

        public FeatureResponse(Guid id) : base(id)
        {
        }
    }
}
