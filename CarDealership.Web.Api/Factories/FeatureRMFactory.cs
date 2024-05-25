using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;

namespace CarDealership.Web.Api.Factories
{
    public class FeatureRMFactory : IModelBuilder<FeatureRequest, Feature>, IResponseBuilder<FeatureResponse, Feature>
    {
        public Feature CreateModel(FeatureRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var featureCreateResult = Feature.Create(req.Id, req.Description, req.IsDeleted);

            if(featureCreateResult.IsFailure)
            {
                throw new ArgumentNullException(featureCreateResult.Error);
            }

            return featureCreateResult.Value;
        }

        public FeatureResponse CreateResponse(Feature model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var response = new FeatureResponse(model.Id)
            {
                Description = model.Description
            };

            return response;
        }
    }
}
