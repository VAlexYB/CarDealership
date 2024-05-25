using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class FeaturesService : BaseService<Feature, BaseFilter>, IFeaturesService
    {
        public FeaturesService(IFeaturesRepository repository) : base(repository)
        {
        }
    }
}
