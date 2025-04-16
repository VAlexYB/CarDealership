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
        private readonly IEquipmentsService equipService;
        private readonly ILogger _logger;
        public EquipmentsController(IEquipmentsService service, IEquipmentRMFactory factory, ILogger<EquipmentsController> logger) : base(service, factory, logger)
        {
            _equipRMFactory = factory ?? throw new ArgumentNullException(nameof(factory));
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
    }
}
