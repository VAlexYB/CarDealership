using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;
using DriveType = CarDealership.Core.Models.DriveType;

namespace CarDealership.Web.Api.Factories
{
    public class DriveTypeRMFactory : IDriveTypeRMFactory
    {
        public DriveType CreateModel(DriveTypeRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var driveTypeCreateResult = DriveType.Create(req.Id, req.Value, req.Price, req.IsDeleted);

            if(driveTypeCreateResult.IsFailure)
            {
                throw new InvalidOperationException(driveTypeCreateResult.Error);
            }

            return driveTypeCreateResult.Value;
        }

        public DriveTypeResponse CreateResponse(DriveType model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var driveTypeRes = new DriveTypeResponse(model.Id)
            {
                Value = model.Value,
                Price = model.Price,
            };

            return driveTypeRes;
        }
    }
}
