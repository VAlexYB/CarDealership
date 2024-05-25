using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Services
{
    public interface ICarsService : IGenericService<Car, BaseFilter>
    {
    }
}
