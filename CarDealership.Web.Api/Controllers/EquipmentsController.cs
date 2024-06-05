using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Filters;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Web.Api.Controllers
{
    [Route("api/[controller]")]
    public class EquipmentsController : BaseController<Equipment, EquipmentsFilter, EquipmentRequest, EquipmentResponse>
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
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Route("removeFeature")]
        [HttpPost]
        public async Task<IActionResult> RemoveFeature(EquipFeatureChangeRequest request)
        {
            try
            {
                await equipService.RemoveFeatureFromEquipment(request.EquipmentId, request.FeatureId);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Внутренняя ошибка");
            }
        }

        [Route("addFeature")]
        [HttpPost]
        public async Task<IActionResult> AddFeature(EquipFeatureChangeRequest request)
        {
            try
            {
                await equipService.AddFeatureToEquipment(request.EquipmentId, request.FeatureId);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ошибки случаются");
            }
        }
    } 
}
