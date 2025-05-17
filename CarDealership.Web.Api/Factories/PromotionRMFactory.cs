using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Exceptions;
using CarDealership.Core.Filters;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;

namespace CarDealership.Web.Api.Factories
{
    public class PromotionRMFactory : IPromotionRMFactory
    {
        private readonly IAutoConfigsService _autoConfigService;

        public PromotionRMFactory(IAutoConfigsService autoConfigService)
        {
            _autoConfigService = autoConfigService;
        }

        public async Task<Promotion> CreateModel(PromotionRequest req)
        {
            ConfigurationsFilter filter = new ConfigurationsFilter();
            filter.Guids = req.ConfigIds.ToHashSet();
            List<AutoConfiguration> appliableConfigs = 
                await _autoConfigService.GetFilteredAsync(filter);

            var modelCreateResult = Promotion.Create(
                req.Id, 
                req.Promocode, 
                req.StartDate, 
                req.EndDate, 
                req.OrderDiscountPercent, 
                req.DealDiscountPercent
            );

            if (modelCreateResult.IsFailure)
            {
                throw new ClientInformationException(modelCreateResult.Error);
            }

            Promotion promotion = modelCreateResult.Value;

            foreach (var appliableConfig in appliableConfigs)
            {
                promotion.AddConfiguration(appliableConfig);
            }

            return promotion;
        }

        public Task<PromotionResponse> CreateResponse(Promotion model)
        {
            return Task.FromResult(new PromotionResponse(model.Id)
            {
                Promocode = model.Promocode,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                OrderDiscountPercent = model.OrderDiscountPercent,
                DealDiscountPercent = model.DealDiscountPercent,
                Configurations = Infrastructure.Jobs.PromotionDistributeJob.GetConfigurationDescription(model.AppliableConfigs)
            });
        }   
    }
}
