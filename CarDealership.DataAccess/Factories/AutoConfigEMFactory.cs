using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using DriveType = CarDealership.Core.Models.DriveType;

namespace CarDealership.DataAccess.Factories
{
    public class AutoConfigEMFactory : IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity>
    {
        private readonly IEntityModelFactory<Car, CarEntity> _carEMFactory;
        public AutoConfigEMFactory(
            IEntityModelFactory<Car, CarEntity> carEMFactory
        )
        {
            _carEMFactory = carEMFactory ?? throw new ArgumentNullException(nameof(carEMFactory));

        }

        public AutoConfiguration CreateModel(AutoConfigurationEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var autoModel = entity.AutoModel != null ? AutoModel.Create(
                    entity.AutoModelId,
                    entity.AutoModel.Name,
                    entity.AutoModel.Price,
                    entity.AutoModel.BrandId,
                    entity.AutoModel.IsDeleted
            ).Value: null;

            var bodyType = entity.BodyType != null ? BodyType.Create(
                    entity.BodyTypeId,
                    entity.BodyType.Value,
                    entity.BodyType.Price,
                    entity.BodyType.IsDeleted
            ).Value : null;

            var driveType = entity.DriveType != null ? DriveType.Create(
                entity.DriveTypeId,
                entity.DriveType.Value,
                entity.DriveType.Price,
                entity.DriveType.IsDeleted
               
            ).Value : null;


            var engine = entity.Engine != null ? Engine.Create(
                entity.EngineId,
                entity.Engine.Power,
                entity.Engine.Consumption,
                entity.Engine.Price,
                entity.Engine.EngineTypeId,
                entity.Engine.TransmissionTypeId,
                entity.Engine.IsDeleted
            ).Value : null;

            var color = entity.Color != null ? Color.Create(
                entity.ColorId,
                entity.Color.Value,
                entity.Color.Price,
                entity.Color.IsDeleted
            ).Value : null;

            var autoConfigurationResult = AutoConfiguration.Create(
                entity.Id,
                entity.Price,
                entity.AutoModelId,
                entity.BodyTypeId,
                entity.DriveTypeId,
                entity.EngineId,
                entity.ColorId,
                entity.IsDeleted,
                autoModel,
                bodyType,
                driveType,
                engine,
                color
            );

            if (autoConfigurationResult.IsFailure)
            {
                throw new InvalidOperationException(autoConfigurationResult.Error);
            }

            var autoConfiguration = autoConfigurationResult.Value;

            foreach (var carEntity in entity.Cars)
            {
                autoConfiguration.AddCar(_carEMFactory.CreateModel(carEntity));
            }

            return autoConfiguration;
        }

        public AutoConfigurationEntity CreateEntity(AutoConfiguration model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var carEntities = model.Cars.Select(car => _carEMFactory.CreateEntity(car)).ToList();

            var entity = new AutoConfigurationEntity
            {
                Id = model.Id,
                Price = model.Price,
                AutoModelId = model.AutoModelId,
                BodyTypeId = model.BodyTypeId,
                DriveTypeId = model.DriveTypeId,
                EngineId = model.EngineId,
                ColorId = model.ColorId,
                IsDeleted = model.IsDeleted,
                Cars = carEntities
            };

            return entity;
        }
    }
}
