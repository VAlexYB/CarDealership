using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
namespace CarDealership.Core.Models
{
    public class Country : BaseModel
    {
        public const int MAX_NAME_LENGTH = 35;
        public string Name { get; }

        private readonly List<Brand> brands = new List<Brand>();
        public IReadOnlyCollection<Brand> Brands => brands.AsReadOnly();
        
        [JsonConstructor]
        private Country(Guid id, string name, bool isDeleted) : base(id)
        {
            Name = name;
            IsDeleted = isDeleted;
        }

        public void AddBrand(Brand brand)
        {
            if(brand == null) throw new ArgumentNullException(nameof(brand));
            brands.Add(brand);
        }

        public static Result<Country> Create(Guid id, string name, bool isDeleted = false)
        {
            var error = string.Empty;

            if(string.IsNullOrWhiteSpace(name) || name.Length > MAX_NAME_LENGTH)
            {
                error += $"Название страны не может быть пустым или иметь более {MAX_NAME_LENGTH} символов. ";
            }

            if(!string.IsNullOrEmpty(error))
            {
                return Result.Failure<Country>(error.Trim());
            }

            var country = new Country(id, name, isDeleted);
            return Result.Success(country);
        }
    }
}
