using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Factories.Abstract;

namespace CarDealership.Web.Api.Factories
{
    public class AutoConfigRMFactory : IAutoConfigRMFactory
    {
        private readonly IAutoModelsService _autoModelsService;
        private readonly IBodyTypesService _bodyTypesService;
        private readonly IDriveTypesService _driveTypesService;
        private readonly IEnginesService _enginesService;
        private readonly IColorsService _colorsService;
        private readonly IEquipmentsService _equipmentsService;

        private readonly IEngineRMFactory _engineRMFactory;
        private readonly IEquipmentRMFactory _equipmentRMFactory;
        public AutoConfigRMFactory(
            IAutoModelsService autoModelsService,
            IBodyTypesService bodyTypesService,
            IDriveTypesService driveTypesService,
            IEnginesService enginesService,
            IColorsService colorsService,
            IEquipmentsService equipmentsService,
            IEngineRMFactory engineRMFactory,
            IEquipmentRMFactory equipmentRMFactory) 
        { 
            _autoModelsService = autoModelsService ?? throw new ArgumentNullException(nameof(autoModelsService));
            _bodyTypesService = bodyTypesService ?? throw new ArgumentNullException(nameof(bodyTypesService));
            _driveTypesService = driveTypesService ?? throw new ArgumentNullException(nameof(driveTypesService));
            _enginesService = enginesService ?? throw new ArgumentNullException(nameof(enginesService));
            _colorsService = colorsService ?? throw new ArgumentNullException(nameof(colorsService));
            _equipmentsService = equipmentsService ?? throw new ArgumentNullException(nameof(equipmentsService));

            _engineRMFactory = engineRMFactory ?? throw new ArgumentNullException(nameof(engineRMFactory));
            _equipmentRMFactory = equipmentRMFactory ?? throw new ArgumentNullException(nameof(equipmentRMFactory));
        }

        public async Task<AutoConfiguration> CreateModelAsync(AutoConfigurationRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var autoModel = await _autoModelsService.GetByIdAsync(req.AutoModelId) ?? throw new ArgumentException($"Модель авто с Id = {req.AutoModelId} не найдена");
            var bodyType = await _bodyTypesService.GetByIdAsync(req.BodyTypeId) ?? throw new ArgumentException($"Тип кузова с Id = {req.BodyTypeId} не найден");
            var driveType = await _driveTypesService.GetByIdAsync(req.DriveTypeId) ?? throw new ArgumentException($"Привод с Id = {req.DriveTypeId} не найден");
            var engineType = await _enginesService.GetByIdAsync(req.EngineId) ?? throw new ArgumentException($"Двигатель с Id = {req.EngineId} не найден");
            var color = await _colorsService.GetByIdAsync(req.ColorId) ?? throw new ArgumentException($"Цвет покраски авто с Id = {req.ColorId} не найден");
            var equipment = await _equipmentsService.GetByIdAsync(req.EquipmentId) ?? throw new ArgumentException($"Комплектация с Id = {req.EquipmentId} не найден");

            var configCreateResult = AutoConfiguration.Create(
                req.Id, req.Price, req.AutoModelId, req.BodyTypeId, 
                req.DriveTypeId, req.EngineId, req.ColorId, req.EquipmentId, false,
                autoModel, bodyType, driveType, engineType, color, equipment);

            if (configCreateResult.IsFailure)
            {
                throw new InvalidOperationException(configCreateResult.Error);
            }

            var autoConfig = configCreateResult.Value;
            autoModel.AddConfiguration(autoConfig);
            bodyType.AddConfiguration(autoConfig);
            driveType.AddConfiguration(autoConfig);
            engineType.AddConfiguration(autoConfig);
            color.AddConfiguration(autoConfig);
            equipment.AddConfiguration(autoConfig);
            return autoConfig;
        }

        public AutoConfigurationResponse CreateResponse(AutoConfiguration model)
        {
            if(model == null) throw new ArgumentNullException(nameof(model));

            var configEngineRes = _engineRMFactory.CreateResponse(model.Engine);
            var configEquipRes = _equipmentRMFactory.CreateResponse(model.Equipment);

            decimal autoModelPrice = model.AutoModel?.Price ?? 0;
            decimal bodyTypePrice = model.BodyType?.Price ?? 0;
            decimal driveTypePrice = model.DriveType?.Price ?? 0;
            decimal colorPrice = model.Color?.Price ?? 0;
            decimal equipmentPrice = model.Equipment?.Price ?? 0;

            var response = new AutoConfigurationResponse(model.Id)
            {
                Price = model.Price + autoModelPrice + bodyTypePrice + driveTypePrice + configEngineRes.Price + colorPrice + equipmentPrice,
                AutoModelId = model.AutoModelId,
                AutoModelName = model.AutoModel?.Name ?? string.Empty,
                BrandName = model.AutoModel?.Brand?.Name ?? string.Empty, 

                BodyTypeId = model.BodyTypeId,
                BodyType = model.BodyType?.Value ?? string.Empty,

                DriveTypeId = model.DriveTypeId,
                DriveType = model.DriveType?.Value ?? string.Empty,

                Engine = configEngineRes,

                ColorId = model.ColorId,
                Color = model.Color?.Value ?? string.Empty,

                EquipmentId = model.EquipmentId,
                Equipment = configEquipRes
            };

            return response;
        }
    }
}
