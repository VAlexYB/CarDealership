using CarDealership.Core.Exceptions;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;

namespace CarDealership.Web.Api.Factories
{
    public class BodyTypeRMFactory : IBodyTypeRMFactory
    {
        public Task<BodyType> CreateModel(BodyTypeRequest req)
        {
            if(req == null) throw new ArgumentNullException(nameof(req));

            var bodyTypeCreateResult = BodyType.Create(req.Id, req.Value, req.Price);

            if(bodyTypeCreateResult.IsFailure)
            {
                throw new ClientInformationException(bodyTypeCreateResult.Error);
            }

            return Task.FromResult(bodyTypeCreateResult.Value);
        }

        public Task<BodyTypeResponse> CreateResponse(BodyType model)
        {
            if(model == null) throw new ArgumentNullException(nameof(model));

            var response = new BodyTypeResponse(model.Id)
            {
                Value = model.Value,
                Price = model.Price
            };
            return Task.FromResult(response);
        }
    }
}
