using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class ColorsService : BaseService<Color, BaseFilter>, IColorsService
    {
        public ColorsService(IColorsRepository repository) : base(repository)
        {
        }
    }
}
