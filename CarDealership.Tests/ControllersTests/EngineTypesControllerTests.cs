using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Controllers;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CarDealership.Tests.ControllersTests
{
    public class EngineTypesControllerTests
    {
        private readonly Mock<IEngineTypesService> _serviceMock;
        private readonly Mock<IEngineTypeRMFactory> _factoryMock;
        private readonly Mock<ILogger<EngineTypesController>> _loggerMock;
        private readonly EngineTypesController _controller;

        public EngineTypesControllerTests()
        {
            _serviceMock = new Mock<IEngineTypesService>();
            _factoryMock = new Mock<IEngineTypeRMFactory>();
            _loggerMock = new Mock<ILogger<EngineTypesController>>();
            _controller = new EngineTypesController(
                _serviceMock.Object,
                _factoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithListOfEngineTypes()
        {
            //Arrange
            var engineTypes = new List<EngineType>
            {
                EngineType.Create(Guid.NewGuid(), "тип1").Value,
                EngineType.Create(Guid.NewGuid(), "тип1").Value,
                EngineType.Create(Guid.NewGuid(), "тип1").Value
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(engineTypes);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<EngineType>())).Returns((EngineType et) => new EngineTypeResponse(et.Id)
            {
                Value = et.Value
            });

            //Act
            var result = await _controller.GetAllAsync();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<EngineTypeResponse>>(okResult.Value);
            Assert.Equal(3, returnValue.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WithEngineType()
        {
            // Arrange
            var engineType = EngineType.Create(Guid.NewGuid(), "тип1").Value;
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(engineType);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<EngineType>())).Returns(new EngineTypeResponse(engineType.Id) { Value = engineType.Value });

            // Act
            var result = await _controller.GetByIdAsync(engineType.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<EngineTypeResponse>(okResult.Value);
            Assert.Equal(engineType.Value, returnValue.Value);
        }

        [Fact]
        public async Task CreateOrEditAsync_ReturnsOkResult()
        {
            // Arrange
            var engineTypeRequest = new EngineTypeRequest();
            var engineType = EngineType.Create(Guid.NewGuid(), "тип1").Value;
            _factoryMock.Setup(f => f.CreateModel(It.IsAny<EngineTypeRequest>())).Returns(engineType);
            _serviceMock.Setup(s => s.CreateOrEditAsync(It.IsAny<EngineType>())).ReturnsAsync(engineType.Id);

            // Act
            var result = await _controller.CreateOrEditAsync(engineTypeRequest);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsOkResult()
        {
            // Arrange
            var autoConfigId = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(autoConfigId);

            // Act
            var result = await _controller.DeleteByIdAsync(autoConfigId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }
    }
}
