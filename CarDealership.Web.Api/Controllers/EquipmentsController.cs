using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Web.Api.Controllers
{
    [Route("api/[controller]")]
    public class EquipmentsController : BaseController<Equipment, BaseFilter, EquipmentRequest, EquipmentResponse>
    {
        private readonly IEquipmentRMFactory _equipRMFactory;
        private readonly IEquipmentsService equipService;
        public EquipmentsController(IEquipmentsService service, IEquipmentRMFactory factory) : base(service, factory)
        {
            _equipRMFactory = factory ?? throw new ArgumentNullException(nameof(factory));
            equipService = service ?? throw new ArgumentNullException(nameof(service));
        }

        public override async Task<IActionResult> CreateOrEdit(EquipmentRequest request)
        {
            try
            {
                Equipment model = await _equipRMFactory.CreateModelAsync(request);
                await equipService.CreateOrEditAsync(model, request.FeatureIds);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Ошибки случаются");
            }
        }

        [Route("removeFeature")]
        [HttpPost]
        public async Task<IActionResult> RemoveFeature(EquipFeatureRemoveRequest request)
        {
            try
            {
                await equipService.RemoveFeatureFromEquipment(request.EquipmentId, request.FeatureId);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Ошибки случаются");
            }
        }
    } 
}
