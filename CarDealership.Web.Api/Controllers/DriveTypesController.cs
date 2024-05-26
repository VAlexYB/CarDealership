using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Components;
using DriveType = CarDealership.Core.Models.DriveType;

namespace CarDealership.Web.Api.Controllers
{
    [Route("api/drivetype")]
    public class DriveTypesController : BaseController<DriveType, BaseFilter, DriveTypeRequest, DriveTypeResponse>
    {
        public DriveTypesController(IDriveTypesService service, IDriveTypeRMFactory factory) : base(service, factory)
        {
        }
    }
}
