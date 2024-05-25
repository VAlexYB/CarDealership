using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;

namespace CarDealership.DataAccess.Repositories
{
    public class ColorsRepository : BaseRepository<Color, ColorEntity, BaseFilter>, IColorsRepository
    {
        public ColorsRepository(CarDealershipDbContext context, ColorEMFactory factory) : base(context, factory)
        {
        }
    }
}
