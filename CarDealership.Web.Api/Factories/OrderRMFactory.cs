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
        private readonly IAutoConfigsService _configsService;

        private readonly IAutoConfigRMFactory _configsRMFactory;
        public OrderRMFactory(IUsersService usersService, IAutoConfigsService configsService, IAutoConfigRMFactory configsRMFactory)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _configsService = configsService ?? throw new ArgumentNullException(nameof(configsService));
            _configsRMFactory = configsRMFactory ?? throw new ArgumentNullException(nameof(configsRMFactory));
        }
        public async Task<Order> CreateModelAsync(OrderRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));


            var manager = req.ManagerId != Guid.Empty ? await _usersService.GetByIdAsync((Guid)req.ManagerId) ?? throw new ArgumentException($"Менеджер с Id = {req.ManagerId} не найден") : null;
            var customer = await _usersService.GetByIdAsync(req.CustomerId) ?? throw new ArgumentNullException($"Покупатель с Id = {req.CustomerId} не найден");
            var autoConfiguration = await _configsService.GetByIdAsync(req.AutoConfigurationId) ?? throw new ArgumentNullException($"Конфигурация машины с Id = {req.AutoConfigurationId} не найден");
            decimal autoModelPrice = autoConfiguration.AutoModel?.Price ?? 0;
            decimal bodyTypePrice = autoConfiguration.BodyType?.Price ?? 0;
            decimal driveTypePrice = autoConfiguration.DriveType?.Price ?? 0;
            decimal colorPrice = autoConfiguration.Color?.Price ?? 0;
            decimal equipmentPrice = autoConfiguration.Equipment?.Price ?? 0;
            var orderPrice = (autoConfiguration.Price + autoModelPrice + bodyTypePrice + driveTypePrice + colorPrice + equipmentPrice) * 0.30m;

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
                Status = model.Status,
                Price = model.Price,
                AutoConfigurationId = model.AutoConfigurationId,
                AutoConfiguration = _configsRMFactory.CreateResponse(model.AutoConfiguration),
                ManagerId = model.ManagerId,
                CustomerId = model.CustomerId,
            };
            return response;
        }
    }
}
