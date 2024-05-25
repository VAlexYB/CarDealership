
using CarDealership.Core.Models;

namespace CarDealership.Web.Api.Contracts.Responses
{
    public class EngineResponse : BaseResponse
    {
        public int Power { get; set; } //лошадиные силы
        public int Consumption { get; set; } //эффективная мощность
        public decimal Price { get; set; }

        public Guid EngineTypeId { get; set; }
        public string EngineType { get; set; }

        public Guid TransmissionTypeId { get; set; }
        public string TransmissionType { get; set; }

        public EngineResponse(Guid id) : base(id)
        {
        }        
    }
}
