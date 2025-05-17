using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Repositories
{
    public interface IPromotionsRepository : IGenericRepository<Promotion, BaseFilter>
    {
        Task<Promotion> GetByPromocode(string promocode);
    }
}
