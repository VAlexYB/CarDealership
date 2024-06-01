using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using Color = CarDealership.Core.Models.Color;

namespace CarDealership.DataAccess.Factories
{
    public class ColorEMFactory : IEntityModelFactory<Color, ColorEntity>
    {
        private readonly IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity> _autoConfigEMFactory;

        public ColorEMFactory(IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity> autoConfigEMFactory)
        {
            _autoConfigEMFactory = autoConfigEMFactory ?? throw new ArgumentNullException(nameof(autoConfigEMFactory));
        }

        public ColorEntity CreateEntity(Color model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var configurations = model.Configurations
                .Select(config => _autoConfigEMFactory.CreateEntity(config))
                .ToList();

            var entity = new ColorEntity
            {
                Id = model.Id,
                Value = model.Value,
                Price = model.Price,
                Configurations = configurations,
                IsDeleted = model.IsDeleted
            };

            return entity;
        }

        public Color CreateModel(ColorEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var colorCreateResult = Color.Create(
                entity.Id,
                entity.Value,
                entity.Price,
                entity.IsDeleted
            );

            if (colorCreateResult.IsFailure)
            {
                throw new InvalidOperationException(colorCreateResult.Error);
            }

            var color = colorCreateResult.Value;

            foreach (var configEntity in entity.Configurations)
            {
                var configModel = _autoConfigEMFactory.CreateModel(configEntity);
                color.AddConfiguration(configModel);
            }

            return color;
        }
    }
}
