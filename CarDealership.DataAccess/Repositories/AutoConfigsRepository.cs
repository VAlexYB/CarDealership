using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;

namespace CarDealership.DataAccess.Repositories
{
    public class AutoConfigsRepository : BaseRepository<AutoConfiguration, AutoConfigurationEntity, BaseFilter>, IAutoConfigsRepository
    {
        public AutoConfigsRepository(CarDealershipDbContext context, IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity> factory) : base(context, factory)
        {
        }
    }
}
