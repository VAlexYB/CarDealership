using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class AutoConfigsService : BaseService<AutoConfiguration, BaseFilter>, IAutoConfigsService
    {
        public AutoConfigsService(IAutoConfigsRepository repository) : base(repository)
        {
        }
    }
}
