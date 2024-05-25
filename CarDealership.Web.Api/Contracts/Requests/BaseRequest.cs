namespace CarDealership.Web.Api.Contracts.Requests
{
    public abstract class BaseRequest
    {
        public Guid Id;

        public bool IsDeleted;
    }
}
