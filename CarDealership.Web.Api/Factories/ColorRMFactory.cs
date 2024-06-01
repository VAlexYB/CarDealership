using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;

namespace CarDealership.Web.Api.Factories
{
    public class ColorRMFactory : IColorRMFactory
    {
        public Color CreateModel(ColorRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var colorCreateResult = Color.Create(req.Id, req.Value, req.Price);

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
