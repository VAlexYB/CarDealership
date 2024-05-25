using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;

namespace CarDealership.Web.Api.Factories
{
    public interface IModelBuilderAsync <Req, M>
        where Req : BaseRequest
        where M : BaseModel
    {
        Task<M> CreateModelAsync(Req req);
    }
}
