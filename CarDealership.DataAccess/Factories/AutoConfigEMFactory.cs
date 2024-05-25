using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class AutoConfigEMFactory : IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity>
    {
        private readonly AutoModelEMFactory _autoModelEMFactory;
        private readonly BodyTypeEMFactory _bodyTypeEMFactory;
        private readonly DriveTypeEMFactory _driveTypeEMFactory;
        private readonly EngineEMFactory _engineEMFactory;
        private readonly ColorEMFactory _colorEMFactory;
        private readonly CarEMFactory _carEMFactory;

        public AutoConfigEMFactory(
            AutoModelEMFactory autoModelEMFactory,
            BodyTypeEMFactory bodyTypeEMFactory,
            DriveTypeEMFactory driveTypeEMFactory,
            EngineEMFactory engineEMFactory,
            ColorEMFactory colorEMFactory,
            CarEMFactory carEMFactory)
        {
            _autoModelEMFactory = autoModelEMFactory ?? throw new ArgumentNullException(nameof(autoModelEMFactory));
            _bodyTypeEMFactory = bodyTypeEMFactory ?? throw new ArgumentNullException(nameof(bodyTypeEMFactory));
            _driveTypeEMFactory = driveTypeEMFactory ?? throw new ArgumentNullException(nameof(driveTypeEMFactory));
            _engineEMFactory = engineEMFactory ?? throw new ArgumentNullException(nameof(engineEMFactory));
            _colorEMFactory = colorEMFactory ?? throw new ArgumentNullException(nameof(colorEMFactory));
            _carEMFactory = carEMFactory;
        }

        public AutoConfiguration CreateModel(AutoConfigurationEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var autoModel = entity.AutoModel != null ? _autoModelEMFactory.CreateModel(entity.AutoModel) : null;
            var bodyType = entity.BodyType != null ? _bodyTypeEMFactory.CreateModel(entity.BodyType) : null;
            var driveType = entity.DriveType != null ? _driveTypeEMFactory.CreateModel(entity.DriveType) : null;
            var engine = entity.Engine != null ? _engineEMFactory.CreateModel(entity.Engine) : null;
            var color = entity.Color != null ? _colorEMFactory.CreateModel(entity.Color) : null;

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

            var autoModelEntity = model.AutoModel != null ? _autoModelEMFactory.CreateEntity(model.AutoModel) : null;
            var bodyTypeEntity = model.BodyType != null ? _bodyTypeEMFactory.CreateEntity(model.BodyType) : null;
            var driveTypeEntity = model.DriveType != null ? _driveTypeEMFactory.CreateEntity(model.DriveType) : null;
            var engineEntity = model.Engine != null ? _engineEMFactory.CreateEntity(model.Engine) : null;
            var colorEntity = model.Color != null ? _colorEMFactory.CreateEntity(model.Color) : null;

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
                AutoModel = autoModelEntity,
                BodyType = bodyTypeEntity,
                DriveType = driveTypeEntity,
                Engine = engineEntity,
                Color = colorEntity,
                Cars = carEntities
            };

            return entity;
        }
    }
}
