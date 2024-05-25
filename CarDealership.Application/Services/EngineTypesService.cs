using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class EngineTypesService : BaseService<EngineType, BaseFilter>, IEngineTypesService
    {
        public EngineTypesService(IEngineTypesRepository repository) : base(repository)
        {
        }
    }
}
