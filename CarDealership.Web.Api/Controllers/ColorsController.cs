using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Components;

namespace CarDealership.Web.Api.Controllers
{
    [Route("api/color")]
    public class ColorsController : BaseController<Color, BaseFilter, ColorRequest, ColorResponse>
    {
        public ColorsController(IColorsService service, IColorRMFactory factory) : base(service, factory)
        {
        }
    }
}
