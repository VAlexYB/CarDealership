using CarDealership.Core.Models.Auth;
using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using System.Text;
using System.Xml.Linq;

namespace CarDealership.Core.Models
{
    public class Promotion : BaseModel
    {
        public string Promocode { get; set; }
        public decimal? OrderDiscountPercent { get; set; }
        public decimal? DealDiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        private readonly List<AutoConfiguration> appliableConfigs = new List<AutoConfiguration>();
        private readonly List<User> participants = new List<User>();
        public IReadOnlyCollection<AutoConfiguration> AppliableConfigs => appliableConfigs.AsReadOnly();
        public IReadOnlyCollection<User> Participants => participants.AsReadOnly();

        [JsonConstructor]
        private Promotion
        (
            Guid id,
            string promocode,
            DateTime startDate,
            DateTime endDate,
            decimal? orderDiscountPercent = null,
            decimal? dealDiscountPercent = null,
            bool isDeleted = false
        ) : base(id)
        {
            Promocode = promocode;
            StartDate = startDate;
            EndDate = endDate;
            OrderDiscountPercent = orderDiscountPercent;
            DealDiscountPercent = dealDiscountPercent;
            IsDeleted = IsDeleted;
        }

        public void AddConfiguration(AutoConfiguration configuration)
        {
            appliableConfigs.Add(configuration);
        }

        public void AddTakedUser(User user)
        {
            participants.Add(user);
        }

        public static Result<Promotion> Create
        (
            Guid id,
            string promocode,
            DateTime startDate,
            DateTime endDate,
            decimal? orderDiscountPercent = null,
            decimal? dealDiscountPercent = null,
            bool isDeleted = false
        )
        {
            StringBuilder errorBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(promocode)) 
            {
                errorBuilder.Append("Для акции промокод обязателен. ");
            }

            if (!orderDiscountPercent.HasValue && !dealDiscountPercent.HasValue)
            {
                errorBuilder.Append("Акция должна давать скидку на какие-то события");
            }

            if (errorBuilder.Length > 0)
            {
                return Result.Failure<Promotion>(errorBuilder.ToString().Trim());
            }

            var promotion = new Promotion(id, promocode, startDate, endDate, orderDiscountPercent, dealDiscountPercent, isDeleted);
            return Result.Success(promotion);
        }
    }
}
