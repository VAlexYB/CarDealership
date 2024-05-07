using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class AutoModelEMFactory : IEntityModelFactory<AutoModel, AutoModelEntity>
    {
        public AutoModel CreateModel(AutoModelEntity entity)
        {
            return AutoModel.Create(entity.Id, entity.Brand, entity.Name, entity.BodyType, entity.Price).model;
        }

        public AutoModelEntity CreateEntity(AutoModel model)
        {
            return new AutoModelEntity(model);
        }

    }
}
