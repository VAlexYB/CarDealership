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
    public class FeaturesControllerTests
    {
        private readonly Mock<IFeaturesService> _serviceMock;
        private readonly Mock<IFeatureRMFactory> _factoryMock;
        private readonly Mock<ILogger<FeaturesController>> _loggerMock;
        private readonly FeaturesController _controller;

        public FeaturesControllerTests()
        {
            _serviceMock = new Mock<IFeaturesService>();
            _factoryMock = new Mock<IFeatureRMFactory>();
            _loggerMock = new Mock<ILogger<FeaturesController>>();
            _controller = new FeaturesController(
                _serviceMock.Object,
                _factoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithListOfFeatures()
        {
            //Arrange
            var autoConfigs = new List<Feature>
            {
                Feature.Create(Guid.NewGuid(), "описание1").Value,
                Feature.Create(Guid.NewGuid(), "описание2").Value,
                Feature.Create(Guid.NewGuid(), "описание3").Value
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(autoConfigs);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<Feature>())).Returns((Feature f) => new FeatureResponse(f.Id)
            {
                Description = f.Description
            });

            //Act
            var result = await _controller.GetAllAsync();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<FeatureResponse>>(okResult.Value);
            Assert.Equal(3, returnValue.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WithFeature()
        {
            // Arrange
            var feature = Feature.Create(Guid.NewGuid(), "описание1").Value;
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(feature);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<Feature>())).Returns(new FeatureResponse(feature.Id) { Description = feature.Description });

            // Act
            var result = await _controller.GetByIdAsync(feature.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<FeatureResponse>(okResult.Value);
            Assert.Equal(feature.Description, returnValue.Description);
        }

        [Fact]
        public async Task CreateOrEditAsync_ReturnsOkResult()
        {
            // Arrange
            var featureRequest = new FeatureRequest();
            var feature = Feature.Create(Guid.NewGuid(), "описание1").Value;
            _factoryMock.Setup(f => f.CreateModel(It.IsAny<FeatureRequest>())).Returns(feature);
            _serviceMock.Setup(s => s.CreateOrEditAsync(It.IsAny<Feature>())).ReturnsAsync(feature.Id);

            // Act
            var result = await _controller.CreateOrEditAsync(featureRequest);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsOkResult()
        {
            // Arrange
            var featureId = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(featureId);

            // Act
            var result = await _controller.DeleteByIdAsync(featureId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }
    }
}
