using CarDealership.Application.Services;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Exceptions;
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
        public async Task<Equipment> CreateModel(EquipmentRequest req)
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
                false,
                equipmentModel
            );

            if (equipmentCreateResult.IsFailure)
            {
                throw new ClientInformationException(equipmentCreateResult.Error);
            }

            var equipment = equipmentCreateResult.Value;
            equipmentModel.AddEquipment(equipment);
            return equipment;
        }

        public async Task<EquipmentResponse> CreateResponse(Equipment model)
        {
            var equipment = await _featuresService.
            var features = model.EquipmentFeatures.Select(ef => ef.Feature).ToList();

            IEnumerable<Task<FeatureResponse>> tasks = features.Select(f => _featureRMFactory.CreateResponse(f));
            List<FeatureResponse> featureResponses = (await Task.WhenAll(tasks)).ToList();

            var response = new EquipmentResponse(model.Id)
            {
                Name = model.Name,
                Price = model.Price,
                ReleaseYear = model.ReleaseYear,
                Features = featureResponses,
                AutoModelId = model.AutoModelId,
                AutoModelName = model.AutoModel?.Name ?? "",
                BrandName = model.AutoModel?.Brand?.Name ?? ""
            };

            return response;
        }
    }
}
