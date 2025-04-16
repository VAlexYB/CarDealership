using CSharpFunctionalExtensions;
using Newtonsoft.Json;

namespace CarDealership.Core.Models
{
    public class EquipmentFeature : BaseModel
    {
        public Guid EquipmentId { get; }
        public Equipment? Equipment { get; }

        public Guid FeatureId { get; }
        public Feature? Feature { get; }

        [JsonConstructor]
        private EquipmentFeature(Guid id, Guid equipmentId, Guid featureId, Equipment? equipment, Feature? feature) : base(id)
        {
            EquipmentId = equipmentId;
            Equipment = equipment;
            FeatureId = featureId;
            Feature = feature;
        }

        public static Result<EquipmentFeature> Create(Guid id, Guid equipmentId, Guid featureId, 
            Equipment? equipment = null, Feature? feature = null)
        {
            var error = string.Empty;

            if(equipmentId == Guid.Empty)
            {
                error += "При формировании связки фичи с комплектацией id комплектации пустой. ";
            }

            if(featureId == Guid.Empty)
            {
                error += "При формировании связки фичи с комплектацией id фичи пустой. ";
            }

            if (!string.IsNullOrEmpty(error))
            {
                return Result.Failure<EquipmentFeature>(error.Trim());
            }

            var equipmentFeature = new EquipmentFeature(id, equipmentId, featureId, equipment, feature);
            return Result.Success(equipmentFeature);
        }
    }
}
