using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Filters;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Components;

namespace CarDealership.Web.Api.Controllers
{
    public class AutoModelsController : BaseController<AutoModel, AutoModelsFilter, AutoModelRequest, AutoModelResponse>
    {
        public AutoModelsController(IAutoModelsService service, IAutoModelRMFactory factory) : base(service, factory)
        {
        }
    }
}
