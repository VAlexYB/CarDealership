using CarDealership.Application.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories;
using Microsoft.AspNetCore.Components;


namespace CarDealership.Web.Api.Controllers
{
    [Route("api/autoconfig")]
    public class AutoConfigController : BaseController<AutoConfiguration, BaseFilter, AutoConfigurationRequest, AutoConfigurationResponse>
    {
        public AutoConfigController(AutoConfigsService service, AutoConfigRMFactory factory) : base(service, factory)
        {
        }
    }
}
