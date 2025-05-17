using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Enums;
using CarDealership.Core.Exceptions;
using CarDealership.Core.Filters;
using CarDealership.Core.Models;
using System.Threading.Tasks;

namespace CarDealership.Application.Services
{
    public class OrdersService : BaseService<Order, OrdersFilter>, IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        public OrdersService(IOrdersRepository repository) : base(repository)
        {
            _ordersRepository = repository;
        }

        public async Task<Guid> ChangeStatus(Guid id, int status)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null) throw new ClientInformationException("Заказ не найден");
            order.ChangeStatus((OrderStatus)status);
            await _repository.UpdateAsync(order);
            return order.Id;
        }

        public async Task<List<Order>> GetOrdersWithoutManager()
        {
            return await _ordersRepository.GetOrdersWithoutManager();
        }


        public async Task<Guid> TakeOrderInProcess(Guid managerId, Guid taskId)
        {
            var order = await _repository.GetByIdAsync(taskId);
            if (order == null) throw new ClientInformationException("Заказ не найден");
            order.SetAsManager(managerId);
            order.ChangeStatus(OrderStatus.Processing);
            await _repository.UpdateAsync(order);
            return order.Id;
        }

        public async Task<Guid> LeaveOrder(Guid taskId)
        {
            var order = await _repository.GetByIdAsync(taskId);
            if (order == null) throw new ClientInformationException("Заказ не найден");
            order.RemoveManager();
            order.ChangeStatus(OrderStatus.Pending);
            await _repository.UpdateAsync(order);
            return order.Id;
        }

        public async Task<decimal> GetOrderPrice(Guid orderId)
        {
            var order = await _repository.GetByIdAsync(orderId);
            return order.Price;
        }
    }
}
