using CarDealership.Core.Filters;
using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Repositories
{
    public interface IAutoModelsRepository : IGenericRepository<AutoModel, AutoModelsFilter>
    {
    }
}
