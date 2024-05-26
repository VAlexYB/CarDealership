using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;

namespace CarDealership.DataAccess.Repositories
{
    public class EquipmentsRepository : BaseRepository<Equipment, EquipmentEntity, BaseFilter>, IEquipmentsRepository
    {
        public EquipmentsRepository(CarDealershipDbContext context, IEntityModelFactory<Equipment, EquipmentEntity> factory) : base(context, factory)
        {
        }
    }
}
