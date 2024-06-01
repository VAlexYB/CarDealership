using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using DriveType = CarDealership.Core.Models.DriveType;

namespace CarDealership.Web.Api.Factories.Abstract
{
    public interface IDriveTypeRMFactory : IModelBuilder<DriveTypeRequest, DriveType>, IResponseBuilder<DriveTypeResponse, DriveType>
    {
    }
}
