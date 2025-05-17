using CarDealership.Core.Exceptions;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;

namespace CarDealership.Web.Api.Factories
{
    public class ColorRMFactory : IColorRMFactory
    {
        public Task<Color> CreateModel(ColorRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var colorCreateResult = Color.Create(req.Id, req.Value, req.Price);

            if(colorCreateResult.IsFailure)
            {
                throw new ClientInformationException(colorCreateResult.Error);
            }

            return Task.FromResult(colorCreateResult.Value);
        }

        public Task<ColorResponse> CreateResponse(Color model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var colorResponse = new ColorResponse(model.Id)
            {
                Value = model.Value,
                Price = model.Price
            };
            return Task.FromResult(colorResponse);
        }
    }
}
