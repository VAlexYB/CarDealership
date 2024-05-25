using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class BrandsService : BaseService<Brand, BaseFilter>, IBrandsService
    {
        public BrandsService(IBrandsRepository repository) : base(repository)
        {
        }
    }
}
