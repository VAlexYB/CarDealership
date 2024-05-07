using CarDealership.Core.Enums;
using CarDealership.Core.Models;

namespace CarDealership.DataAccess.Entities
{
    public class AutoModelEntity : BaseEntity
    {
        public AutoModelEntity(AutoModel model) : base(model.Id)
        {
            Brand = model.Brand;
            Name = model.Name;
            BodyType = model.BodyType;
            Price = model.Price;
        }
        public string Brand { get; set; }
        public string Name { get; set; }
        public BodyType BodyType { get; set; }
        public decimal Price { get; set; }
    }
}
