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
        private readonly ILogger _logger;
        public DealsController(IDealsService service, IDealRMFactory factory, ILogger<DealsController> logger) : base(service, factory, logger)
        {
            _dealsService = service ?? throw new ArgumentNullException(nameof(service));
            _dealRMFactory = factory ?? throw new ArgumentNullException(nameof(factory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка возникла в DealsController -> ChangeStatus()");
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
                _logger.LogError(e, "Ошибка возникла в DealsController -> GetDealsWithoutManager()");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "SeniorManager")]
        [Route("takeInProcess")]
        [HttpPost]
        public async Task<IActionResult> TakeDealInProcess([FromBody] TakeTaskRequest request)
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
                _logger.LogError(e, "Ошибка возникла в DealsController -> TakeDealInProcess()");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "SeniorManager")]
        [Route("leaveOrder/{dealId}")]
        [HttpPost]
        public async Task<IActionResult> LeaveDeal(Guid dealId)
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
                _logger.LogError(e, "Ошибка возникла в DealsController -> LeaveDeal()");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}
