using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
   public interface IEntityModelFactory <M, E>
        where M : BaseModel
        where E : BaseEntity
    {
        M CreateModel(E entity);
        E CreateEntity(M model);
    }
}
