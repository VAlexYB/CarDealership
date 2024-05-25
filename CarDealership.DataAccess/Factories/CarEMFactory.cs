using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class CarEMFactory : IEntityModelFactory<Car, CarEntity>
    {
        private readonly AutoConfigEMFactory _autoConfigEMFactory;

        public CarEMFactory(AutoConfigEMFactory autoConfigEMFactory)
        {
            _autoConfigEMFactory = autoConfigEMFactory ?? throw new ArgumentNullException(nameof(autoConfigEMFactory));
        }
        public CarEntity CreateEntity(Car model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var autoConfigEntity = model.AutoConfiguration  != null ? _autoConfigEMFactory.CreateEntity(model.AutoConfiguration) : null;

            var entity = new CarEntity
            { 
                Id = model.Id,
                AutoConfiguration = autoConfigEntity,
                IsDeleted = model.IsDeleted,
                AutoConfigurationId = model.AutoConfigurationId,
                VIN = model.VIN
            };

            return entity;
        }

        public Car CreateModel(CarEntity entity)
        {
            if(entity == null) throw new ArgumentNullException(nameof(entity));

            var autoConfigModel = entity.AutoConfiguration != null ? _autoConfigEMFactory.CreateModel(entity.AutoConfiguration) : null;

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
