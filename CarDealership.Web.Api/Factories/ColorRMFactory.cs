using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;

namespace CarDealership.Web.Api.Factories
{
    public class ColorRMFactory : IModelBuilder<ColorRequest, Color>, IResponseBuilder<ColorResponse, Color>
    {
        public Color CreateModel(ColorRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var colorCreateResult = Color.Create(req.Id, req.Value, req.Price, req.IsDeleted);

            if(colorCreateResult.IsFailure)
            {
                throw new InvalidOperationException(colorCreateResult.Error);
            }

            return colorCreateResult.Value;
        }

        public ColorResponse CreateResponse(Color model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var colorResponse = new ColorResponse(model.Id)
            {
                Value = model.Value,
                Price = model.Price
            };
            return colorResponse;
        }
    }
}
