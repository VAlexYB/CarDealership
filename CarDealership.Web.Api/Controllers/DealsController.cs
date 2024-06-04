using CarDealership.Application.Services;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Enums;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Web.Api.Controllers
{
    public class DealsController : BaseController<Deal, BaseFilter, DealRequest, DealResponse>
    {
        private readonly IDealsService _dealsService;
        public DealsController(IDealsService service, IDealRMFactory factory) : base(service, factory)
        {
            _dealsService = service;
        }

        [Authorize(Roles = "SeniorManager")]
        [Route("changeStatus")]
        [HttpPost]
        public async Task<IActionResult> ChangeStatus([FromBody] ChangeStatusRequest request)
        {
            try
            {
                if (request.Status > Enum.GetValues(typeof(DealStatus)).Cast<int>().Max())
                {
                    return BadRequest("Нет такого статуса заказа");
                }
                await _dealsService.ChangeStatus(request.Id, request.Status);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}
