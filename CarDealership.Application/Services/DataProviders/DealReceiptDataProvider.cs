using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using Data;
using Grpc.Core;

namespace CarDealership.Application.Services.DataProviders
{
    public class DealReceiptDataProvider : IEntityDataProvider
    {
        private readonly IDealsRepository _dealsRepository;

        public DealReceiptDataProvider(IDealsRepository dealsRepository)
        {
            _dealsRepository = dealsRepository;
        }

        public async Task<DataResponse> GetDataAsync(string entityId)
        {
            var deal = await _dealsRepository.GetByIdAsync(Guid.Parse(entityId));

            if (deal == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Сделка не найдена"));
            }

            return new DataResponse
            {
                DealReceiptResponse = new DealReceiptResponse
                {
                    DealDate = deal.DealDate.ToString("yyyy-MM-dd"),
                    Price = deal.Price.ToString("F2"),
                    Customer = deal.Customer.FullName,
                    Vin = deal.Car.VIN,
                    Model = deal?.Car?.AutoConfiguration?.AutoModel?.Name ?? "Тестовая модель",
                    Brand = deal?.Car?.AutoConfiguration?.AutoModel?.Brand?.Name ?? "Тестовый брэнд",
                    Country = deal?.Car?.AutoConfiguration?.AutoModel?.Brand?.Country?.Name ?? "Тестовая страна",
                    Body = deal?.Car?.AutoConfiguration?.BodyType?.Value ?? "Тестовый кузов",
                    Drive = deal?.Car?.AutoConfiguration?.DriveType?.Value ?? "Тестовый привод",
                    Color = deal?.Car?.AutoConfiguration?.Color?.Value ?? "Тестовый цвет",
                    Manager = deal?.Manager.FullName ?? "Тестовый менеджер"
                }
            };
        }
    }
}
