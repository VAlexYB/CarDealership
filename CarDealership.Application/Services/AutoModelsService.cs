using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class AutoModelsService : BaseService<AutoModel, BaseFilter>, IAutoModelsService
    {
        public AutoModelsService(IAutoModelsRepository autoModelsRepository) : base(autoModelsRepository) 
        { }
    }
}
