
namespace CarDealership.Web.Api.Contracts.Responses
{
    public class EquipmentResponse : BaseResponse
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ReleaseYear { get; set; }

        public List<FeatureResponse> Features { get; set; }
        public EquipmentResponse(Guid id) : base(id)
        {
        }
    }
}
