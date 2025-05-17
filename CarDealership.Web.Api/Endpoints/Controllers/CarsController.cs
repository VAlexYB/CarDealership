using CarDealership.Application.Services;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Factories;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog.Config;

namespace CarDealership.Web.Api.Endpoints.Controllers
{
    public class CarsController : BaseController<Car, BaseFilter, CarRequest, CarResponse>
    {
        private readonly ICarsService _carsService;
        public CarsController(ICarsService service, ICarRMFactory factory, ILogger<CarsController> logger) : base(service, factory, logger)
        {
            _carsService = service;
        }

        //[Authorize(Roles = "Manager")]
        [Route("getFreeCars")]
        [HttpGet]
        public async Task<IActionResult> GetFreeCars()
        {
            var cars = await _carsService.GetFreeCars();

            IEnumerable<Task<CarResponse>> tasks = cars.Select(car => _factory.CreateResponse(car));
            List<CarResponse> response = (await Task.WhenAll(tasks)).ToList();
            return Ok(response);
        }
    }
}
