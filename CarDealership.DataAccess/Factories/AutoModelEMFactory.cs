using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class AutoModelEMFactory : IEntityModelFactory<AutoModel, AutoModelEntity>
    {
        private readonly IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity> _autoConfigEMFactory;
        private readonly IEntityModelFactory<Equipment, EquipmentEntity> _equipmentEMFactory;

        public AutoModelEMFactory(
            IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity> autoConfigEMFactory,
            IEntityModelFactory<Equipment, EquipmentEntity> equipmentEMFactory)
        {
            _autoConfigEMFactory = autoConfigEMFactory ?? throw new ArgumentNullException(nameof(autoConfigEMFactory));
            _equipmentEMFactory = equipmentEMFactory ?? throw new ArgumentNullException(nameof(equipmentEMFactory));
        }

        public AutoModel CreateModel(AutoModelEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var modelCountryEntity = entity.Brand?.Country;
            var modelCountry = modelCountryEntity != null ? Country.Create(
                modelCountryEntity.Id,
                modelCountryEntity.Name,
                modelCountryEntity.IsDeleted
            ).Value : null;

            var brand = entity.Brand != null ? Brand.Create(
                entity.BrandId,
                entity.Brand.Name,
                entity.Brand.CountryId,
                entity.Brand.IsDeleted,
                modelCountry
            ).Value : null;
            
            var autoModelResult = AutoModel.Create(
                entity.Id,
                entity.Name,
                entity.Price,
                entity.BrandId,
                entity.IsDeleted,
                brand
            );

            if (autoModelResult.IsFailure)
            {
                throw new InvalidOperationException(autoModelResult.Error);
            }

            var autoModel = autoModelResult.Value;

            foreach (var config in entity.Configurations)
            {
                autoModel.AddConfiguration(_autoConfigEMFactory.CreateModel(config));
            }

            foreach (var equipment in entity.Equipments)
            {
                autoModel.AddEquipment(_equipmentEMFactory.CreateModel(equipment));
            }

            return autoModel;
        }

        public AutoModelEntity CreateEntity(AutoModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var configurations = model.Configurations.Select(config => _autoConfigEMFactory.CreateEntity(config)).ToList();
            var equipments = model.Equipments.Select(equipment => _equipmentEMFactory.CreateEntity(equipment)).ToList();

            var entity = new AutoModelEntity
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                BrandId = model.BrandId,
                Configurations = configurations,
                Equipments = equipments
            };

            return entity;
        }

    }
}
