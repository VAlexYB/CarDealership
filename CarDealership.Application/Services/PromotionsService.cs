using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Exceptions;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class PromotionsService : BaseService<Promotion, BaseFilter>, IPromotionsService
    {
        IPromotionsRepository _promotionsRepository;
        IAutoConfigsRepository _autoConfigsRepository;
        IUsersRepository _usersRepository;
        public PromotionsService(IPromotionsRepository repository, IAutoConfigsRepository configsRepository, IUsersRepository usersRepository) : base(repository)
        {
            _promotionsRepository = repository;
            _autoConfigsRepository = configsRepository;
            _usersRepository = usersRepository;
        }

        public async Task<Promotion> GetByPromocode(string promocode)
        {
            Promotion promotion = await _promotionsRepository.GetByPromocode(promocode);
            DateTime now = DateTime.UtcNow;
            if (now < promotion.StartDate || now > promotion.EndDate)
            {
                throw new ClientInformationException("Промокод недействителен");
            }
            return promotion;
        }

        public async Task<Promotion> UsePromocode(Guid customerId, Guid configurationId, string promocode)
        {
            Promotion promo = await _promotionsRepository.GetByPromocode(promocode);

            if (promo.Participants.Any(participant => participant.Id == customerId))
            {
                throw new ClientInformationException("Промокод разрешено использовать только 1 раз");
            }

            if (!promo.AppliableConfigs.Any(config => config.Id == configurationId)) 
            {
                throw new ClientInformationException("Промокод не применим к этому автомобилю");
            }
            promo.AddTakedUser(await _usersRepository.GetByIdAsync(customerId));
            await _promotionsRepository.UpdateAsync(promo);
            return promo;
        }
    }
}
