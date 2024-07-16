using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Controllers;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CarDealership.Tests.ControllersTests
{
    public class EquipmentsControllerTests
    {
        private readonly Mock<IEquipmentsService> _equipmentsServiceMock;
        private readonly Mock<IEquipmentRMFactory> _equipmentRMFactoryMock;
        private readonly Mock<ILogger<EquipmentsController>> _loggerMock;
        private readonly EquipmentsController _controller;

        public EquipmentsControllerTests()
        {
            _equipmentsServiceMock = new Mock<IEquipmentsService>();
            _equipmentRMFactoryMock = new Mock<IEquipmentRMFactory>();
            _loggerMock = new Mock<ILogger<EquipmentsController>>();

            _controller = new EquipmentsController(
                _equipmentsServiceMock.Object,
                _equipmentRMFactoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task CreateOrEditAsync_ReturnsOkResult()
        {
            // Arrange
            var equipmentRequest = new EquipmentRequest { FeatureIds = new List<Guid> { Guid.NewGuid() } };
            var equipment = Equipment.Create(Guid.NewGuid(), "комплектация1", 1000, "2000", Guid.NewGuid()).Value;
            _equipmentRMFactoryMock.Setup(f => f.CreateModelAsync(equipmentRequest)).ReturnsAsync(equipment);
            _equipmentsServiceMock.Setup(s => s.CreateOrEditAsync(equipment, equipmentRequest.FeatureIds)).ReturnsAsync(equipment.Id);

            // Act
            var result = await _controller.CreateOrEditAsync(equipmentRequest);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task RemoveFeature_ReturnsOkResult()
        {
            // Arrange
            var request = new EquipFeatureChangeRequest { EquipmentId = Guid.NewGuid(), FeatureId = Guid.NewGuid() };
            _equipmentsServiceMock.Setup(s => s.RemoveFeatureFromEquipment(request.EquipmentId, request.FeatureId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.RemoveFeature(request);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AddFeature_ReturnsOkResult()
        {
            // Arrange
            var request = new EquipFeatureChangeRequest { EquipmentId = Guid.NewGuid(), FeatureId = Guid.NewGuid() };
            _equipmentsServiceMock.Setup(s => s.AddFeatureToEquipment(request.EquipmentId, request.FeatureId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddFeature(request);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
