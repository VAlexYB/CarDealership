using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;

namespace CarDealership.DataAccess.Repositories
{
    public class CarsRepository : BaseRepository<Car, CarEntity, BaseFilter>, ICarsRepository
    {
        public CarsRepository(CarDealershipDbContext context, CarEMFactory factory) : base(context, factory)
        {
        }
    }
}
