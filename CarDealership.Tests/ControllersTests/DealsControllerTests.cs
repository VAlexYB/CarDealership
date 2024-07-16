using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Enums;
using CarDealership.Core.Models;
using CarDealership.Core.Models.Auth;
using CarDealership.Infrastructure.Messaging;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Controllers;
using CarDealership.Web.Api.Factories.Abstract;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CarDealership.Tests.ControllersTests
{
    public class DealsControllerTests
    {
        private readonly Mock<IDealsService> _dealsServiceMock;
        private readonly Mock<IDealRMFactory> _dealRMFactoryMock;
        private readonly Mock<ILogger<DealsController>> _loggerMock;
        private readonly Mock<IRabbitMQMessageSender> _messageSenderMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly DealsController _controller;

        public DealsControllerTests()
        {
            _dealsServiceMock = new Mock<IDealsService>();
            _dealRMFactoryMock = new Mock<IDealRMFactory>();
            _loggerMock = new Mock<ILogger<DealsController>>();
            _messageSenderMock = new Mock<IRabbitMQMessageSender>();
            _configurationMock = new Mock<IConfiguration>();

            _controller = new DealsController(
                _dealsServiceMock.Object,
                _dealRMFactoryMock.Object,
                _loggerMock.Object,
                _messageSenderMock.Object,
                _configurationMock.Object
            );
        }

        [Fact]
        public async Task ChangeStatus_ReturnsBadRequest_WhenInvalidStatus()
        {
            // Arrange
            var request = new ChangeStatusRequest { Id = Guid.NewGuid(), Status = 999 };

            // Act
            var result = await _controller.ChangeStatus(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Нет такого статуса заказа", badRequestResult.Value);
        }

        [Fact]
        public async Task ChangeStatus_ReturnsOk_WhenValidRequest()
        {
            // Arrange
            var request = new ChangeStatusRequest { Id = Guid.NewGuid(), Status = (int)DealStatus.Approved };
            var customer = User.Create(Guid.NewGuid(), "username", "email", "hash", null, null, null, "1234567890").Value;
            var deal = Deal.Create(request.Id, DateTime.Now, DealStatus.Negotiation, 1000, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), false, null, null, customer).Value;
            

            _dealsServiceMock.Setup(s => s.GetByIdAsync(request.Id)).ReturnsAsync(deal);
            _configurationMock.Setup(c => c["RabbitMQ:Queues:CDQueue"]).Returns("CDQueue");

            // Act
            var result = await _controller.ChangeStatus(request);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _messageSenderMock.Verify(m => m.SendMessage(It.IsAny<MessageInfo>(), "CDQueue"), Times.Once);
        }

        [Fact]
        public async Task ChangeStatus_ReturnsServerError_OnException()
        {
            // Arrange
            var request = new ChangeStatusRequest { Id = Guid.NewGuid(), Status = (int)DealStatus.Approved };
            _dealsServiceMock.Setup(s => s.ChangeStatus(It.IsAny<Guid>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.ChangeStatus(request);

            // Assert
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetDealsWithoutManager_ReturnsOk_WithDeals()
        {
            // Arrange
            var deals = new List<Deal> { Deal.Create(Guid.NewGuid(), DateTime.Now, DealStatus.Negotiation, 1000, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Value };
        
            var dealResponses = deals.Select(d => new DealResponse(d.Id)).ToList();

            _dealsServiceMock.Setup(s => s.GetDealsWithoutManager()).ReturnsAsync(deals);
            _dealRMFactoryMock.Setup(f => f.CreateResponse(It.IsAny<Deal>())).Returns((Deal d) => new DealResponse (d.Id));

            // Act
            var result = await _controller.GetDealsWithoutManager();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<List<DealResponse>>(okResult.Value);
            Assert.Equal(dealResponses.Count, response.Count);
        }

        [Fact]
        public async Task GetDealsWithoutManager_ReturnsServerError_OnException()
        {
            // Arrange
            _dealsServiceMock.Setup(s => s.GetDealsWithoutManager()).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetDealsWithoutManager();

            // Assert
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverErrorResult.StatusCode);
        }

        [Fact]
        public async Task TakeDealInProcess_ReturnsOk_WhenValidRequest()
        {
            // Arrange
            var request = new TakeTaskRequest { ManagerId = Guid.NewGuid(), TaskId = Guid.NewGuid() };

            // Act
            var result = await _controller.TakeDealInProcess(request);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task TakeDealInProcess_ReturnsServerError_OnException()
        {
            // Arrange
            var request = new TakeTaskRequest { ManagerId = Guid.NewGuid(), TaskId = Guid.NewGuid() };
            _dealsServiceMock.Setup(s => s.TakeDealInProcess(It.IsAny<Guid>(), It.IsAny<Guid>())).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.TakeDealInProcess(request);

            // Assert
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverErrorResult.StatusCode);
        }

        [Fact]
        public async Task LeaveDeal_ReturnsOk_WhenValidRequest()
        {
            // Arrange
            var dealId = Guid.NewGuid();

            // Act
            var result = await _controller.LeaveDeal(dealId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task LeaveDeal_ReturnsServerError_OnException()
        {
            // Arrange
            var dealId = Guid.NewGuid();
            _dealsServiceMock.Setup(s => s.LeaveDeal(It.IsAny<Guid>())).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.LeaveDeal(dealId);

            // Assert
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverErrorResult.StatusCode);
        }
    }
}
