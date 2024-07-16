using CarDealership.Core.Filters;
using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Services
{
    public interface IAutoModelsService : IGenericService<AutoModel, AutoModelsFilter>
    {
    }
}
