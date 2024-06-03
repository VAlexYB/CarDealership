using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class DealsService : BaseService<Deal, BaseFilter>, IDealsService
    {
        public DealsService(IDealsRepository repository) : base(repository)
        {
        }
    }
}
