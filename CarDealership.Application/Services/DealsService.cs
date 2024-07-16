using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Enums;
using CarDealership.Core.Filters;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class DealsService : BaseService<Deal, DealsFilter>, IDealsService
    {
        private readonly IDealsRepository _dealsRepository;
        public DealsService(IDealsRepository repository) : base(repository)
        {
        }

        public async Task<Guid> ChangeStatus(Guid id, int status)
        {
            try
            {
                var deal = await _repository.GetByIdAsync(id);
                if (deal == null) throw new InvalidOperationException("Сделка не найдена");
                deal.ChangeStatus((DealStatus)status);
                await _repository.UpdateAsync(deal);
                return deal.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Deal>> GetDealsWithoutManager()
        {
            try
            {
                return await _dealsRepository.GetDealsWithoutManager();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Guid> LeaveDeal(Guid taskId)
        {
            try
            {
                var deal = await _repository.GetByIdAsync(taskId);
                if (deal == null) throw new InvalidOperationException("Сделка не найдена");
                deal.RemoveManager();
                deal.ChangeStatus(DealStatus.Negotiation);
                await _repository.UpdateAsync(deal);
                return deal.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Guid> TakeDealInProcess(Guid managerId, Guid taskId)
        {
            try
            {
                var deal = await _repository.GetByIdAsync(taskId);
                if (deal == null) throw new InvalidOperationException("Сделка не найдена");
                deal.SetAsManager(managerId);
                deal.ChangeStatus(DealStatus.Negotiation);
                await _repository.UpdateAsync(deal);
                return deal.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
