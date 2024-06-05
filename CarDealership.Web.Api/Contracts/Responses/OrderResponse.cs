using CarDealership.Core.Enums;
using CarDealership.DataAccess.Entities.Auth;
using CarDealership.DataAccess.Entities;

namespace CarDealership.Web.Api.Contracts.Responses
{
    public class OrderResponse : BaseResponse
    {
        public DateTime OrderDate { get; set; }
        public DateTime CompleteDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Price { get; set; }
        public Guid AutoConfigurationId { get; set; }
        public AutoConfigurationResponse AutoConfiguration { get; set; }

        public Guid? ManagerId { get; set; }

        public Guid CustomerId { get; set; }
        public OrderResponse(Guid id) : base(id)
        {
        }
    }
}
