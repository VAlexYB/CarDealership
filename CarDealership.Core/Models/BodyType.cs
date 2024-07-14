
using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace CarDealership.Core.Models
{
    public class BodyType : BaseModel
    {
        public const int MAX_NAME_LENGTH = 15;

        public string Value { get; }
        public decimal Price { get; }

        private readonly List<AutoConfiguration> configurations = new List<AutoConfiguration>();
        public IReadOnlyCollection<AutoConfiguration> Configurations => configurations.AsReadOnly();
        
        [JsonConstructor]
        private BodyType(Guid id, string value, decimal price, bool isDeleted) : base(id)
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

        public static Result<BodyType> Create(Guid id, string value, decimal price, bool isDeleted = false)
        {
            var error = string.Empty;

            if(price < 0)
            {
                error += "Цена кузова не может быть отрицательной. ";
            }

            if(string.IsNullOrWhiteSpace(value) || value.Length > MAX_NAME_LENGTH)
            {
                error += $"Название кузова не может быть пустым или иметь больше {MAX_NAME_LENGTH} символов. ";
            }

            if (!string.IsNullOrEmpty(error))
            {
                return Result.Failure<BodyType>(error.Trim());
            }

            var bodyType = new BodyType(id, value, price, isDeleted);
            return Result.Success(bodyType);
        }
    }
}
