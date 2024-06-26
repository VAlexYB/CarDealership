﻿using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Components;

namespace CarDealership.Web.Api.Controllers
{
    [Route("api/automodel")]
    public class AutoModelsController : BaseController<AutoModel, BaseFilter, AutoModelRequest, AutoModelResponse>
    {
        public AutoModelsController(IAutoModelsService service, IAutoModelRMFactory factory) : base(service, factory)
        {
        }
    }
}
