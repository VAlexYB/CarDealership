using CarDealership.DataAccess.Attributes;
using CarDealership.DataAccess.Entities.Auth;

namespace CarDealership.DataAccess.Entities
{
    public class PromotionEntity : BaseEntity
    {
        [Unique]
        public string Promocode { get; set; }
        public decimal? OrderDiscountPercent { get; set; }
        public decimal? DealDiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual List<AutoConfigurationEntity> AppliableConfigs { get; set; }
        public virtual List<UserEntity> Participants { get; set; }
    }
}
