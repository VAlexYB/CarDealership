using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Enums;
using CarDealership.Core.Filters;
using CarDealership.Core.Models;
using CarDealership.Core.Models.AnalyticsDto;
using CSharpFunctionalExtensions;

namespace CarDealership.Application.Services
{
    public class DealsService : BaseService<Deal, DealsFilter>, IDealsService
    {
        IDealsRepository _dealsRepository;
        public DealsService(IDealsRepository repository) : base(repository)
        {
            _dealsRepository = repository;
        }

        public async Task<Guid> ChangeStatus(Guid id, int status)
        {
            var deal = await _dealsRepository.GetByIdAsync(id);
            if (deal == null) throw new InvalidOperationException("Сделка не найдена");
            deal.ChangeStatus((DealStatus)status);
            await _repository.UpdateAsync(deal);
            return deal.Id;
        }

        public async Task<List<Deal>> GetDealsWithoutManager()
        {
            return await _dealsRepository.GetDealsWithoutManager();
        }

        public async Task<Guid> LeaveDeal(Guid taskId)
        {
            var deal = await _dealsRepository.GetByIdAsync(taskId);
            if (deal == null) throw new InvalidOperationException("Сделка не найдена");
            deal.RemoveManager();
            deal.ChangeStatus(DealStatus.Negotiation);
            await _repository.UpdateAsync(deal);
            return deal.Id;
        }

        public async Task<Guid> TakeDealInProcess(Guid managerId, Guid taskId)
        {
            var deal = await _dealsRepository.GetByIdAsync(taskId);
            if (deal == null) throw new InvalidOperationException("Сделка не найдена");
            deal.SetAsManager(managerId);
            deal.ChangeStatus(DealStatus.Negotiation);
            await _repository.UpdateAsync(deal);
            return deal.Id;
        }

        public async Task<DealAnalyticsDto> GetAnalytics(bool byConfiguration)
        {
            DealsFilter filter = new DealsFilter();
            filter.DealStatus = DealStatus.Completed;

            var completedDeals = await _dealsRepository.GetFilteredAsync(filter);

            var result = new DealAnalyticsDto();
            if (byConfiguration)
            {
                var configsStats = completedDeals
                    .GroupBy(d => new
                    {
                        AutoModel = d.Car.AutoConfiguration.AutoModel.Name,
                        BodyType = d.Car.AutoConfiguration.BodyType.Value,
                        Color = d.Car.AutoConfiguration.Color.Value,
                        DriveType = d.Car.AutoConfiguration.DriveType.Value,
                        Equipment = d.Car.AutoConfiguration.Equipment.Name
                    })
                    .Select(g => new
                    {
                        g.Key.AutoModel,
                        g.Key.BodyType,
                        g.Key.Color,
                        g.Key.DriveType,
                        g.Key.Equipment,
                        Count = g.Count(),
                        TotalAmount = g.Sum(d => d.Price)
                    })
                    .ToList();

                result.StatsByConfiguration = configsStats.Select(item => new DealStatsDto
                {
                    Name = $"{item.AutoModel} - {item.BodyType} - {item.Color} - {item.DriveType} - {item.Equipment}",
                    Count = item.Count,
                    TotalAmount = item.TotalAmount
                }).ToList();
            }
            else
            {
                var modelsStats = completedDeals
                    .GroupBy(d => d.Car.AutoConfiguration.AutoModel.Name)
                    .Select(g => new
                    {
                        g.Key,
                        Count = g.Count(),
                        TotalAmount = g.Sum(d => d.Price)
                    })
                    .ToList();

                result.StatsByModel = modelsStats.Select(item => new DealStatsDto
                {
                    Name = item.Key,
                    Count = item.Count,
                    TotalAmount = item.TotalAmount
                }).ToList();
            }

            return result;
        }
    }
}
