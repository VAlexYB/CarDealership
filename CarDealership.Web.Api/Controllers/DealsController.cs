using CarDealership.Application.Services;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Enums;
using CarDealership.Core.Filters;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Web.Api.Controllers
{
    public class DealsController : BaseController<Deal, DealsFilter, DealRequest, DealResponse>
    {
        private readonly IDealsService _dealsService;
        private readonly IDealRMFactory _dealRMFactory;
        public DealsController(IDealsService service, IDealRMFactory factory) : base(service, factory)
        {
            _dealsService = service;
            _dealRMFactory = factory;
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
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "SeniorManager")]
        [Route("getFreeDeals")]
        [HttpGet]
        public async Task<IActionResult> GetDealsWithoutManager()
        {
            try
            {
                var deals = await _dealsService.GetDealsWithoutManager();
                var response = deals.Select(deal => _dealRMFactory.CreateResponse(deal)).ToList();
                return Ok(response);
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "SeniorManager")]
        [Route("takeInProcess")]
        [HttpPost]
        public async Task<IActionResult> TakeOrderInProcess([FromBody] TakeTaskRequest request)
        {
            try
            {
                await _dealsService.TakeDealInProcess(request.ManagerId, request.TaskId);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "SeniorManager")]
        [Route("leaveOrder/{dealId}")]
        [HttpPost]
        public async Task<IActionResult> LeaveOrder(Guid dealId)
        {
            try
            {
                await _dealsService.LeaveDeal(dealId);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}
