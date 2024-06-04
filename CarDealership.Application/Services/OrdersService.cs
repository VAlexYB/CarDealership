using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class OrdersService : BaseService<Order, BaseFilter>, IOrdersService
    {
        public OrdersService(IOrdersRepository repository) : base(repository)
        {
        }

        public Task<Guid> ChangeStatus(int status)
        {
            throw new NotImplementedException();
        }
    }
}
