using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
namespace CarDealership.Core.Models
{
    public class Color : BaseModel
    {
        public const int MAX_VALUE_LENGTH = 30;
        public string Value { get; }
        public decimal Price { get; }

        private readonly List<AutoConfiguration> configurations = new List<AutoConfiguration>();
        public IReadOnlyCollection<AutoConfiguration> Configurations => configurations.AsReadOnly();
        
        [JsonConstructor]
        private Color(Guid id, string value, decimal price, bool isDeleted) : base(id)
        {
            Value = value;
            Price = price;
            IsDeleted = isDeleted;
        }

        public void AddConfiguration(AutoConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            configurations.Add(configuration);
        }

        public static Result<Color> Create(Guid id, string value, decimal price, bool isDeleted = false)
        {
            var error = string.Empty;

            if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_VALUE_LENGTH)
            {
                error += $"Название цвета не может быть пустым или иметь больше {MAX_VALUE_LENGTH}. ";
            }

            if (price < 0)
            {
                error += "Цена покраски не может быть отрицательной. ";
            }

            if (!string.IsNullOrEmpty(error))
            {
                return Result.Failure<Color>(error.Trim());
            }

            var color = new Color(id, value, price, isDeleted);
            return Result.Success(color);
        }
    }
}
