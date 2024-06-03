using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;

namespace CarDealership.Web.Api.Controllers
{
    public class DealsController : BaseController<Deal, BaseFilter, DealRequest, DealResponse>
    {
        public DealsController(IDealsService service, IDealRMFactory factory) : base(service, factory)
        {
        }
    }
}
