using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Components;

namespace CarDealership.Web.Api.Controllers
{
    [Route("api/equipment")]
    public class EquipmentsController : BaseController<Equipment, BaseFilter, EquipmentRequest, EquipmentResponse>
    {
        public EquipmentsController(IEquipmentsService service, IEquipmentRMFactory factory) : base(service, factory)
        {
        }
    }
}
