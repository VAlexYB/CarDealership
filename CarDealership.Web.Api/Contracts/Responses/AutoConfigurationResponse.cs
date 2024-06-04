
namespace CarDealership.Web.Api.Contracts.Responses
{
    public class AutoConfigurationResponse : BaseResponse
    {   
        public decimal Price { get; set; }

        public Guid AutoModelId { get; set; }
        public string AutoModelName { get; set; }
        public string BrandName { get; set; }
        public string CountryName { get; set; }

        public Guid BodyTypeId { get; set; }
        public string BodyType { get; set; }


        public Guid DriveTypeId { get; set; }
        public string DriveType { get; set; }


        public EngineResponse Engine { get; set; }


        public Guid ColorId { get; set; }
        public string Color { get; set; }

        public Guid EquipmentId { get; set; }
        public EquipmentResponse Equipment { get; set; }

        public AutoConfigurationResponse(Guid id) : base(id)
        {

        }
    }
}
