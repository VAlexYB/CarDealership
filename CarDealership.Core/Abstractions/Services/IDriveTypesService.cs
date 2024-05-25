using CarDealership.Core.Models;
using DriveType = CarDealership.Core.Models.DriveType;

namespace CarDealership.Core.Abstractions.Services
{
    public interface IDriveTypesService : IGenericService<DriveType, BaseFilter>
    {
    }
}
