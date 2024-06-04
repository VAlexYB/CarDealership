using CarDealership.Core.Filters;
using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Services
{
    public interface IDealsService : IGenericService<Deal, DealsFilter>
    {
        Task<Guid> ChangeStatus(Guid id, int status);
    }
}
