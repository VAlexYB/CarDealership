using CarDealership.Core.Enums;

namespace CarDealership.Web.Api.Contracts.Requests
{
    public class AutoModelRequest : BaseRequest
    {
        public string Brand;
        public string Name;
        public BodyType BodyType;
        public decimal Price;
    }
}
