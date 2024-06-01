using CSharpFunctionalExtensions;

namespace CarDealership.Core.Models
{
    public class DriveType : BaseModel
    {
        public const int MAX_VALUE_LENGTH = 10;
        public string Value { get; }
        public decimal Price { get; }

        private readonly List<AutoConfiguration> configurations = new List<AutoConfiguration>();
        public IReadOnlyCollection<AutoConfiguration> Configurations => configurations.AsReadOnly();
        private DriveType(Guid id, string value, decimal price, bool isDeleted) : base(id)
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

        public static Result<DriveType> Create(Guid id, string value, decimal price, bool isDeleted = false)
        {
            var error = string.Empty;

            if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_VALUE_LENGTH)
            {
                error += $"Тип привода не должен быть пустым или иметь более {MAX_VALUE_LENGTH} символов. ";
            }

            if (price < 0)
            {
                error += "Цена привода не может быть отрицательной. ";
            }

            if (!string.IsNullOrEmpty(error))
            {
                return Result.Failure<DriveType>(error.Trim());
            }

            var driveType = new DriveType(id, value, price, isDeleted);
            return Result.Success(driveType);
        }
    }
}
