using CarDealership.Core.Models;
using CarDealership.Core.Models.Auth;
using CarDealership.DataAccess.Entities;
using DriveType = CarDealership.Core.Models.DriveType;

namespace CarDealership.DataAccess.Factories
{
    public class OrderEMFactory : IEntityModelFactory<Order, OrderEntity>
    {
        private readonly IEntityModelFactory<EquipmentFeature, EquipmentFeatureEntity> _equipmentFeatureEMFactory;

        public OrderEMFactory(IEntityModelFactory<EquipmentFeature, EquipmentFeatureEntity> equipmentFeatureEMFactory)
        {
            _equipmentFeatureEMFactory = equipmentFeatureEMFactory;
        }

        public OrderEntity CreateEntity(Order model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var order = new OrderEntity
            {
                Id = model.Id,
                OrderDate = model.OrderDate,
                CompleteDate = model.CompleteDate,
                Status = model.Status,
                AutoConfigurationId = model.AutoConfigurationId,
                CustomerId = model.CustomerId,
                ManagerId = model.ManagerId,
                Price = model.Price
            };

            return order;
        }

        public Order CreateModel(OrderEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));


            var configEntity = entity.AutoConfiguration;

            var carBrandEntity = configEntity?.AutoModel?.Brand;
            var carEngineTypeEntity = configEntity?.Engine?.EngineType;
            var carTransmissionTypeEntity = configEntity?.Engine?.TransmissionType;

            var carBrand = carBrandEntity != null ? Brand.Create(
               carBrandEntity.Id,
               carBrandEntity.Name,
               carBrandEntity.CountryId,
               carBrandEntity.IsDeleted
           ).Value : null;

            var autoModel = configEntity?.AutoModel != null ? AutoModel.Create(
                    configEntity.AutoModelId,
                    configEntity.AutoModel.Name,
                    configEntity.AutoModel.Price,
                    configEntity.AutoModel.BrandId,
                    configEntity.AutoModel.IsDeleted,
                    carBrand
            ).Value : null;

            var bodyType = configEntity?.BodyType != null ? BodyType.Create(
                    configEntity.BodyTypeId,
                    configEntity.BodyType.Value,
                    configEntity.BodyType.Price,
                    configEntity.BodyType.IsDeleted
            ).Value : null;

            var driveType = configEntity?.DriveType != null ? DriveType.Create(
                configEntity.DriveTypeId,
                configEntity.DriveType.Value,
                configEntity.DriveType.Price,
                configEntity.DriveType.IsDeleted
            ).Value : null;


            var carEngineType = carEngineTypeEntity != null ? EngineType.Create(
               carEngineTypeEntity.Id,
               carEngineTypeEntity.Value,
               carEngineTypeEntity.IsDeleted
           ).Value : null;

            var carTransmissionType = carTransmissionTypeEntity != null ? TransmissionType.Create(
                carTransmissionTypeEntity.Id,
                carTransmissionTypeEntity.Value,
                carTransmissionTypeEntity.IsDeleted
            ).Value : null;

            var engine = configEntity?.Engine != null ? Engine.Create(
                configEntity.EngineId,
                configEntity.Engine.Power,
                configEntity.Engine.Consumption,
                configEntity.Engine.Price,
                configEntity.Engine.EngineTypeId,
                configEntity.Engine.TransmissionTypeId,
                configEntity.Engine.IsDeleted,
                carEngineType,
                carTransmissionType
            ).Value : null;

            var color = configEntity?.Color != null ? Color.Create(
                configEntity.ColorId,
                configEntity.Color.Value,
                configEntity.Color.Price,
                configEntity.Color.IsDeleted
            ).Value : null;

            var carEquipment = configEntity?.Equipment != null ? Equipment.Create(
                 configEntity.EquipmentId,
                 configEntity.Equipment.Name,
                 configEntity.Equipment.Price,
                 configEntity.Equipment.ReleaseYear,
                 configEntity.Equipment.AutoModelId,
                 configEntity.Equipment.IsDeleted,
                 autoModel
             ).Value : null;

            if (carEquipment != null)
            {
                foreach (var featureEntity in configEntity.Equipment.equipmentFeatures)
                {
                    var featureModel = _equipmentFeatureEMFactory.CreateModel(featureEntity);
                    carEquipment.AddEquipmentFeature(featureModel);
                }
            }

            var config = configEntity != null ? AutoConfiguration.Create(
                configEntity.Id,
                configEntity.Price,
                configEntity.AutoModelId,
                configEntity.BodyTypeId,
                configEntity.DriveTypeId,
                configEntity.EngineId,
                configEntity.ColorId,
                configEntity.EquipmentId,
                configEntity.IsDeleted,
                autoModel,
                bodyType,
                driveType,
                engine,
                color,
                carEquipment
            ).Value : null;

            var manager = entity.Manager != null ? User.Create(
                entity.Manager.Id,
                entity.Manager.UserName,
                entity.Manager.Email,
                entity.Manager.PasswordHash,
                entity.Manager.FirstName,
                entity.Manager.MiddleName,
                entity.Manager.LastName,
                entity.Manager.PhoneNumber
            ).Value : null;

            var customer = entity.Customer != null ? User.Create(
                entity.Customer.Id,
                entity.Customer.UserName,
                entity.Customer.Email,
                entity.Customer.PasswordHash,
                entity.Customer.FirstName,
                entity.Customer.MiddleName,
                entity.Customer.LastName,
                entity.Customer.PhoneNumber
            ).Value : null;

            var orderCreateResult = Order.Create(
                entity.Id,
                entity.OrderDate,
                entity.CompleteDate,
                entity.Status,
                entity.Price,
                entity.AutoConfigurationId,
                entity.ManagerId,
                entity.CustomerId,
                entity.IsDeleted,
                config,
                manager,
                customer
            );

            if (orderCreateResult.IsFailure)
            {
                throw new InvalidOperationException(orderCreateResult.Error);
            }

            var order = orderCreateResult.Value;
            return order;
        }
    }
}
