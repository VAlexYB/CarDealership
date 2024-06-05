using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;

namespace CarDealership.Web.Api.Factories
{
    public class DealRMFactory : IDealRMFactory
    {
        private readonly IUsersService _usersService;
        private readonly ICarsService _carsService;

        private readonly ICarRMFactory _carRMFactory;
        public DealRMFactory(IUsersService usersService, ICarsService carsService, ICarRMFactory carRMFactory)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _carsService = carsService ?? throw new ArgumentNullException(nameof(carsService));
            _carRMFactory = carRMFactory ?? throw new ArgumentNullException(nameof(carRMFactory));
        }
        public async Task<Deal> CreateModelAsync(DealRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));


            var manager = req.ManagerId != null ? await _usersService.GetByIdAsync((Guid)req.ManagerId) ?? throw new ArgumentException($"Менеджер с Id = {req.ManagerId} не найден") : null;
            var customer = await _usersService.GetByIdAsync(req.CustomerId) ?? throw new ArgumentNullException($"Покупатель с Id = {req.CustomerId} не найден");
            var car = await _carsService.GetByIdAsync(req.CarId) ?? throw new ArgumentNullException($"Автомобиль с Id = {req.CarId} не найден");
            var dealPrice = car.AutoConfiguration.Price;

            var dealCreateResult = Deal.Create(
                req.Id,
                req.DealDate,
                req.Status,
                dealPrice,
                req.CarId,
                req.ManagerId != Guid.Empty ? req.ManagerId : null,
                req.CustomerId,
                false,
                car,
                manager,
                customer
            );

            if(dealCreateResult.IsFailure)
            {
                throw new InvalidOperationException(dealCreateResult.Error);
            }

            var deal = dealCreateResult.Value;

            customer.AddCustomerDeal(deal);

            if(manager != null)
            {
                manager.AddManagedDeal(deal);
            }
            return deal;
        }

        public DealResponse CreateResponse(Deal model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var response = new DealResponse(model.Id)
            {
                DealDate = model.DealDate,
                Status = model.Status.ToString(),
                Price = model.Price,
                CarId = model.CarId,
                Car = _carRMFactory.CreateResponse(model.Car),
                ManagerId = model.ManagerId,
                CustomerId = model.CustomerId,
            };
            return response;
        }
    }
}
