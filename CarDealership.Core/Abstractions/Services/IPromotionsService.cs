using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Services
{
    public interface IPromotionsService : IGenericService<Promotion, BaseFilter>
    {
        Task<Promotion> GetByPromocode(string promocode);
        Task<Promotion> UsePromocode(Guid customerId, Guid configurationId, string promocode);
    }
}
