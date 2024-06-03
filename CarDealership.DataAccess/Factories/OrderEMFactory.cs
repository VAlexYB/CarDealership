using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using DriveType = CarDealership.Core.Models.DriveType;

namespace CarDealership.DataAccess.Factories
{
    public class OrderEMFactory : IEntityModelFactory<Order, OrderEntity>
    {
        public OrderEntity CreateEntity(Order model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var order = new OrderEntity
            {
                Id = model.Id,
                OrderDate = model.OrderDate,
                CompleteDate = model.CompleteDate,
                Status = model.Status,
                CarId = model.CarId,
                CustomerId = model.CustomerId,
                ManagerId = model.ManagerId,
            };

            return order;
        }

        public Order CreateModel(OrderEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));


            var carConfigEntity = entity.Car?.AutoConfiguration;

            var carBrandEntity = carConfigEntity?.AutoModel?.Brand;
            var carEngineTypeEntity = carConfigEntity?.Engine?.EngineType;
            var carTransmissionTypeEntity = carConfigEntity?.Engine?.TransmissionType;

            var carBrand = carBrandEntity != null ? Brand.Create(
               carBrandEntity.Id,
               carBrandEntity.Name,
               carBrandEntity.CountryId,
               carBrandEntity.IsDeleted
           ).Value : null;

            var autoModel = carConfigEntity?.AutoModel != null ? AutoModel.Create(
                    carConfigEntity.AutoModelId,
                    carConfigEntity.AutoModel.Name,
                    carConfigEntity.AutoModel.Price,
                    carConfigEntity.AutoModel.BrandId,
                    carConfigEntity.AutoModel.IsDeleted,
                    carBrand
            ).Value : null;

            var bodyType = carConfigEntity?.BodyType != null ? BodyType.Create(
                    carConfigEntity.BodyTypeId,
                    carConfigEntity.BodyType.Value,
                    carConfigEntity.BodyType.Price,
                    carConfigEntity.BodyType.IsDeleted
            ).Value : null;

            var driveType = carConfigEntity?.DriveType != null ? DriveType.Create(
                carConfigEntity.DriveTypeId,
                carConfigEntity.DriveType.Value,
                carConfigEntity.DriveType.Price,
                carConfigEntity.DriveType.IsDeleted
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

            var engine = carConfigEntity?.Engine != null ? Engine.Create(
                carConfigEntity.EngineId,
                carConfigEntity.Engine.Power,
                carConfigEntity.Engine.Consumption,
                carConfigEntity.Engine.Price,
                carConfigEntity.Engine.EngineTypeId,
                carConfigEntity.Engine.TransmissionTypeId,
                carConfigEntity.Engine.IsDeleted,
                carEngineType,
                carTransmissionType
            ).Value : null;

            var color = carConfigEntity?.Color != null ? Color.Create(
                carConfigEntity.ColorId,
                carConfigEntity.Color.Value,
                carConfigEntity.Color.Price,
                carConfigEntity.Color.IsDeleted
            ).Value : null;

            var carConfig = carConfigEntity != null ? AutoConfiguration.Create(
                carConfigEntity.Id,
                carConfigEntity.Price,
                carConfigEntity.AutoModelId,
                carConfigEntity.BodyTypeId,
                carConfigEntity.DriveTypeId,
                carConfigEntity.EngineId,
                carConfigEntity.ColorId,
                carConfigEntity.IsDeleted,
                autoModel,
                bodyType,
                driveType,
                engine,
                color
            ).Value : null;

            var car = entity.Car != null ? Car.Create(
                entity.Car.Id,
                entity.Car.VIN,
                entity.Car.AutoConfigurationId,
                entity.Car.IsDeleted,
                carConfig
            ).Value : null;

            var orderCreateResult = Order.Create(
                entity.Id,
                entity.OrderDate,
                entity.CompleteDate,
                entity.Status,
                entity.CarId,
                entity.ManagerId,
                entity.CustomerId,
                entity.IsDeleted,
                car
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
