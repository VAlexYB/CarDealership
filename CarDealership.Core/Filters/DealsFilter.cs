using CarDealership.Core.Enums;
using CarDealership.Core.Models;

namespace CarDealership.Core.Filters
{
    public class DealsFilter : BaseFilter
    {
        public Guid? CustomerId { get; set; }
        public Guid? ManagerId { get; set; }
        public DealStatus? DealStatus { get; set; }
    }
}
