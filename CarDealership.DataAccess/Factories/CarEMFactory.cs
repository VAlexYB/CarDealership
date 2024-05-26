using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class CarEMFactory : IEntityModelFactory<Car, CarEntity>
    {
        public CarEntity CreateEntity(Car model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var entity = new CarEntity
            { 
                Id = model.Id,
                IsDeleted = model.IsDeleted,
                AutoConfigurationId = model.AutoConfigurationId,
                VIN = model.VIN
            };

            return entity;
        }

        public Car CreateModel(CarEntity entity)
        {
            if(entity == null) throw new ArgumentNullException(nameof(entity));

            var autoConfigModel = entity.AutoConfiguration != null ? AutoConfiguration.Create(
                entity.AutoConfigurationId,
                entity.AutoConfiguration.Price,
                entity.AutoConfiguration.AutoModelId,
                entity.AutoConfiguration.BodyTypeId,
                entity.AutoConfiguration.DriveTypeId,
                entity.AutoConfiguration.EngineId,
                entity.AutoConfiguration.ColorId,
                entity.AutoConfiguration.IsDeleted
            ).Value : null;

            var carCreateResult = Car.Create(
                entity.Id,
                entity.VIN,
                entity.AutoConfigurationId,
                entity.IsDeleted,
                autoConfigModel
            );

            if(carCreateResult.IsFailure)
            {
                throw new InvalidOperationException(carCreateResult.Error);
            }

            var car = carCreateResult.Value;

            return car;
        }
    }
}
