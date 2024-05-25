using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class CountriesService : BaseService<Country, BaseFilter>, ICountriesService
    {
        public CountriesService(ICountriesRepository repository) : base(repository)
        {
        }
    }
}
