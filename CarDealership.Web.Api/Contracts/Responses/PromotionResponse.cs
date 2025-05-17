namespace CarDealership.Web.Api.Contracts.Responses
{
    public class PromotionResponse : BaseResponse
    {
        public string Promocode { get; set; }
        public decimal? OrderDiscountPercent { get; set; }
        public decimal? DealDiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Configurations { get; set; }
        public PromotionResponse(Guid id) : base(id) { }
    }
}
