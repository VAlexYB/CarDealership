using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;

namespace CarDealership.DataAccess.Repositories
{
    public class DealsRepository : BaseRepository<Deal, DealEntity, BaseFilter>, IDealsRepository
    {
        public DealsRepository(CarDealershipDbContext context, IEntityModelFactory<Deal, DealEntity> factory) : base(context, factory)
        {
        }
    }
}
