using CarDealership.Application.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;

namespace CarDealership.Web.Api.Factories
{
    public class BrandRMFactory : IModelBuilderAsync<BrandRequest, Brand>, IResponseBuilder<BrandResponse, Brand>
    {
        private readonly CountriesService _countriesService;

        public BrandRMFactory(CountriesService countriesService)
        {
            _countriesService = countriesService ?? throw new ArgumentNullException(nameof(countriesService));
        }
        public async Task<Brand> CreateModelAsync(BrandRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof (req));

            var brandCountry = await _countriesService.GetByIdAsync(req.CountryId);
            var brandCreateResult = Brand.Create(req.Id, req.Name, req.CountryId, req.IsDeleted, brandCountry);

            if(brandCreateResult.IsFailure)
            {
                throw new InvalidOperationException(brandCreateResult.Error);
            }

            var brand = brandCreateResult.Value;
            brandCountry.AddBrand(brand);
            return brand;
        }

        public BrandResponse CreateResponse(Brand model)
        {
            var response = new BrandResponse(model.Id)
            {
                Name = model.Name
            };
            return response;
        }
    }
}
