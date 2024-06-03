using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Repositories
{
    public interface IDealsRepository : IGenericRepository<Deal, BaseFilter>
    {
    }
}
