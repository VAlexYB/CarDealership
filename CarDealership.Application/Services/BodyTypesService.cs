using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class BodyTypesService : BaseService<BodyType, BaseFilter>, IBodyTypesService
    {
        public BodyTypesService(IBodyTypesRepository repository) : base(repository)
        {
        }
    }
}
