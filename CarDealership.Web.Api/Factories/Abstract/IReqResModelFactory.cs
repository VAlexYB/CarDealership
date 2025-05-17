using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;

namespace CarDealership.Web.Api.Factories.Abstract
{
    public interface IReqResModelFactory<Req, Res, M>
       where Req : BaseRequest
       where Res : BaseResponse
       where M : BaseModel
    {
        Task<M> CreateModel(Req req);
        Task<Res> CreateResponse(M model);
    }
}
