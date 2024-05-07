using CarDealership.Core.Enums;

namespace CarDealership.Core.Models
{
    public class AutoModel : BaseModel
    {
        public const int MAX_BRAND_LENGTH = 50;

        private readonly List<Car> cars = new List<Car>();
        private AutoModel(Guid id, string brand, string name, BodyType bodyType, decimal price) : base(id)
        {
            Brand = brand;
            Name = name;
            BodyType = bodyType;
            Price = price;
        }
        
        public string Brand { get; }
        public string Name { get; }
        public BodyType BodyType { get; }
        public decimal Price { get; }

        public static (AutoModel model, string Error) Create(Guid id, string brand, string name, BodyType bodyType, decimal price)
        {
            var error = string.Empty;
            var model = new AutoModel(id, brand, name, bodyType, price);

            if(string.IsNullOrEmpty(brand) || brand.Length > MAX_BRAND_LENGTH)
            {
                error += $"Марка не может быть пустой или содержать больше {MAX_BRAND_LENGTH} символов";
            }

            if (string.IsNullOrEmpty(name) || name.Length > MAX_BRAND_LENGTH)
            {
                error += $"Название модели не может быть пустым или содержать больше {MAX_BRAND_LENGTH} символов";
            }

            return (model, error);

        }
    }
}
