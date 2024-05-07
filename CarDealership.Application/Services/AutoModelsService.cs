using CarDealership.Core.Abstractions;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class AutoModelsService : BaseService<AutoModel, BaseFilter>, IAutoModelsService
    {
        public AutoModelsService(IAutoModelsRepository autoModelsRepository) : base(autoModelsRepository) 
        { }
    }
}
