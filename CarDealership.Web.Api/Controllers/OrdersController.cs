using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;

namespace CarDealership.Web.Api.Controllers
{
    public class OrdersController : BaseController<Order, BaseFilter, OrderRequest, OrderResponse>
    {
        public OrdersController(IOrdersService service, IOrderRMFactory factory) : base(service, factory)
        {
        }
    }
}
