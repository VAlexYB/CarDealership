using CarDealership.Core.Models;

namespace CarDealership.Core.Filters
{
    public class EquipmentsFilter : BaseFilter
    {
        public Guid? AutoModelId { get; set; }
    }
}
