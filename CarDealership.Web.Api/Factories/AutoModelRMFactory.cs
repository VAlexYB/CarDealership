﻿using CarDealership.Application.Services;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Factories;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CSharpFunctionalExtensions;

namespace CarDealership.Web.Api.Factories
{
    public class AutoModelRMFactory : IModelBuilderAsync<AutoModelRequest, AutoModel>, IResponseBuilder<AutoModelResponse, AutoModel>
    {
        private readonly BrandsService _brandsService;

        public AutoModelRMFactory(BrandsService brandsService, BrandRMFactory brandRMFactory, AutoConfigRMFactory autoConfigRMFactory, EquipmentRMFactory equipmentRMFactory)
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
                req.IsDeleted,
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
