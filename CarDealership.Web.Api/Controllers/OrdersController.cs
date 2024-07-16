using CarDealership.Application.Services;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Enums;
using CarDealership.Core.Filters;
using CarDealership.Core.Models;
using CarDealership.Core.Models.Auth;
using CarDealership.Infrastructure.Messaging;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Web.Api.Controllers
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
            try
            {
                if(request.Status > Enum.GetValues(typeof(OrderStatus)).Cast<int>().Max())
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
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка возникла в OrdersController -> ChangeStatus()");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "Manager")]
        [Route("getFreeOrders")]
        [HttpGet]
        public async Task<IActionResult> GetOrdersWithoutManager()
        {
            try
            {
                var orders = await _ordersService.GetOrdersWithoutManager();
                var response = orders.Select(order => _orderRMFactory.CreateResponse(order)).ToList();
                return Ok(response);
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка возникла в OrdersController -> GetOrdersWithoutManager()");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "Manager")]
        [Route("takeInProcess")]
        [HttpPost]
        public async Task<IActionResult> TakeOrderInProcess([FromBody] TakeTaskRequest request)
        {
            try
            {
                await _ordersService.TakeOrderInProcess(request.ManagerId, request.TaskId);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка возникла в OrdersController -> TakeOrderInProcess()");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "Manager")]
        [Route("leaveOrder/{orderId}")]
        [HttpPost]
        public async Task<IActionResult> LeaveOrder(Guid orderId)
        {
            try
            {
                await _ordersService.LeaveOrder(orderId);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка возникла в OrdersController ->  LeaveOrder()");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "User")]
        [Route("cancelOrder/{orderId}")]
        [HttpGet]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            try
            {
                await _ordersService.ChangeStatus(orderId, (int)OrderStatus.Cancelled);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка возникла в OrdersController ->  CancelOrder()");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}
