using CarDealership.Application.Services;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Consts;
using CarDealership.Core.Exceptions;
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
        private readonly IDealsService _dealsService;

        private readonly ICarRMFactory _carRMFactory;
        public DealRMFactory(IUsersService usersService, ICarsService carsService, IDealsService dealsService, ICarRMFactory carRMFactory)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _carsService = carsService ?? throw new ArgumentNullException(nameof(carsService));
            _dealsService = dealsService ?? throw new ArgumentNullException(nameof(dealsService));
            _carRMFactory = carRMFactory ?? throw new ArgumentNullException(nameof(carRMFactory));
        }
        public async Task<Deal> CreateModel(DealRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));


            var manager = req.ManagerId != null ? await _usersService.GetByIdAsync((Guid)req.ManagerId) ?? throw new ArgumentException($"Менеджер с Id = {req.ManagerId} не найден") : null;
            var customer = await _usersService.GetByIdAsync(req.CustomerId) ?? throw new ArgumentNullException($"Покупатель с Id = {req.CustomerId} не найден");
            var car = await _carsService.GetByIdAsync(req.CarId) ?? throw new ArgumentNullException($"Автомобиль с Id = {req.CarId} не найден");

            decimal dealPrice = 0.0m;

            if (req.Id == Guid.Empty)
            {
                dealPrice = req.Price * CDConstants.PaymentPartition.Deal;
            }
            else
            {
                dealPrice = await _dealsService.GetDealPrice(req.Id);
            }

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
                throw new ClientInformationException(dealCreateResult.Error);
            }

            var deal = dealCreateResult.Value;

            customer.AddCustomerDeal(deal);

            if(manager != null)
            {
                manager.AddManagedDeal(deal);
            }
            return deal;
        }

        public async Task<DealResponse> CreateResponse(Deal model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var car = await _carsService.GetByIdAsync(model.CarId);

            var response = new DealResponse(model.Id)
            {
                DealDate = model.DealDate,
                Status = model.Status,
                Price = model.Price,
                CarId = model.CarId,
                Car = await _carRMFactory.CreateResponse(car),
                ManagerId = model.ManagerId,
                CustomerId = model.CustomerId,
            };
            return response;
        }
    }
}
