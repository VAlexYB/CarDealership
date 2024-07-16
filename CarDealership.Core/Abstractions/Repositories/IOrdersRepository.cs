using CarDealership.Core.Filters;
using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Repositories
{
    public interface IOrdersRepository : IGenericRepository<Order, OrdersFilter>
    {
        Task<List<Order>> GetOrdersWithoutManager();
    }
}
