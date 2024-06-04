using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Enums;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class OrdersService : BaseService<Order, BaseFilter>, IOrdersService
    {
        public OrdersService(IOrdersRepository repository) : base(repository)
        {
        }

        public async Task<Guid> ChangeStatus(Guid id, int status)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null) throw new InvalidOperationException("Заказ не найден");
            order.ChangeStatus((OrderStatus)status);
            await _repository.UpdateAsync(order);
            return order.Id;
        }
    }
}
