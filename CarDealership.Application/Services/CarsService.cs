using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class CarsService : BaseService<Car, BaseFilter>, ICarsService
    {
        public CarsService(ICarsRepository repository) : base(repository)
        {
        }
    }
}
