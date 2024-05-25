using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;

namespace CarDealership.DataAccess.Factories
{
    public class AutoModelEMFactory : IEntityModelFactory<AutoModel, AutoModelEntity>
    {
        private readonly BrandEMFactory _brandEMFactory;
        private readonly AutoConfigEMFactory _autoConfigEMFactory;
        private readonly EquipmentEMFactory _equipmentEMFactory;

        public AutoModelEMFactory(BrandEMFactory brandEMFactory, AutoConfigEMFactory autoConfigEMFactory, EquipmentEMFactory equipmentEMFactory)
        {
            _brandEMFactory = brandEMFactory ?? throw new ArgumentNullException(nameof(brandEMFactory));
            _autoConfigEMFactory = autoConfigEMFactory ?? throw new ArgumentNullException(nameof(autoConfigEMFactory));
            _equipmentEMFactory = equipmentEMFactory ?? throw new ArgumentNullException(nameof(equipmentEMFactory));
        }

        public AutoModel CreateModel(AutoModelEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var brand = entity.Brand != null ? _brandEMFactory.CreateModel(entity.Brand) : null;

            var autoModelResult = AutoModel.Create(
                entity.Id,
                entity.Name,
                entity.Price,
                entity.BrandId,
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

            var brandEntity = model.Brand != null ? _brandEMFactory.CreateEntity(model.Brand) : null;

            var configurations = model.Configurations.Select(config => _autoConfigEMFactory.CreateEntity(config)).ToList();
            var equipments = model.Equipments.Select(equipment => _equipmentEMFactory.CreateEntity(equipment)).ToList();

            var entity = new AutoModelEntity
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                BrandId = model.BrandId,
                Brand = brandEntity,
                Configurations = configurations,
                Equipments = equipments
            };

            return entity;
        }

    }
}
