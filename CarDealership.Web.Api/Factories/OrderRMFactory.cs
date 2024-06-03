using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;

namespace CarDealership.Web.Api.Factories
{
    public class OrderRMFactory : IOrderRMFactory
    {
        private readonly IUsersService _usersService;
        private readonly ICarsService _carsService;

        private readonly ICarRMFactory _carRMFactory;
        public OrderRMFactory(IUsersService usersService, ICarsService carsService, ICarRMFactory carRMFactory)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _carsService = carsService ?? throw new ArgumentNullException(nameof(carsService));
            _carRMFactory = carRMFactory ?? throw new ArgumentNullException(nameof(carRMFactory));
        }
        public async Task<Order> CreateModelAsync(OrderRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));


            var manager = req.ManagerId != null ? await _usersService.GetByIdAsync((Guid)req.ManagerId) ?? throw new ArgumentException($"Менеджер с Id = {req.ManagerId} не найден") : null;
            var customer = await _usersService.GetByIdAsync(req.CustomerId) ?? throw new ArgumentNullException($"Покупатель с Id = {req.CustomerId} не найден");
            var car = await _carsService.GetByIdAsync(req.CarId) ?? throw new ArgumentNullException($"Автомобиль с Id = {req.CarId} не найден");

            var orderCreateResult = Order.Create(
                req.Id,
                req.OrderDate,
                req.CompleteDate,
                req.Status,
                req.CarId,
                req.ManagerId,
                req.CustomerId,
                false,
                car,
                manager,
                customer
            );

            if (orderCreateResult.IsFailure)
            {
                throw new InvalidOperationException(orderCreateResult.Error);
            }

            var order = orderCreateResult.Value;

            customer.AddCustomerOrder(order);

            if (manager != null)
            {
                manager.AddCustomerOrder(order);
            }
            return order;
        }

        public OrderResponse CreateResponse(Order model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var response = new OrderResponse(model.Id)
            {
                OrderDate = model.OrderDate,
                CompleteDate = model.CompleteDate,
                Status = model.Status.ToString(),
                CarId = model.CarId,
                Car = _carRMFactory.CreateResponse(model.Car),
                ManagerId = model.ManagerId,
                CustomerId = model.CustomerId,
            };
            return response;
        }
    }
}
