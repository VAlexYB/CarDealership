﻿using CarDealership.Application.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;

namespace CarDealership.Web.Api.Factories
{
    public class EngineRMFactory : IModelBuilderAsync<EngineRequest, Engine>, IResponseBuilder<EngineResponse, Engine>
    {
        private readonly EngineTypesService _engineTypesService;
        private readonly TransmissionTypesService _transmissionTypesService;

        public EngineRMFactory(EngineTypesService engineTypesService, TransmissionTypesService transmissionTypesService)
        {
            _engineTypesService = engineTypesService ?? throw new ArgumentNullException(nameof(engineTypesService));
            _transmissionTypesService = transmissionTypesService ?? throw new ArgumentNullException(nameof(transmissionTypesService));
        }
        public async Task<Engine> CreateModelAsync(EngineRequest req)
        {
            if(req == null) throw new ArgumentNullException(nameof(req));

            var engineType = await _engineTypesService.GetByIdAsync(req.EngineTypeId);
            var transmissionType = await _transmissionTypesService.GetByIdAsync(req.TransmissionTypeId);

            var engineCreateResult = Engine.Create(
                req.Id,
                req.Power,
                req.Consumption,
                req.Price,
                req.EngineTypeId,
                req.TransmissionTypeId,
                req.IsDeleted,
                engineType,
                transmissionType
            );

            if(engineCreateResult.IsFailure)
            {
                throw new InvalidOperationException(engineCreateResult.Error);
            }

            var engine = engineCreateResult.Value;

            engineType.AddEngine(engine);
            transmissionType.AddEngine(engine);
            return engine;
        }

        public EngineResponse CreateResponse(Engine model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var response = new EngineResponse(model.Id)
            {
                Power = model.Power,
                Consumption = model.Consumption,
                Price = model.Price,
                EngineTypeId = model.EngineTypeId,
                EngineType = model.EngineType?.Value ?? "Неизвестный тип двигателя",
                TransmissionTypeId = model.TransmissionTypeId,
                TransmissionType = model.TransmissionType?.Value ?? "Неизвестный тип трансмиссии"
            };

            return response;
        }
    }
}
