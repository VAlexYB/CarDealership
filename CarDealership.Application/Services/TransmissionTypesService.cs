using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class TransmissionTypesService : BaseService<TransmissionType, BaseFilter>, ITransmissionTypesService
    {
        public TransmissionTypesService(ITransmissionTypesRepository repository) : base(repository)
        {
        }
    }
}
