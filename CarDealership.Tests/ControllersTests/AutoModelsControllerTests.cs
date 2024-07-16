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
    public class AutoModelsControllerTests
    {
        private readonly Mock<IAutoModelsService> _serviceMock;
        private readonly Mock<IAutoModelRMFactory> _factoryMock;
        private readonly Mock<ILogger<AutoModelsController>> _loggerMock;
        private readonly AutoModelsController _controller;

        public AutoModelsControllerTests()
        {
            _serviceMock = new Mock<IAutoModelsService>();
            _factoryMock = new Mock<IAutoModelRMFactory>();
            _loggerMock = new Mock<ILogger<AutoModelsController>>();
            _controller = new AutoModelsController(
                _serviceMock.Object,
                _factoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithListOfAutoModels()
        {
            //Arrange
            var autoModels = new List<AutoModel>
            {
                AutoModel.Create(Guid.NewGuid(), "defaultName", 1000, Guid.NewGuid()).Value,
                AutoModel.Create(Guid.NewGuid(), "defaultName", 2000, Guid.NewGuid()).Value,
                AutoModel.Create(Guid.NewGuid(), "defaultName", 3000, Guid.NewGuid()).Value
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(autoModels);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<AutoModel>())).Returns((AutoModel am) => new AutoModelResponse(am.Id));

            //Act
            var result = await _controller.GetAllAsync();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<AutoModelResponse>>(okResult.Value);
            Assert.Equal(3, returnValue.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WithAutoModel()
        {
            // Arrange
            var autoModel = AutoModel.Create(Guid.NewGuid(), "defaultName", 1000, Guid.NewGuid()).Value;
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(autoModel);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<AutoModel>())).Returns(new AutoModelResponse(autoModel.Id) { Price = 1000});

            // Act
            var result = await _controller.GetByIdAsync(autoModel.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<AutoModelResponse>(okResult.Value);
            Assert.Equal(autoModel.Price, returnValue.Price);
        }

        [Fact]
        public async Task CreateOrEditAsync_ReturnsOkResult()
        {
            // Arrange
            var autoModelRequest = new AutoModelRequest();
            var autoModel = AutoModel.Create(Guid.NewGuid(), "defaultName", 1000, Guid.NewGuid()).Value;
            _factoryMock.Setup(f => f.CreateModelAsync(It.IsAny<AutoModelRequest>())).ReturnsAsync(autoModel);
            _serviceMock.Setup(s => s.CreateOrEditAsync(It.IsAny<AutoModel>())).ReturnsAsync(autoModel.Id);

            // Act
            var result = await _controller.CreateOrEditAsync(autoModelRequest);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsOkResult()
        {
            // Arrange
            var autoModelId = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(autoModelId);

            // Act
            var result = await _controller.DeleteByIdAsync(autoModelId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }
    }
}
