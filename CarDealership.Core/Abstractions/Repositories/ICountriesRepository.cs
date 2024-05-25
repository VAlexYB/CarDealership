using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Repositories
{
    public interface ICountriesRepository : IGenericRepository<Country, BaseFilter>
    {
    }
}
