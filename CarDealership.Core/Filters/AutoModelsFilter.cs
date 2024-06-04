using CarDealership.Core.Models;

namespace CarDealership.Core.Filters
{
    public class AutoModelsFilter : BaseFilter
    {
        public Guid? BrandId { get; set; }
    }
}
