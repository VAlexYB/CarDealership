
namespace CarDealership.Web.Api.Contracts.Responses
{
    public class TransmissionTypeResponse : BaseResponse
    {
        public TransmissionTypeResponse(Guid id) : base(id)
        {
        }

        public string Value { get; set; } = string.Empty;
    }
}
