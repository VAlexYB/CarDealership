
using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace CarDealership.Core.Models
{
    public class Feature : BaseModel
    {
        public const int MAX_DESCRIPTION_LENGTH = 250;
        public string Description { get; }

        private readonly List<EquipmentFeature> featureEquipments = new List<EquipmentFeature>();
        public IReadOnlyCollection<EquipmentFeature> FeatureEquipments => featureEquipments.AsReadOnly();
        
        [JsonConstructor]
        private Feature(Guid id, string description, bool isDeleted) : base(id)
        {
            Description = description;
            IsDeleted = isDeleted;
        }

        public void AddFeatureEquipment(EquipmentFeature equipment)
        {
            featureEquipments.Add(equipment);
        }

        public static Result<Feature> Create(Guid id, string description, bool isDeleted = false)
        {
            var error = string.Empty;

            if (string.IsNullOrWhiteSpace(description) || description.Length > MAX_DESCRIPTION_LENGTH)
            {
                error += $"Описание характеристики не может быть пустым или содержать более {MAX_DESCRIPTION_LENGTH} символов. ";
            }

            if (!string.IsNullOrEmpty(error))
            {
                return Result.Failure<Feature>(error.Trim());
            }

            var feature = new Feature(id, description, isDeleted);
            return Result.Success(feature);
        }
        
    }
}
