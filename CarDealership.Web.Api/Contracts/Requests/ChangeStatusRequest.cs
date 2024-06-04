namespace CarDealership.Web.Api.Contracts.Requests
{
    public class ChangeStatusRequest
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
    }
}
