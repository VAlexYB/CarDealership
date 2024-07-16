using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;

namespace CarDealership.Web.Api.Factories
{
    public class EngineTypeRMFactory : IEngineTypeRMFactory
    {
        public EngineType CreateModel(EngineTypeRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var engineTypeCreateResult = EngineType.Create(req.Id, req.Value);

            if(engineTypeCreateResult.IsFailure)
            {
                throw new InvalidOperationException(engineTypeCreateResult.Error);
            }

            return engineTypeCreateResult.Value;
        }

        public EngineTypeResponse CreateResponse(EngineType model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var response = new EngineTypeResponse(model.Id)
            {
                Value = model.Value
            };

            return response;
        }
    }
}
