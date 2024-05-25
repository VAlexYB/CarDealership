using CarDealership.Core.Models;
using DriveType = CarDealership.Core.Models.DriveType;

namespace CarDealership.Core.Abstractions.Repositories
{
    public interface IDriveTypesRepository : IGenericRepository<DriveType, BaseFilter>
    {
    }
}
