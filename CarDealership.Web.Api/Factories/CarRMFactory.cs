using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;

namespace CarDealership.Web.Api.Factories
{
    public class CarRMFactory : ICarRMFactory
    {
        private readonly IAutoConfigsService _autoConfigsService;

        private readonly IAutoConfigRMFactory _autoConfigRMFactory;

        public CarRMFactory(IAutoConfigsService autoConfigsService, IAutoConfigRMFactory autoConfigRMFactory)
        {
            _autoConfigsService = autoConfigsService;
            _autoConfigRMFactory = autoConfigRMFactory;
        }

        public async Task<Car> CreateModelAsync(CarRequest req)
        {
            if(req == null) throw new ArgumentNullException(nameof(req));

            var autoConfig = await _autoConfigsService.GetByIdAsync(req.AutoConfigurationId);

            var carCreateResult = Car.Create(req.Id, req.VIN, req.AutoConfigurationId, false, autoConfig);

            if(carCreateResult.IsFailure)
            {
                throw new InvalidOperationException(carCreateResult.Error);
            }

            var car = carCreateResult.Value;
            autoConfig.AddCar(car);
            return car;         
        }

        public CarResponse CreateResponse(Car model)
        {
            if(model == null) throw new ArgumentNullException(nameof(model));

            var autoConfigRes = _autoConfigRMFactory.CreateResponse(model.AutoConfiguration);
            var response = new CarResponse(model.Id)
            {
                VIN = model.VIN,
                Configuration = autoConfigRes
            };

            return response;
        }
    }
}
