using CarDealership.Core.Enums;
using CarDealership.DataAccess.Entities.Auth;
using CarDealership.DataAccess.Entities;

namespace CarDealership.Web.Api.Contracts.Requests
{
    public class DealRequest : BaseRequest
    {
        public DateTime DealDate { get; set; }
        public DealStatus Status { get; set; }
        public Guid CarId { get; set; }

        public Guid? ManagerId { get; set; }

        public Guid CustomerId { get; set; }
    }
}
