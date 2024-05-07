using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;

namespace CarDealership.Web.Api.Factories
{
    public class AutoModelRMFactory : IReqResModelFactory<AutoModelRequest, AutoModelResponse, AutoModel>
    {
        public AutoModel CreateModel(AutoModelRequest req)
        {
            return AutoModel.Create(req.Id, req.Brand, req.Name, req.BodyType, req.Price).model;
        }

        public AutoModelResponse CreateResponse(AutoModel model)
        {
            return new AutoModelResponse(model);
        }
    }
}
