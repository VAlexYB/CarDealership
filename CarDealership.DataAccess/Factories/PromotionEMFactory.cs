using CarDealership.Core.Models;
using CarDealership.Core.Models.Auth;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Entities.Auth;
using CSharpFunctionalExtensions;

namespace CarDealership.DataAccess.Factories
{
    public class PromotionEMFactory : IEntityModelFactory<Promotion, PromotionEntity>
    {
        private readonly IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity> _autoConfigEMFactory;

        public PromotionEMFactory(IEntityModelFactory<AutoConfiguration, AutoConfigurationEntity> autoConfigEMFactory)
        {
            _autoConfigEMFactory = autoConfigEMFactory;
        }

        public PromotionEntity CreateEntity(Promotion model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var configurations = model.AppliableConfigs.Select(config => _autoConfigEMFactory.CreateEntity(config)).ToList();

            var users = model.Participants.Select(user =>
            {
                return new UserEntity
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber
                };
            }).ToList();

            PromotionEntity entity = new PromotionEntity
            {
                Id = model.Id,
                Promocode = model.Promocode,
                OrderDiscountPercent = model.OrderDiscountPercent,
                DealDiscountPercent = model.DealDiscountPercent,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                AppliableConfigs = configurations,
                Participants = users
            };
            return entity;
        }

        public Promotion CreateModel(PromotionEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var modelCreateResult = Promotion.Create(
                entity.Id,
                entity.Promocode,
                entity.StartDate,
                entity.EndDate,
                entity.OrderDiscountPercent,
                entity.DealDiscountPercent,
                entity.IsDeleted
            );

            if (modelCreateResult.IsFailure)
            {
                throw new InvalidOperationException(modelCreateResult.Error);
            }

            Promotion model = modelCreateResult.Value;

            foreach (var config in entity.AppliableConfigs)
            {
                model.AddConfiguration(_autoConfigEMFactory.CreateModel(config));
            }

            User tempUser;
            foreach(var user in entity.Participants)
            {
                tempUser = User.Create(
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.PasswordHash,
                    user.FirstName,
                    user.MiddleName,
                    user.LastName,
                    user.PhoneNumber
                ).Value;

                model.AddTakedUser(tempUser);
            }

            return model;
        }
    }
}
