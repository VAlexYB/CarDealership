using CarDealership.Core.Abstractions.Services;
using CarDealership.Shared.Enums;
using Data;
using Grpc.Core;

namespace CarDealership.Web.Api.Endpoints.Grpc
{
    public class DataGrpcService : DataService.DataServiceBase
    {
        private readonly Dictionary<string, IEntityDataProvider> _dataProviders;
        public DataGrpcService(IEnumerable<IEntityDataProvider> dataProviders)
        {
            _dataProviders = dataProviders
                .ToDictionary(p => p.GetType().Name.Replace("DataProvider", "").ToLower(), p => p);
        }
        public override async Task<DataResponse> GetData(DataRequest request, ServerCallContext context)
        {
            string key = $"{request.EntityName}{(DocumentType)request.DocType}".ToLower();
            if (_dataProviders.TryGetValue(key, out var provider))
            {
                return await provider.GetDataAsync(request.EntityId);
            }

            throw new RpcException(new Status(StatusCode.NotFound, $"Не найден обработчик для получения данных." +
                $" Cущность: {request.EntityName}. Тип документа: {request.DocType}"));
        }
    }
}
