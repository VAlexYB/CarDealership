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
    public class BrandsControllerTests
    {
        private readonly Mock<IBrandsService> _serviceMock;
        private readonly Mock<IBrandRMFactory> _factoryMock;
        private readonly Mock<ILogger<BrandsController>> _loggerMock;
        private readonly BrandsController _controller;

        public BrandsControllerTests()
        {
            _serviceMock = new Mock<IBrandsService>();
            _factoryMock = new Mock<IBrandRMFactory>();
            _loggerMock = new Mock<ILogger<BrandsController>>();
            _controller = new BrandsController(
                _serviceMock.Object,
                _factoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithListOfBrands()
        {
            //Arrange
            var brands = new List<Brand>
            {
                Brand.Create(Guid.NewGuid(), "brandName1", Guid.NewGuid()).Value,
                Brand.Create(Guid.NewGuid(), "brandName2", Guid.NewGuid()).Value,
                Brand.Create(Guid.NewGuid(), "brandName3", Guid.NewGuid()).Value
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(brands);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<Brand>())).Returns((Brand b) => new BrandResponse(b.Id));

            //Act
            var result = await _controller.GetAllAsync();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<BrandResponse>>(okResult.Value);
            Assert.Equal(3, returnValue.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WithBrand()
        {
            // Arrange
            var brand = Brand.Create(Guid.NewGuid(), "brandName1", Guid.NewGuid()).Value;
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(brand);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<Brand>())).Returns(new BrandResponse(brand.Id) { Name = brand.Name });

            // Act
            var result = await _controller.GetByIdAsync(brand.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<BrandResponse>(okResult.Value);
            Assert.Equal(brand.Name, returnValue.Name);
        }

        [Fact]
        public async Task CreateOrEditAsync_ReturnsOkResult()
        {
            // Arrange
            var brandRequest = new BrandRequest();
            var brand = Brand.Create(Guid.NewGuid(), "brandName1", Guid.NewGuid()).Value;
            _factoryMock.Setup(f => f.CreateModelAsync(It.IsAny<BrandRequest>())).ReturnsAsync(brand);
            _serviceMock.Setup(s => s.CreateOrEditAsync(It.IsAny<Brand>())).ReturnsAsync(brand.Id);

            // Act
            var result = await _controller.CreateOrEditAsync(brandRequest);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsOkResult()
        {
            // Arrange
            var brandId = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(brandId);

            // Act
            var result = await _controller.DeleteByIdAsync(brandId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }
    }
}
