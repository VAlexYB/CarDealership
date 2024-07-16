using CarDealership.Core.Enums;
using CarDealership.DataAccess.Entities.Auth;
using CarDealership.DataAccess.Entities;

namespace CarDealership.Web.Api.Contracts.Requests
{
    public class OrderRequest : BaseRequest
    {
        public DateTime OrderDate { get; set; }
        public DateTime CompleteDate { get; set; }
        public OrderStatus Status { get; set; }
        public Guid AutoConfigurationId { get; set; }

        public Guid ManagerId { get; set; }

        public Guid CustomerId { get; set; }
    }
}
