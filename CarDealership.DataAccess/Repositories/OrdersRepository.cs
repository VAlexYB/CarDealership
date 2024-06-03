using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;

namespace CarDealership.DataAccess.Repositories
{
    public class OrdersRepository : BaseRepository<Order, OrderEntity, BaseFilter>, IOrdersRepository
    {
        public OrdersRepository(CarDealershipDbContext context, IEntityModelFactory<Order, OrderEntity> factory) : base(context, factory)
        {
        }
    }
}
