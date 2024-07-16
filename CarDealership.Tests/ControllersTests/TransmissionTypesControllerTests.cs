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
    public class TransmissionTypesControllerTests
    {
        private readonly Mock<ITransmissionTypesService> _serviceMock;
        private readonly Mock<ITransmissionTypeRMFactory> _factoryMock;
        private readonly Mock<ILogger<TransmissionTypesController>> _loggerMock;
        private readonly TransmissionTypesController _controller;

        public TransmissionTypesControllerTests()
        {
            _serviceMock = new Mock<ITransmissionTypesService>();
            _factoryMock = new Mock<ITransmissionTypeRMFactory>();
            _loggerMock = new Mock<ILogger<TransmissionTypesController>>();
            _controller = new TransmissionTypesController(
                _serviceMock.Object,
                _factoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithListOfTransmissionTypes()
        {
            //Arrange
            var transmissionTypes = new List<TransmissionType>
            {
                TransmissionType.Create(Guid.NewGuid(), "тип1").Value,
                TransmissionType.Create(Guid.NewGuid(), "тип2").Value,
                TransmissionType.Create(Guid.NewGuid(), "тип3").Value
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(transmissionTypes);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<TransmissionType>())).Returns((TransmissionType tt) => new TransmissionTypeResponse(tt.Id)
            {
                Value = tt.Value
            });

            //Act
            var result = await _controller.GetAllAsync();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<TransmissionTypeResponse>>(okResult.Value);
            Assert.Equal(3, returnValue.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WithTransmissonType()
        {
            // Arrange
            var transmissionType = TransmissionType.Create(Guid.NewGuid(), "тип1").Value;
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(transmissionType);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<TransmissionType>())).Returns(new TransmissionTypeResponse(transmissionType.Id) { Value = transmissionType.Value });

            // Act
            var result = await _controller.GetByIdAsync(transmissionType.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TransmissionTypeResponse>(okResult.Value);
            Assert.Equal(transmissionType.Value, returnValue.Value);
        }

        [Fact]
        public async Task CreateOrEditAsync_ReturnsOkResult()
        {
            // Arrange
            var transmissionTypeRequest = new TransmissionTypeRequest();
            var transmissionType = TransmissionType.Create(Guid.NewGuid(), "тип1").Value;
            _factoryMock.Setup(f => f.CreateModel(It.IsAny<TransmissionTypeRequest>())).Returns(transmissionType);
            _serviceMock.Setup(s => s.CreateOrEditAsync(It.IsAny<TransmissionType>())).ReturnsAsync(transmissionType.Id);

            // Act
            var result = await _controller.CreateOrEditAsync(transmissionTypeRequest);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsOkResult()
        {
            // Arrange
            var transmissionTypeId = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(transmissionTypeId);

            // Act
            var result = await _controller.DeleteByIdAsync(transmissionTypeId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }
    }
}
