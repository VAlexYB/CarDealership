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
    public class CarsControllerTests
    {
        private readonly Mock<ICarsService> _serviceMock;
        private readonly Mock<ICarRMFactory> _factoryMock;
        private readonly Mock<ILogger<CarsController>> _loggerMock;
        private readonly CarsController _controller;

        public CarsControllerTests()
        {
            _serviceMock = new Mock<ICarsService>();
            _factoryMock = new Mock<ICarRMFactory>();
            _loggerMock = new Mock<ILogger<CarsController>>();
            _controller = new CarsController(
                _serviceMock.Object,
                _factoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithListOfCars()
        {
            //Arrange
            var cars = new List<Car>
            {
                Car.Create(Guid.NewGuid(), "12345678901234561", Guid.NewGuid()).Value,
                Car.Create(Guid.NewGuid(), "12345678901234562", Guid.NewGuid()).Value,
                Car.Create(Guid.NewGuid(), "12345678901234563", Guid.NewGuid()).Value
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(cars);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<Car>())).Returns((Car c) => new CarResponse(c.Id));

            //Act
            var result = await _controller.GetAllAsync();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<CarResponse>>(okResult.Value);
            Assert.Equal(3, returnValue.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WithCar()
        {
            // Arrange
            var car = Car.Create(Guid.NewGuid(), "12345678901234561", Guid.NewGuid()).Value;
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(car);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<Car>())).Returns(new CarResponse(car.Id) { VIN = car.VIN });

            // Act
            var result = await _controller.GetByIdAsync(car.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CarResponse>(okResult.Value);
            Assert.Equal(car.VIN, returnValue.VIN);
        }

        [Fact]
        public async Task CreateOrEditAsync_ReturnsOkResult()
        {
            // Arrange
            var carRequest = new CarRequest();
            var car = Car.Create(Guid.NewGuid(), "12345678901234561", Guid.NewGuid()).Value;
            _factoryMock.Setup(f => f.CreateModelAsync(It.IsAny<CarRequest>())).ReturnsAsync(car);
            _serviceMock.Setup(s => s.CreateOrEditAsync(It.IsAny<Car>())).ReturnsAsync(car.Id);

            // Act
            var result = await _controller.CreateOrEditAsync(carRequest);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsOkResult()
        {
            // Arrange
            var carId = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(carId);

            // Act
            var result = await _controller.DeleteByIdAsync(carId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }
    }
}
