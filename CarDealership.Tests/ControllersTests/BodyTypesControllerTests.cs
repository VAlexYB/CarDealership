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
    public class BodyTypesControllerTests
    {
        private readonly Mock<IBodyTypesService> _serviceMock;
        private readonly Mock<IBodyTypeRMFactory> _factoryMock;
        private readonly Mock<ILogger<BodyTypesController>> _loggerMock;
        private readonly BodyTypesController _controller;

        public BodyTypesControllerTests()
        {
            _serviceMock = new Mock<IBodyTypesService>();
            _factoryMock = new Mock<IBodyTypeRMFactory>();
            _loggerMock = new Mock<ILogger<BodyTypesController>>();
            _controller = new BodyTypesController(
                _serviceMock.Object,
                _factoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithListOfBodyTypes()
        {
            //Arrange
            var bodyTypes = new List<BodyType>
            {
                BodyType.Create(Guid.NewGuid(), "BodyType1", 1000).Value,
                BodyType.Create(Guid.NewGuid(), "BodyType2", 2000).Value,
                BodyType.Create(Guid.NewGuid(), "BodyType3", 3000).Value
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(bodyTypes);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<BodyType>())).Returns((BodyType bt) => new BodyTypeResponse(bt.Id)
            {
                Price = bt.Price
            });

            //Act
            var result = await _controller.GetAllAsync();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<BodyTypeResponse>>(okResult.Value);
            Assert.Equal(3, returnValue.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WithBodyType()
        {
            // Arrange
            var bodyType = BodyType.Create(Guid.NewGuid(), "BodyType1", 1000).Value;
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(bodyType);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<BodyType>())).Returns(new BodyTypeResponse(bodyType.Id) { Price = bodyType.Price });

            // Act
            var result = await _controller.GetByIdAsync(bodyType.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<BodyTypeResponse>(okResult.Value);
            Assert.Equal(bodyType.Price, returnValue.Price);
        }

        [Fact]
        public async Task CreateOrEditAsync_ReturnsOkResult()
        {
            // Arrange
            var bodyTypeRequest = new BodyTypeRequest { Price = 1000 };
            var bodyType = BodyType.Create(Guid.NewGuid(), "bodyType1", bodyTypeRequest.Price).Value;
            _factoryMock.Setup(f => f.CreateModel(It.IsAny<BodyTypeRequest>())).Returns(bodyType);
            _serviceMock.Setup(s => s.CreateOrEditAsync(It.IsAny<BodyType>())).ReturnsAsync(bodyType.Id);

            // Act
            var result = await _controller.CreateOrEditAsync(bodyTypeRequest);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsOkResult()
        {
            // Arrange
            var bodyTypeId = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(bodyTypeId);

            // Act
            var result = await _controller.DeleteByIdAsync(bodyTypeId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }
    }
}
