
using CSharpFunctionalExtensions;

namespace CarDealership.Core.Models
{
    public class Brand : BaseModel
    {
        public const int MAX_NAME_LENGTH = 25;

        public string Name { get; }
        public Guid CountryId { get; }
        public Country? Country { get; }

        private readonly List<AutoModel> models = new List<AutoModel>();
        public IReadOnlyCollection<AutoModel> Models => models.AsReadOnly();
        private Brand(Guid id, string name, Guid countryId, bool isDeleted, Country? country) : base(id)
        {
            Name = name;
            CountryId = countryId;
            IsDeleted = isDeleted;
            Country = country;
        }

        public void AddModel(AutoModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            models.Add(model);
        }

        public static Result<Brand> Create(Guid id, string name, Guid countryId, bool isDeleted = false, Country? country = null)
        {
            var error = string.Empty;

            if(string.IsNullOrWhiteSpace(name) || name.Length > MAX_NAME_LENGTH)
            {
                error += $"Название марки не может быть пустым или иметь больше {MAX_NAME_LENGTH} символов. ";
            }

            if(countryId == Guid.Empty)
            {
                error += "CountryId не может быть пустым. ";
            }

            if (!string.IsNullOrEmpty(error))
            {
                return Result.Failure<Brand>(error.Trim());
            }

            var brand = new Brand(id, name, countryId, isDeleted, country);
            return Result.Success(brand);
        }
    }
}
