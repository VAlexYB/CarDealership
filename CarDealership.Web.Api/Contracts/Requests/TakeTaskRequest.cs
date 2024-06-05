namespace CarDealership.Web.Api.Contracts.Requests
{
    public class TakeTaskRequest 
    {
        public Guid ManagerId { get; set; }
        public Guid TaskId { get; set; }
    }
}
