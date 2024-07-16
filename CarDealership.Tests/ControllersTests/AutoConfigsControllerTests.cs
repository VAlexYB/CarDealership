using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Controllers;
using CarDealership.Web.Api.Factories.Abstract;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CarDealership.Tests.ControllersTests
{
    public class AutoConfigsControllerTests
    {
        private readonly Mock<IAutoConfigsService> _serviceMock;
        private readonly Mock<IAutoConfigRMFactory> _factoryMock;
        private readonly Mock<ILogger<AutoConfigController>> _loggerMock;
        private readonly AutoConfigController _controller;

        public AutoConfigsControllerTests()
        {
            _serviceMock = new Mock<IAutoConfigsService>();
            _factoryMock = new Mock<IAutoConfigRMFactory>();
            _loggerMock = new Mock<ILogger<AutoConfigController>>();
            _controller = new AutoConfigController(
                _serviceMock.Object,
                _factoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithListOfAutoConfigurations()
        {
            //Arrange
            var autoConfigs = new List<AutoConfiguration>
            {
                AutoConfiguration.Create(Guid.NewGuid(), 1000, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Value,
                AutoConfiguration.Create(Guid.NewGuid(), 2000, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Value,
                AutoConfiguration.Create(Guid.NewGuid(), 3000, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Value
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(autoConfigs);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<AutoConfiguration>())).Returns((AutoConfiguration ac) => new AutoConfigurationResponse(ac.Id)
            {
                Price = ac.Price
            });

            //Act
            var result = await _controller.GetAllAsync();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<AutoConfigurationResponse>>(okResult.Value);
            Assert.Equal(3, returnValue.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WithAutoConfiguration()
        {
            // Arrange
            var autoConfig = AutoConfiguration.Create(Guid.NewGuid(), 1000, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Value;
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(autoConfig);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<AutoConfiguration>())).Returns(new AutoConfigurationResponse(autoConfig.Id) { Price = autoConfig.Price });

            // Act
            var result = await _controller.GetByIdAsync(autoConfig.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<AutoConfigurationResponse>(okResult.Value);
            Assert.Equal(autoConfig.Price, returnValue.Price);
        }

        [Fact]
        public async Task CreateOrEditAsync_ReturnsOkResult()
        {
            // Arrange
            var autoConfigRequest = new AutoConfigurationRequest { Price = 1000 };
            var autoConfig = AutoConfiguration.Create(Guid.NewGuid(), autoConfigRequest.Price, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Value;
            _factoryMock.Setup(f => f.CreateModelAsync(It.IsAny<AutoConfigurationRequest>())).ReturnsAsync(autoConfig);
            _serviceMock.Setup(s => s.CreateOrEditAsync(It.IsAny<AutoConfiguration>())).ReturnsAsync(autoConfig.Id);

            // Act
            var result = await _controller.CreateOrEditAsync(autoConfigRequest);

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
