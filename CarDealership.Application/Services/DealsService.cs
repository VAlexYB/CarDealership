using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Enums;
using CarDealership.Core.Filters;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class DealsService : BaseService<Deal, DealsFilter>, IDealsService
    {
        public DealsService(IDealsRepository repository) : base(repository)
        {
        }

        public async Task<Guid> ChangeStatus(Guid id, int status)
        {
            var deal = await _repository.GetByIdAsync(id);
            if (deal == null) throw new InvalidOperationException("Сделка не найдена");
            deal.ChangeStatus((DealStatus)status);
            await _repository.UpdateAsync(deal);
            return deal.Id;
        }
    }
}
