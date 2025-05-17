using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Consts;
using CarDealership.Core.Exceptions;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;

namespace CarDealership.Web.Api.Factories
{
    public class OrderRMFactory : IOrderRMFactory
    {
        private readonly IUsersService _usersService;
        private readonly IAutoConfigsService _configsService;
        private readonly IOrdersService _ordersService;

        private readonly IAutoConfigRMFactory _configsRMFactory;
        public OrderRMFactory 
        (
            IUsersService usersService,
            IAutoConfigsService configsService,
            IOrdersService ordersService,
            IAutoConfigRMFactory configsRMFactory
        )
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _configsService = configsService ?? throw new ArgumentNullException(nameof(configsService));
            _ordersService = ordersService ?? throw new ArgumentNullException(nameof(configsService));
            _configsRMFactory = configsRMFactory ?? throw new ArgumentNullException(nameof(configsRMFactory));
        }
        public async Task<Order> CreateModel(OrderRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));


            var manager = req.ManagerId != Guid.Empty ? await _usersService.GetByIdAsync((Guid)req.ManagerId) ?? throw new ArgumentException($"Менеджер с Id = {req.ManagerId} не найден") : null;
            var customer = await _usersService.GetByIdAsync(req.CustomerId) ?? throw new ArgumentNullException($"Покупатель с Id = {req.CustomerId} не найден");
            var autoConfiguration = await _configsService.GetByIdAsync(req.AutoConfigurationId) ?? throw new ArgumentNullException($"Конфигурация машины с Id = {req.AutoConfigurationId} не найден");

            decimal orderPrice = 0.0m;

            if (req.Id ==  Guid.Empty)
            {
                orderPrice = req.Price * CDConstants.PaymentPartition.Order;
            }
            else
            {
                orderPrice = await _ordersService.GetOrderPrice(req.Id);
            }

            var orderCreateResult = Order.Create(
                req.Id,
                req.OrderDate,
                req.CompleteDate,
                req.Status,
                orderPrice,
                req.AutoConfigurationId,
                req.ManagerId != Guid.Empty ? req.ManagerId : null,
                req.CustomerId,
                false,
                autoConfiguration,
                manager,
                customer
            );

            if (orderCreateResult.IsFailure)
            {
                throw new ClientInformationException(orderCreateResult.Error);
            }

            var order = orderCreateResult.Value;

            customer.AddCustomerOrder(order);

            if (manager != null)
            {
                manager.AddCustomerOrder(order);
            }
            return order;
        }

        public async Task<OrderResponse> CreateResponse(Order model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var autoConfig = await _configsService.GetByIdAsync(model.AutoConfigurationId);

            var response = new OrderResponse(model.Id)
            {
                OrderDate = model.OrderDate,
                CompleteDate = model.CompleteDate,
                Status = model.Status,
                Price = model.Price,
                AutoConfigurationId = model.AutoConfigurationId,
                AutoConfiguration = await _configsRMFactory.CreateResponse(autoConfig),
                ManagerId = model.ManagerId,
                CustomerId = model.CustomerId,
            };
            return response;
        }
    }
}
