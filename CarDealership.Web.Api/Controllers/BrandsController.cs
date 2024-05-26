using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Components;

namespace CarDealership.Web.Api.Controllers
{
    [Route("api/brand")]
    public class BrandsController : BaseController<Brand, BaseFilter, BrandRequest, BrandResponse>
    {
        public BrandsController(IBrandsService service, IBrandRMFactory factory) : base(service, factory)
        {
        }
    }
}
