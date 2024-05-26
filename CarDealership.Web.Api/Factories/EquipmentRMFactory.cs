using CarDealership.Application.Services;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Factories;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;

namespace CarDealership.Web.Api.Factories
{
    public class EquipmentRMFactory : IEquipmentRMFactory
    {
        private readonly IAutoModelsService _autoModelsService;
        private readonly IFeaturesService _featuresService;
        private readonly IFeatureRMFactory _featureRMFactory;
        public EquipmentRMFactory(
            IAutoModelsService autoModelsService,
            IFeaturesService featuresService, 
            IFeatureRMFactory featureRMFactory
        )
        {
            _autoModelsService = autoModelsService ?? throw new ArgumentNullException(nameof(autoModelsService));
            _featuresService = featuresService ?? throw new ArgumentNullException(nameof(featuresService));
            _featureRMFactory = featureRMFactory ?? throw new ArgumentNullException(nameof(featureRMFactory));
        }
        public async Task<Equipment> CreateModelAsync(EquipmentRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var equipmentModel = await _autoModelsService.GetByIdAsync(req.AutoModelId);

            if (equipmentModel == null) throw new InvalidOperationException($"Модель с id = {req.AutoModelId} не найден");
            
            var equipmentCreateResult = Equipment.Create(
                req.Id,
                req.Name,
                req.Price,
                req.ReleaseYear,
                req.AutoModelId,
                req.IsDeleted,
                equipmentModel
            );

            if (equipmentCreateResult.IsFailure)
            {
                throw new InvalidOperationException(equipmentCreateResult.Error);
            }

            var equipment = equipmentCreateResult.Value;

            foreach (var featureId in req.FeatureIds)
            {
                var tempFeature = await _featuresService.GetByIdAsync(featureId);

                if (tempFeature == null) throw new InvalidOperationException($"Feature с id={featureId} не найден");

                var equipmentFeatureCreateResult = EquipmentFeature.Create(Guid.NewGuid(), req.Id, featureId, equipment, tempFeature);
                if(equipmentFeatureCreateResult.IsFailure)
                {
                    throw new InvalidOperationException(equipmentFeatureCreateResult.Error);
                }
                var equipmentFeature = equipmentFeatureCreateResult.Value;
                tempFeature.AddFeatureEquipment(equipmentFeature);
                equipment.AddEquipmentFeature(equipmentFeature);
            }
            equipmentModel.AddEquipment(equipment);
            return equipment;
        }

        public EquipmentResponse CreateResponse(Equipment model)
        {
            var features = model.EquipmentFeatures.Select(ef => ef.Feature).ToList();
            List<FeatureResponse> featureResponses = features.Select(feature => _featureRMFactory.CreateResponse(feature)).ToList();


            var response = new EquipmentResponse(model.Id)
            {
                Name = model.Name,
                Price = model.Price,
                ReleaseYear = model.ReleaseYear,
                Features = featureResponses
            };

            return response;
        }
    }
}
