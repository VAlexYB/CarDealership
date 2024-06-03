using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Repositories
{
    public interface IOrdersRepository : IGenericRepository<Order, BaseFilter>
    {
    }
}
