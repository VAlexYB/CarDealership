using Data;

namespace CarDealership.Core.Abstractions.Services
{
    public interface IEntityDataProvider
    {
        Task<DataResponse> GetDataAsync(string entityId);
    }
}
