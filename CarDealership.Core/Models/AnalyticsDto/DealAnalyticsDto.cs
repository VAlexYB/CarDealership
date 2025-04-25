namespace CarDealership.Core.Models.AnalyticsDto
{
    public class DealAnalyticsDto
    {
        public List<DealStatsDto> StatsByModel { get; set; }
        public List<DealStatsDto> StatsByConfiguration { get; set; }
    }
}
