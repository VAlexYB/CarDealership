
using CSharpFunctionalExtensions;
using System.Text;

namespace CarDealership.Core.Models
{
    public class Equipment : BaseModel
    {
        public const int MAX_NAME_LENGTH = 100;
        public string Name { get; } 
        public decimal Price { get; }
        public string ReleaseYear { get; }
        public Guid AutoModelId { get; }
        public AutoModel? AutoModel { get; }
        private readonly List<EquipmentFeature> equipmentFeatures = new List<EquipmentFeature>();
        public IReadOnlyCollection<EquipmentFeature> EquipmentFeatures => equipmentFeatures.AsReadOnly();
        public Equipment(Guid id, string name, decimal price, string releaseYear, Guid autoModelId, bool isDeleted, AutoModel? autoModel) : base(id)
        {
            Name = name;
            Price = price;
            ReleaseYear = releaseYear;
            AutoModelId = autoModelId;
            IsDeleted = isDeleted;
            AutoModel = autoModel;
        }

        public void AddEquipmentFeature(EquipmentFeature feature)
        {
            if(feature == null) throw new ArgumentNullException(nameof(feature));
            equipmentFeatures.Add(feature);
        }

        public static Result<Equipment> Create(Guid id, string name, decimal price, string releaseYear,
            Guid autoModelId, bool isDeleted, AutoModel? autoModel = null)
        {
            var errorBuilder = new StringBuilder();

            if(string.IsNullOrWhiteSpace(name) || name.Length > MAX_NAME_LENGTH)
            {
                errorBuilder.Append($"Название характеристики не может быть пустым и иметь более {MAX_NAME_LENGTH} символов. ");
            }

            if(price < 0)
            {
                errorBuilder.Append("Цена комплектации не может быть отрицательной. ");
            }

            //if(int.Parse(releaseYear) < 1900)
            //{
            //    errorBuilder.Append("Год комплектации не может быть менее 1900. ");
            //}

            if (autoModelId == Guid.Empty)
            {
                errorBuilder.Append("AutoModelId не должен быть пустым. ");
            }

            if (errorBuilder.Length > 0)
            {
                return Result.Failure<Equipment>(errorBuilder.ToString().Trim());
            }

            var equipment = new Equipment(id, name, price, releaseYear, autoModelId, isDeleted, autoModel);
            return Result.Success(equipment);
        }
    }
}
