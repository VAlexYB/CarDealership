using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;

namespace CarDealership.DataAccess.Repositories
{
    public class BrandsRepository : BaseRepository<Brand, BrandEntity, BaseFilter>, IBrandsRepository
    {
        public BrandsRepository(CarDealershipDbContext context, IEntityModelFactory<Brand, BrandEntity> factory) : base(context, factory)
        {
        }
    }
}
