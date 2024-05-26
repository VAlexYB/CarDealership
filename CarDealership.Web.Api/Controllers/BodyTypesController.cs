using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Components;

namespace CarDealership.Web.Api.Controllers
{
    [Route("api/bodytype")]
    public class BodyTypesController : BaseController<BodyType, BaseFilter, BodyTypeRequest, BodyTypeResponse>
    {
        public BodyTypesController(IBodyTypesService service, IBodyTypeRMFactory factory) : base(service, factory)
        {
        }
    }
}
