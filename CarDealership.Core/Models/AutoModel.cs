using CSharpFunctionalExtensions;
using System.Text;

namespace CarDealership.Core.Models
{
    public class AutoModel : BaseModel
    {
        public const int MAX_NAME_LENGTH = 50;

        private readonly List<AutoConfiguration> configurations = new List<AutoConfiguration>();
        private readonly List<Equipment> equipments = new List<Equipment>();
        public string Name { get; }

        public decimal Price { get; }
        public Guid BrandId { get; }
        public Brand? Brand { get; }
        public IReadOnlyCollection<AutoConfiguration> Configurations => configurations.AsReadOnly();
        public IReadOnlyCollection<Equipment> Equipments => equipments.AsReadOnly();

        private AutoModel(Guid id,  string name, decimal price, Guid brandId, bool isDeleted, Brand? brand) : base(id)
        {
            Name = name;
            Price = price;
            BrandId = brandId;
            IsDeleted = isDeleted;
            Brand = brand;
        }

        public void AddConfiguration(AutoConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            configurations.Add(configuration);
        }

        public void AddEquipment(Equipment equip)
        {
            if (equip == null) throw new ArgumentNullException(nameof(equip));
            equipments.Add(equip);
        }

        public static Result<AutoModel> Create(Guid id, string name, decimal price, Guid brandId, bool isDeleted, Brand? brand = null)
        {
            var errorBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(name) || name.Length > MAX_NAME_LENGTH)
            {
                errorBuilder.Append($"Название модели не может быть пустым или содержать больше {MAX_NAME_LENGTH} символов. ");
            }

            if (price < 0)
            {
                errorBuilder.Append($"Цена модели не может быть отрицательной. ");
            }

            if (brandId == Guid.Empty)
            {
                errorBuilder.Append("BrandId не должен быть пустым. ");
            }

            if (errorBuilder.Length > 0)
            {
                return Result.Failure<AutoModel>(errorBuilder.ToString().Trim());
            }
           
            var autoModel = new AutoModel(id, name, price, brandId, isDeleted, brand);
            return Result.Success(autoModel);
        }
    }
}
