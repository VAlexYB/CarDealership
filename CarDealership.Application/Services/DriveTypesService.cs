using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using DriveType = CarDealership.Core.Models.DriveType;

namespace CarDealership.Application.Services
{
    public class DriveTypesService : BaseService<DriveType, BaseFilter>, IDriveTypesService
    {
        public DriveTypesService(IDriveTypesRepository repository) : base(repository)
        {
        }
    }
}
