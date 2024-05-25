using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;
using DriveType = CarDealership.Core.Models.DriveType;
namespace CarDealership.DataAccess.Repositories
{
    public class DriveTypesRepository : BaseRepository<DriveType, DriveTypeEntity, BaseFilter>, IDriveTypesRepository
    {
        public DriveTypesRepository(CarDealershipDbContext context, DriveTypeEMFactory factory) : base(context, factory)
        {
        }
    }
}
