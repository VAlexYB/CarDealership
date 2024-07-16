using CarDealership.Core.Filters;
using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Services
{
    public interface IOrdersService : IGenericService<Order, OrdersFilter>
    {
        Task<Guid> ChangeStatus(Guid id, int status);
        Task<List<Order>> GetOrdersWithoutManager();
        Task<Guid> TakeOrderInProcess(Guid managerId, Guid taskId);
        Task<Guid> LeaveOrder(Guid taskId);
    }
}
