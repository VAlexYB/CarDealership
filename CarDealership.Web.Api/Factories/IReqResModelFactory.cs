using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;

namespace CarDealership.Web.Api.Factories
{
    public interface IReqResModelFactory <Req, Res, M>
        where Req: BaseRequest
        where Res: BaseResponse
        where M: BaseModel
    {
        M CreateModel (Req req);
        Res CreateResponse(M model);
    }
}
