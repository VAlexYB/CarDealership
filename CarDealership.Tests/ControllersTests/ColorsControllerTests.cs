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
    public class ColorsControllerTests
    {
        private readonly Mock<IColorsService> _serviceMock;
        private readonly Mock<IColorRMFactory> _factoryMock;
        private readonly Mock<ILogger<ColorsController>> _loggerMock;
        private readonly ColorsController _controller;

        public ColorsControllerTests()
        {
            _serviceMock = new Mock<IColorsService>();
            _factoryMock = new Mock<IColorRMFactory>();
            _loggerMock = new Mock<ILogger<ColorsController>>();
            _controller = new ColorsController(
                _serviceMock.Object,
                _factoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithListOfColors()
        {
            //Arrange
            var colors = new List<Color>
            {
                Color.Create(Guid.NewGuid(), "цвет1", 1000).Value,
                Color.Create(Guid.NewGuid(), "цвет2", 1000).Value,
                Color.Create(Guid.NewGuid(), "цвет3", 1000).Value
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(colors);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<Color>())).Returns((Color c) => new ColorResponse(c.Id)
            {
                Price = c.Price
            });

            //Act
            var result = await _controller.GetAllAsync();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ColorResponse>>(okResult.Value);
            Assert.Equal(3, returnValue.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WithColor()
        {
            // Arrange
            var color = Color.Create(Guid.NewGuid(), "цвет1", 1000).Value;
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(color);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<Color>())).Returns(new ColorResponse(color.Id) { Price = color.Price });

            // Act
            var result = await _controller.GetByIdAsync(color.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ColorResponse>(okResult.Value);
            Assert.Equal(color.Price, returnValue.Price);
        }

        [Fact]
        public async Task CreateOrEditAsync_ReturnsOkResult()
        {
            // Arrange
            var colorRequest = new ColorRequest { Price = 1000 };
            var color = Color.Create(Guid.NewGuid(), "цвет1", 1000).Value;
            _factoryMock.Setup(f => f.CreateModel(It.IsAny<ColorRequest>())).Returns(color);
            _serviceMock.Setup(s => s.CreateOrEditAsync(It.IsAny<Color>())).ReturnsAsync(color.Id);

            // Act
            var result = await _controller.CreateOrEditAsync(colorRequest);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsOkResult()
        {
            // Arrange
            var colorId = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(colorId);

            // Act
            var result = await _controller.DeleteByIdAsync(colorId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }
    }
}
