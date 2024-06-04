using CarDealership.Core.Models;

namespace CarDealership.Core.Filters
{
    public class ConfigurationsFilter : BaseFilter
    {
        public Guid? BrandId { get; set; }
        public Guid? AutoModelId { get; set; }
        public Guid? EquipmentId { get; set; }
        public Guid? BodyTypeId { get; set; }
        public Guid? EngineId { get; set; }
        public Guid? ColorId { get; set; }
        public Guid? DriveTypeId { get; set; }
    }
}
