using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;

namespace CarDealership.Web.Api.Factories
{
    public interface IModelBuilder<Req, M>
        where Req : BaseRequest
        where M : BaseModel
    {
        M CreateModel(Req req);
    }
}
