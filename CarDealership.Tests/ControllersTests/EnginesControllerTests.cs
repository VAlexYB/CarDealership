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
    public class EnginesControllerTests
    {
        private readonly Mock<IEnginesService> _serviceMock;
        private readonly Mock<IEngineRMFactory> _factoryMock;
        private readonly Mock<ILogger<EnginesController>> _loggerMock;
        private readonly EnginesController _controller;

        public EnginesControllerTests()
        {
            _serviceMock = new Mock<IEnginesService>();
            _factoryMock = new Mock<IEngineRMFactory>();
            _loggerMock = new Mock<ILogger<EnginesController>>();
            _controller = new EnginesController(
                _serviceMock.Object,
                _factoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithListOfEngines()
        {
            //Arrange
            var engines = new List<Engine>
            {
                Engine.Create(Guid.NewGuid(), 1, 1, 1000, Guid.NewGuid(), Guid.NewGuid()).Value,
                Engine.Create(Guid.NewGuid(), 2, 2, 2000, Guid.NewGuid(), Guid.NewGuid()).Value,
                Engine.Create(Guid.NewGuid(), 3, 3, 3000, Guid.NewGuid(), Guid.NewGuid()).Value
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(engines);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<Engine>())).Returns((Engine e) => new EngineResponse(e.Id)
            {
                Price = e.Price
            });

            //Act
            var result = await _controller.GetAllAsync();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<EngineResponse>>(okResult.Value);
            Assert.Equal(3, returnValue.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WithEngine()
        {
            // Arrange
            var engine = Engine.Create(Guid.NewGuid(), 1, 1, 1000, Guid.NewGuid(), Guid.NewGuid()).Value;
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(engine);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<Engine>())).Returns(new EngineResponse(engine.Id) { Price = engine.Price });

            // Act
            var result = await _controller.GetByIdAsync(engine.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<EngineResponse>(okResult.Value);
            Assert.Equal(engine.Price, returnValue.Price);
        }

        [Fact]
        public async Task CreateOrEditAsync_ReturnsOkResult()
        {
            // Arrange
            var engineRequest = new EngineRequest { Price = 1000 };
            var engine = Engine.Create(Guid.NewGuid(), 1, 1, 1000, Guid.NewGuid(), Guid.NewGuid()).Value;
            _factoryMock.Setup(f => f.CreateModelAsync(It.IsAny<EngineRequest>())).ReturnsAsync(engine);
            _serviceMock.Setup(s => s.CreateOrEditAsync(It.IsAny<Engine>())).ReturnsAsync(engine.Id);

            // Act
            var result = await _controller.CreateOrEditAsync(engineRequest);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsOkResult()
        {
            // Arrange
            var engineId = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(engineId);

            // Act
            var result = await _controller.DeleteByIdAsync(engineId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }
    }
}
