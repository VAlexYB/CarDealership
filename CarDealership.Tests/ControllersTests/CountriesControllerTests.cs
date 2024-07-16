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
    public class CountriesControllerTests
    {
        private readonly Mock<ICountriesService> _serviceMock;
        private readonly Mock<ICountryRMFactory> _factoryMock;
        private readonly Mock<ILogger<CountriesController>> _loggerMock;
        private readonly CountriesController _controller;

        public CountriesControllerTests()
        {
            _serviceMock = new Mock<ICountriesService>();
            _factoryMock = new Mock<ICountryRMFactory>();
            _loggerMock = new Mock<ILogger<CountriesController>>();
            _controller = new CountriesController(
                _serviceMock.Object,
                _factoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithListOfCountries()
        {
            //Arrange
            var countries = new List<Country>
            {
                Country.Create(Guid.NewGuid(), "страна1").Value,
                Country.Create(Guid.NewGuid(), "страна2").Value,
                Country.Create(Guid.NewGuid(), "страна3").Value
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(countries);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<Country>())).Returns((Country ac) => new CountryResponse(ac.Id));

            //Act
            var result = await _controller.GetAllAsync();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<CountryResponse>>(okResult.Value);
            Assert.Equal(3, returnValue.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WithCountry()
        {
            // Arrange
            var country = Country.Create(Guid.NewGuid(), "страна1").Value;
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(country);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<Country>())).Returns(new CountryResponse(country.Id) { Name = country.Name });

            // Act
            var result = await _controller.GetByIdAsync(country.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CountryResponse>(okResult.Value);
            Assert.Equal(country.Name, returnValue.Name);
        }

        [Fact]
        public async Task CreateOrEditAsync_ReturnsOkResult()
        {
            // Arrange
            var countryRequest = new CountryRequest();
            var country = Country.Create(Guid.NewGuid(), "страна1").Value;
            _factoryMock.Setup(f => f.CreateModel(It.IsAny<CountryRequest>())).Returns(country);
            _serviceMock.Setup(s => s.CreateOrEditAsync(It.IsAny<Country>())).ReturnsAsync(country.Id);

            // Act
            var result = await _controller.CreateOrEditAsync(countryRequest);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsOkResult()
        {
            // Arrange
            var countryId = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(countryId);

            // Act
            var result = await _controller.DeleteByIdAsync(countryId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }
    }
}
