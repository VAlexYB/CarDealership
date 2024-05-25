using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Repositories
{
    public interface ICarsRepository : IGenericRepository<Car, BaseFilter>
    {
    }
}
