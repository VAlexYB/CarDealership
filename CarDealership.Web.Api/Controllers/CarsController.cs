using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories;
using CarDealership.Web.Api.Factories.Abstract;

namespace CarDealership.Web.Api.Controllers
{
    public class CarsController : BaseController<Car, BaseFilter, CarRequest, CarResponse>
    {
        public CarsController(ICarsService service, ICarRMFactory factory) : base(service, factory)
        {
        }
    }
}
