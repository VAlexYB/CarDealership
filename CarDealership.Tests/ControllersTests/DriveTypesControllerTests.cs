using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Controllers;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using DriveType = CarDealership.Core.Models.DriveType;

namespace CarDealership.Tests.ControllersTests
{
    public class DriveTypesControllerTests
    {
        private readonly Mock<IDriveTypesService> _serviceMock;
        private readonly Mock<IDriveTypeRMFactory> _factoryMock;
        private readonly Mock<ILogger<DriveTypesController>> _loggerMock;
        private readonly DriveTypesController _controller;

        public DriveTypesControllerTests()
        {
            _serviceMock = new Mock<IDriveTypesService>();
            _factoryMock = new Mock<IDriveTypeRMFactory>();
            _loggerMock = new Mock<ILogger<DriveTypesController>>();
            _controller = new DriveTypesController(
                _serviceMock.Object,
                _factoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithListOfDriveTypes()
        {
            //Arrange
            var driveTypes = new List<DriveType>
            {
                DriveType.Create(Guid.NewGuid(), "привод1", 1000).Value,
                DriveType.Create(Guid.NewGuid(), "привод2", 2000).Value,
                DriveType.Create(Guid.NewGuid(), "привод4", 3000).Value
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(driveTypes);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<DriveType>())).Returns((DriveType dt) => new DriveTypeResponse(dt.Id)
            {
                Price = dt.Price
            });

            //Act
            var result = await _controller.GetAllAsync();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<DriveTypeResponse>>(okResult.Value);
            Assert.Equal(3, returnValue.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WithDriveType()
        {
            // Arrange
            var driveType = DriveType.Create(Guid.NewGuid(), "привод1", 1000).Value;
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(driveType);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<DriveType>())).Returns(new DriveTypeResponse(driveType.Id) { Price = driveType.Price });

            // Act
            var result = await _controller.GetByIdAsync(driveType.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<DriveTypeResponse>(okResult.Value);
            Assert.Equal(driveType.Price, returnValue.Price);
        }

        [Fact]
        public async Task CreateOrEditAsync_ReturnsOkResult()
        {
            // Arrange
            var driveTypeRequest = new DriveTypeRequest { Price = 1000 };
            var driveType = DriveType.Create(Guid.NewGuid(), "привод1", 1000).Value;
            _factoryMock.Setup(f => f.CreateModel(It.IsAny<DriveTypeRequest>())).Returns(driveType);
            _serviceMock.Setup(s => s.CreateOrEditAsync(It.IsAny<DriveType>())).ReturnsAsync(driveType.Id);

            // Act
            var result = await _controller.CreateOrEditAsync(driveTypeRequest);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsOkResult()
        {
            // Arrange
            var driveTypeId = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(driveTypeId);

            // Act
            var result = await _controller.DeleteByIdAsync(driveTypeId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }
    }
}
