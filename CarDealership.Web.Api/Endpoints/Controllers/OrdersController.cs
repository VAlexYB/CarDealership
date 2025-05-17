using CarDealership.Application.Services;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Enums;
using CarDealership.Core.Filters;
using CarDealership.Core.Models;
using CarDealership.Core.Models.Auth;
using CarDealership.Infrastructure.Messaging;
using CarDealership.Shared.Messaging;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Web.Api.Endpoints.Controllers
{
    public class OrdersController : BaseController<Order, OrdersFilter, OrderRequest, OrderResponse>
    {
        private readonly IOrdersService _ordersService;
        private readonly IOrderRMFactory _orderRMFactory;
        private readonly ILogger<OrdersController> _logger;
        private readonly IRabbitMQMessageSender _messageSender;
        private readonly IConfiguration _configuration;

        public OrdersController(
            IOrdersService service,
            IOrderRMFactory factory,
            ILogger<OrdersController> logger,
            IRabbitMQMessageSender messageSender,
            IConfiguration configuration
        ) : base(service, factory, logger)
        {
            _ordersService = service;
            _orderRMFactory = factory;
            _logger = logger;
            _messageSender = messageSender;
            _configuration = configuration;
        }

        [Authorize(Roles = "Manager")]
        [Route("changeStatus")]
        [HttpPost]
        public async Task<IActionResult> ChangeStatus([FromBody] ChangeStatusRequest request)
        {
            if (request.Status > Enum.GetValues(typeof(OrderStatus)).Cast<int>().Max())
            {
                return BadRequest("Нет такого статуса заказа");
            }
            await _ordersService.ChangeStatus(request.Id, request.Status);

            var orderInfo = await _ordersService.GetByIdAsync(request.Id);
            var message = new MessageInfo
            {
                Id = orderInfo.Id.ToString(),
                PhoneNumber = orderInfo?.Customer?.PhoneNumber ?? string.Empty,
                Status = request.Status.ToString("d"),
                Type = MessageTypes.Order.ToString("g")
            };
            _messageSender.SendMessage(message, _configuration["RabbitMQ:Queues:CDQueue"]);
            return Ok();
        }

        [Authorize(Roles = "Manager")]
        [Route("getFreeOrders")]
        [HttpGet]
        public async Task<IActionResult> GetOrdersWithoutManager()
        {
            var orders = await _ordersService.GetOrdersWithoutManager();

            IEnumerable<Task<OrderResponse>> tasks = orders.Select(order => _orderRMFactory.CreateResponse(order));
            var response = (await Task.WhenAll(tasks)).ToList();
            return Ok(response);
        }

        [Authorize(Roles = "Manager")]
        [Route("takeInProcess")]
        [HttpPost]
        public async Task<IActionResult> TakeOrderInProcess([FromBody] TakeTaskRequest request)
        {
            await _ordersService.TakeOrderInProcess(request.ManagerId, request.TaskId);
            return Ok();
        }

        [Authorize(Roles = "Manager")]
        [Route("leaveOrder/{orderId}")]
        [HttpGet]
        public async Task<IActionResult> LeaveOrder(Guid orderId)
        {
            await _ordersService.LeaveOrder(orderId);
            return Ok();
        }

        [Authorize(Roles = "User")]
        [Route("cancelOrder/{orderId}")]
        [HttpGet]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            await _ordersService.ChangeStatus(orderId, (int)OrderStatus.Cancelled);
            return Ok();
        }
    }
}
