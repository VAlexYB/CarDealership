using CarDealership.Core.Filters;
using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Services
{
    public interface IOrdersService : IGenericService<Order, OrdersFilter>
    {
        Task<Guid> ChangeStatus(Guid id, int status);
    }
}
