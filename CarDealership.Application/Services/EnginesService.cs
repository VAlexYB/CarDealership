using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class EnginesService : BaseService<Engine, BaseFilter>, IEnginesService
    {
        public EnginesService(IEnginesRepository repository) : base(repository)
        {
        }
    }
}
