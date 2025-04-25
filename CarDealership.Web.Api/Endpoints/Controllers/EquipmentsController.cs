using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Filters;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Web.Api.Endpoints.Controllers
{
    [Route("api/[controller]")]
    public class EquipmentsController : BaseController<Equipment, EquipmentsFilter, EquipmentRequest, EquipmentResponse>
    {
        private readonly IEquipmentRMFactory _equipRMFactory;
        private readonly IFeatureRMFactory _featureRMFactory;
        private readonly IEquipmentsService equipService;
        private readonly ILogger _logger;
        public EquipmentsController
        (
            IEquipmentsService service,
            IEquipmentRMFactory factory,
            IFeatureRMFactory featureFactory,
            ILogger<EquipmentsController> logger
        ) : base(service, factory, logger)
        {
            _equipRMFactory = factory ?? throw new ArgumentNullException(nameof(factory));
            _featureRMFactory = featureFactory ?? throw new ArgumentNullException(nameof(featureFactory));
            equipService = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        public override async Task<IActionResult> CreateOrEditAsync(EquipmentRequest request)
        {
            Equipment model = await _equipRMFactory.CreateModelAsync(request);
            Guid equipmentId = await equipService.CreateOrEditAsync(model, request.FeatureIds);
            return Ok(equipmentId);
        }

        [Route("removeFeature")]
        [HttpPost]
        public async Task<IActionResult> RemoveFeature(EquipFeatureChangeRequest request)
        {
            await equipService.RemoveFeatureFromEquipment(request.EquipmentId, request.FeatureId);
            return Ok();
        }

        [Route("addFeature")]
        [HttpPost]
        public async Task<IActionResult> AddFeature(EquipFeatureChangeRequest request)
        {
            await equipService.AddFeatureToEquipment(request.EquipmentId, request.FeatureId);
            return Ok();
        }

        [Route("getFeatures")]
        [HttpPost]
        public async Task<IActionResult> GetFeatures(EquipmentsFilter filter)
        {
            (Equipment equipment, List<Feature> features) = await equipService.GetModelFeatures(filter);
            EquipmentResponse response = new EquipmentResponse(equipment.Id);
            response.Name = equipment.Name;
            response.Price = equipment.Price;
            response.ReleaseYear = equipment.ReleaseYear;   
            response.Features = features.Select(f => _featureRMFactory.CreateResponse(f)).ToList();

            return Ok(response);
        }
    }
}
