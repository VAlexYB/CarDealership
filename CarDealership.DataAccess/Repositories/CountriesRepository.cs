using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;
using Microsoft.Extensions.Caching.Distributed;

namespace CarDealership.DataAccess.Repositories
{
    public class CountriesRepository : BaseRepository<Country, CountryEntity, BaseFilter>, ICountriesRepository
    {
        public CountriesRepository(CarDealershipDbContext context, IEntityModelFactory<Country, CountryEntity> factory,  IDistributedCache cache) : base(context, factory, cache)
        {
        }
    }
}
