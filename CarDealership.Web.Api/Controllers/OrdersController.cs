using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Enums;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Web.Api.Controllers
{
    public class OrdersController : BaseController<Order, BaseFilter, OrderRequest, OrderResponse>
    {
        private readonly IOrdersService _ordersSerivce;

        public OrdersController(IOrdersService service, IOrderRMFactory factory) : base(service, factory)
        {
            _ordersSerivce = service;
        }

        [Route("changeStatus/{status}")]
        [HttpGet]
        public async Task<IActionResult> RemoveFeature(int status)
        {
            try
            {
                if(status < Enum.GetValues(typeof(OrderStatus)).Cast<int>().Max())
                await _ordersSerivce.ChangeStatus(4);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Ошибки случаются");
            }
        }
    }
}
