using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;

namespace CarDealership.Web.Api.Factories
{
    public class CountryRMFactory : IModelBuilder<CountryRequest, Country>, IResponseBuilder<CountryResponse, Country>
    {
        public Country CreateModel(CountryRequest req)
        {
            if(req == null) throw new ArgumentNullException(nameof(req));

            var countryCreateResult = Country.Create(req.Id, req.Name, req.IsDeleted);
            if(countryCreateResult.IsFailure)
            {
                throw new InvalidOperationException(countryCreateResult.Error);
            }

            return countryCreateResult.Value;
        }

        public CountryResponse CreateResponse(Country model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            var countryResponse = new CountryResponse(model.Id)
            {
                Name = model.Name
            };
            return countryResponse;
        }
    }
}
