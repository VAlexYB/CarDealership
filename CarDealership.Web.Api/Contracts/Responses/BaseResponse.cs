namespace CarDealership.Web.Api.Contracts.Responses
{
    public abstract class BaseResponse
    {
        public Guid Id;
        public BaseResponse(Guid id)
        {
            Id = id;
        }
    }
}
