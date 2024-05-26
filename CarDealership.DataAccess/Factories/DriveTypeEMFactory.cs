using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using DriveType = CarDealership.Core.Models.DriveType;

namespace CarDealership.DataAccess.Factories
{
    public class DriveTypeEMFactory : IEntityModelFactory<DriveType, DriveTypeEntity>
    {
        private readonly IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity> _autoConfigEMFactory;

        public DriveTypeEMFactory(IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity> autoConfigEMFactory)
        {
            _autoConfigEMFactory = autoConfigEMFactory ?? throw new ArgumentNullException(nameof(autoConfigEMFactory));
        }

        public DriveTypeEntity CreateEntity(DriveType model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var configurationEntities = model.Configurations
                .Select(configuration => _autoConfigEMFactory.CreateEntity(configuration))
                .ToList();

            var entity = new DriveTypeEntity
            {
                Id = model.Id,
                Value = model.Value,
                Price = model.Price,
                IsDeleted = model.IsDeleted,
                Configurations = configurationEntities
            };

            return entity;
        }

        public DriveType CreateModel(DriveTypeEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var driveTypeCreateResult = DriveType.Create(
                entity.Id,
                entity.Value,
                entity.Price,
                entity.IsDeleted
            );

            if (driveTypeCreateResult.IsFailure)
            {
                throw new InvalidOperationException(driveTypeCreateResult.Error);
            }

            var driveType = driveTypeCreateResult.Value;

            foreach (var configurationEntity in entity.Configurations)
            {
                var configurationModel = _autoConfigEMFactory.CreateModel(configurationEntity);
                driveType.AddConfiguration(configurationModel);
            }

            return driveType;
        }
    }
}
