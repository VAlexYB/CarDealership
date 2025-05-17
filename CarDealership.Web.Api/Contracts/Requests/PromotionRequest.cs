namespace CarDealership.Web.Api.Contracts.Requests
{
    public class PromotionRequest : BaseRequest
    {
        public string Promocode { get; set; }
        public decimal? OrderDiscountPercent { get; set; }
        public decimal? DealDiscountPercent { get; set; }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set => _startDate = value.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
                : value.ToUniversalTime();
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set => _endDate = value.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
                : value.ToUniversalTime();
        }

        public ICollection<Guid> ConfigIds { get; set; } = [];
    }
}
