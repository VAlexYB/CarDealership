using CarDealership.Core.Enums;
using CarDealership.Core.Models;

namespace CarDealership.Core.Filters
{
    public class OrdersFilter : BaseFilter
    {
        public Guid? CustomerId { get; set; }
        public Guid? ManagerId { get; set; }
        public OrderStatus? OrderStatus { get; set; }
    }
}
