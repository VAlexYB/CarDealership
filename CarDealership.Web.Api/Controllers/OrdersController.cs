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
    public class OrdersController : BaseController<Order, OrdersFilter, OrderRequest, OrderResponse>
    {
        private readonly IOrdersService _ordersSerivce;

        public OrdersController(IOrdersService service, IOrderRMFactory factory) : base(service, factory)
        {
            _ordersSerivce = service;
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
                await _ordersSerivce.ChangeStatus(request.Id, request.Status);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}
