using CarDealership.Application.Services;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Enums;
using CarDealership.Core.Filters;
using CarDealership.Core.Models;
using CarDealership.Core.Models.Auth;
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

        public OrdersController(IOrdersService service, IOrderRMFactory factory) : base(service, factory)
        {
            _ordersService = service;
            _orderRMFactory = factory;
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
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}
