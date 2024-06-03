using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;

namespace CarDealership.Web.Api.Contracts.Responses
{
    public class AutoModelResponse : BaseResponse
    {
        public string Name { get; set; }
        public Guid BrandId { get; set; }
        public string BrandName { get; set; }
        public string Country { get; set; }
        public decimal Price { get; set; }

        public AutoModelResponse(Guid id) : base(id)
        {

        }
    }
}
