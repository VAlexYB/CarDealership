using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;

namespace CarDealership.Web.Api.Factories
{
    public class BodyTypeRMFactory : IResponseBuilder<BodyTypeResponse, BodyType>, IModelBuilder<BodyTypeRequest, BodyType>
    {
        public BodyType CreateModel(BodyTypeRequest req)
        {
            if(req == null) throw new ArgumentNullException(nameof(req));

            var bodyTypeCreateResult = BodyType.Create(req.Id, req.Value, req.Price, req.IsDeleted);

            if(bodyTypeCreateResult.IsFailure)
            {
                throw new InvalidOperationException(bodyTypeCreateResult.Error);
            }

            return bodyTypeCreateResult.Value;
        }

        public BodyTypeResponse CreateResponse(BodyType model)
        {
            if(model == null) throw new ArgumentNullException(nameof(model));

            var response = new BodyTypeResponse(model.Id)
            {
                Value = model.Value,
                Price = model.Price
            };
            return response;
        }
    }
}
