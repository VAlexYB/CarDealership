using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Components;

namespace CarDealership.Web.Api.Controllers
{
    public class TransmissionTypesController : BaseController<TransmissionType, BaseFilter, TransmissionTypeRequest, TransmissionTypeResponse>
    {
        public TransmissionTypesController(ITransmissionTypesService service, ITransmissionTypeRMFactory factory, ILogger<TransmissionTypesController> logger) : base(service, factory, logger)
        {
        }
    }
}
