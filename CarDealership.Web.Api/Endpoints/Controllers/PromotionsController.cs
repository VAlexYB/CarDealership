using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Infrastructure.Messaging;
using CarDealership.Shared.Messaging;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Web.Api.Endpoints.Controllers
{
    public class PromotionsController : BaseController<Promotion, BaseFilter, PromotionRequest, PromotionResponse>
    {
        private readonly IPromotionsService promotionsService;
        private readonly IRabbitMQMessageSender _messageSender;
        private readonly IConfiguration _configuration;
        public PromotionsController(
            IPromotionsService service,
            IPromotionRMFactory factory,
            ILogger<PromotionsController> logger,
            IRabbitMQMessageSender messageSender,
            IConfiguration configuration
        ) : base(service, factory, logger)
        {
            promotionsService = service;
            _messageSender = messageSender;
            _configuration = configuration;
        }

        public override async Task<IActionResult> CreateOrEditAsync([FromBody] PromotionRequest req)
        {
            IActionResult actionResult = await base.CreateOrEditAsync(req);
            if (!(actionResult is OkObjectResult))
            {
                return actionResult;
            }

            Promotion promotion = await promotionsService.GetByPromocode(req.Promocode);
            PromotionResponse promotionInfo = await _factory.CreateResponse(promotion);
            _messageSender.SendMessage(promotionInfo, _configuration["RabbitMQ:Queues:CDQueue"]);

            return actionResult;
        }

        [HttpPost]
        [Route("use")]
        public async Task<IActionResult> UsePromocode([FromBody] UsePromocodeRequest request)
        {
            Promotion promotion = await promotionsService.UsePromocode(request.CustomerId, request.AutoConfigurationId, request.Promocode);
            PromotionResponse promotionInfo = await _factory.CreateResponse(promotion);
            return Ok(promotionInfo);
        }
    }
}
