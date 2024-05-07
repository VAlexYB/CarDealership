using CarDealership.Core.Enums;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;

namespace CarDealership.Web.Api.Contracts.Responses
{
    public class AutoModelResponse : BaseResponse
    {
        public AutoModelResponse(AutoModel model) : base(model.Id)
        {
            Brand = model.Brand;
            Name = model.Name;
            BodyType = model.BodyType;
            Price = model.Price;
        }
        public string Brand;
        public string Name;
        public BodyType BodyType;
        public decimal Price;
    }
}
