using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;
using CSharpFunctionalExtensions;

namespace CarDealership.Web.Api.Factories
{
    public class AutoModelRMFactory : IAutoModelRMFactory
    {
        private readonly IBrandsService _brandsService;

        public AutoModelRMFactory(
            IBrandsService brandsService
        )
        {
            _brandsService = brandsService ?? throw new ArgumentNullException(nameof(brandsService));
        }

        public async Task<AutoModel> CreateModelAsync(AutoModelRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var modelBrand = await _brandsService.GetByIdAsync(req.BrandId);

            var autoModelCreateResult = AutoModel.Create(
                req.Id,
                req.Name,
                req.Price,
                req.BrandId,
                false,
                modelBrand
            );

            if(autoModelCreateResult.IsFailure)
            {
                throw new InvalidOperationException(autoModelCreateResult.Error);
            }

            var autoModel = autoModelCreateResult.Value;

            modelBrand.AddModel(autoModel);

            return autoModel;
        }

        public AutoModelResponse CreateResponse(AutoModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var response = new AutoModelResponse(model.Id)
            {
                Name = model.Name,
                BrandId = model.BrandId,
                BrandName = model.Brand?.Name ?? string.Empty,
                Country = model.Brand?.Country?.Name ?? string.Empty,
            };

            return response;
        }
    }
}
