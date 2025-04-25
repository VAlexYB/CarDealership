using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Enums;
using CarDealership.Core.Filters;
using CarDealership.Core.Models;
using CarDealership.Infrastructure.Messaging;
using CarDealership.Shared.Messaging;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Web.Api.Endpoints.Controllers
{
    public class DealsController : BaseController<Deal, DealsFilter, DealRequest, DealResponse>
    {
        private readonly IDealsService _dealsService;
        private readonly IDealRMFactory _dealRMFactory;
        private readonly ILogger<DealsController> _logger;
        private readonly IRabbitMQMessageSender _messageSender;
        private readonly IConfiguration _configuration;
        public DealsController(
            IDealsService service,
            IDealRMFactory factory,
            ILogger<DealsController> logger,
            IRabbitMQMessageSender messageSender,
            IConfiguration configuration
        ) : base(service, factory, logger)
        {
            _dealsService = service ?? throw new ArgumentNullException(nameof(service));
            _dealRMFactory = factory ?? throw new ArgumentNullException(nameof(factory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _messageSender = messageSender;
            _configuration = configuration;
        }

        [Authorize(Roles = "SeniorManager")]
        [Route("changeStatus")]
        [HttpPost]
        public async Task<IActionResult> ChangeStatus([FromBody] ChangeStatusRequest request)
        {
            if (request.Status > Enum.GetValues(typeof(DealStatus)).Cast<int>().Max())
            {
                return BadRequest("Нет такого статуса заказа");
            }
            await _dealsService.ChangeStatus(request.Id, request.Status);

            var dealInfo = await _dealsService.GetByIdAsync(request.Id);
            var message = new MessageInfo
            {
                Id = dealInfo.Id.ToString(),
                PhoneNumber = dealInfo?.Customer?.PhoneNumber ?? string.Empty,
                Status = request.Status.ToString("d"),
                Type = MessageTypes.Deal.ToString("g"),
                Path = request.Path ?? string.Empty
            };
            _messageSender.SendMessage(message, _configuration["RabbitMQ:Queues:CDQueue"]);

            return Ok();
        }

        [Authorize(Roles = "SeniorManager")]
        [Route("getFreeDeals")]
        [HttpGet]
        public async Task<IActionResult> GetDealsWithoutManager()
        {
            var deals = await _dealsService.GetDealsWithoutManager();
            var response = deals.Select(deal => _dealRMFactory.CreateResponse(deal)).ToList();
            return Ok(response);
        }

        [Authorize(Roles = "SeniorManager")]
        [Route("takeInProcess")]
        [HttpPost]
        public async Task<IActionResult> TakeDealInProcess([FromBody] TakeTaskRequest request)
        {
            await _dealsService.TakeDealInProcess(request.ManagerId, request.TaskId);
            return Ok();
        }

        [Authorize(Roles = "SeniorManager")]
        [Route("leaveOrder/{dealId}")]
        [HttpPost]
        public async Task<IActionResult> LeaveDeal(Guid dealId)
        {
            await _dealsService.LeaveDeal(dealId);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("analytics")]
        [HttpGet]
        public async Task<IActionResult> GetAnalytics([FromQuery] bool byConfiguration)
        {
            var stats = await _dealsService.GetAnalytics(byConfiguration);
            return Ok(stats);
        }
    }
}
