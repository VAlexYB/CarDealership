using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Infrastructure.Messaging;
using CarDealership.Shared.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Text;

namespace CarDealership.Infrastructure.Jobs
{
    public class PromotionDistributeJob : IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IRabbitMQMessageSender _messageSender;
        private readonly IConfiguration _configuration;

        public PromotionDistributeJob(
            IServiceScopeFactory serviceScopeFactory, 
            IRabbitMQMessageSender messageSender,
            IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _messageSender = messageSender;
            _configuration = configuration;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            IPromotionsService promotionsService = scope.ServiceProvider.GetRequiredService<IPromotionsService>();
            List<Promotion> rawPromos = await promotionsService.GetAllAsync();

            foreach (var promo in rawPromos)
            {
                PromotionInfo promoInfo = new PromotionInfo
                {
                    Promocode = promo.Promocode,
                    StartDate = promo.StartDate,
                    EndDate = promo.EndDate,
                    OrderDiscountPercent = promo.OrderDiscountPercent,
                    DealDiscountPercent = promo.DealDiscountPercent,
                    Configurations = GetConfigurationDescription(promo.AppliableConfigs)
                };
                _messageSender.SendMessage(promoInfo, _configuration["RabbitMQ:Queues:CDQueue"]);
            }
        }

        public static string GetConfigurationDescription(IReadOnlyCollection<AutoConfiguration> configs)
        {
            var groupedConfigs = configs
                .GroupBy(c => new { Brand = c.AutoModel?.Brand?.Name, AutoModel = c.AutoModel?.Name, ReleaseYear = c?.Equipment?.ReleaseYear })
                .OrderBy(g => g.Key.Brand)
                .ThenBy(g => g.Key.AutoModel)
                .ThenBy(g => g.Key.ReleaseYear);

            var descriptionBuilder = new StringBuilder();

            foreach (var group in groupedConfigs)
            {
                descriptionBuilder.AppendLine($"{group.Key.Brand} {group.Key.AutoModel} {group.Key.ReleaseYear} год:");

                var uniqueDescriptions = new HashSet<string>();

                foreach (var config in group)
                {
                    string description = $"- {config.Equipment?.Name} ({config.BodyType?.Value}, {config.Engine?.Power} л.с.)";
                    if (uniqueDescriptions.Add(description))
                    {
                        descriptionBuilder.AppendLine(description);
                    }
                }

                descriptionBuilder.AppendLine();
            }

            return descriptionBuilder.ToString().Trim();
        }
    }
}
