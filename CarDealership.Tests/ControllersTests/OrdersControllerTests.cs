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

namespace CarDealership.Tests.ControllersTests
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrdersService> _serviceMock;
        private readonly Mock<IOrderRMFactory> _factoryMock;
        private readonly Mock<ILogger<OrdersController>> _loggerMock;
        private readonly Mock<IRabbitMQMessageSender> _messageSenderMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _serviceMock = new Mock<IOrdersService>();
            _factoryMock = new Mock<IOrderRMFactory>();
            _loggerMock = new Mock<ILogger<OrdersController>>();
            _messageSenderMock = new Mock<IRabbitMQMessageSender>();
            _configurationMock = new Mock<IConfiguration>();

            _controller = new OrdersController(
                _serviceMock.Object,
                _factoryMock.Object,
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
            var request = new ChangeStatusRequest { Id = Guid.NewGuid(), Status = (int)OrderStatus.Processing };
            var customer = User.Create(Guid.NewGuid(), "username", "email", "hash", null, null, null, "1234567890").Value;
            var order = Order.Create(request.Id, DateTime.Now, DateTime.Now.AddDays(14), OrderStatus.Pending, 1000, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), false, null, null, customer).Value;


            _serviceMock.Setup(s => s.GetByIdAsync(request.Id)).ReturnsAsync(order);
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
            var request = new ChangeStatusRequest { Id = Guid.NewGuid(), Status = (int)OrderStatus.Processing };
            _serviceMock.Setup(s => s.ChangeStatus(It.IsAny<Guid>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.ChangeStatus(request);

            // Assert
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetOrdersWithoutManager_ReturnsOk_WithDeals()
        {
            // Arrange
            var orders = new List<Order> { Order.Create(Guid.NewGuid(), DateTime.Now, DateTime.Now.AddDays(14), OrderStatus.Pending, 1000, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Value };

            var orderResponses = orders.Select(d => new OrderResponse(d.Id)).ToList();

            _serviceMock.Setup(s => s.GetOrdersWithoutManager()).ReturnsAsync(orders);
            _factoryMock.Setup(f => f.CreateResponse(It.IsAny<Order>())).Returns((Order o) => new OrderResponse(o.Id));

            // Act
            var result = await _controller.GetOrdersWithoutManager();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<List<OrderResponse>>(okResult.Value);
            Assert.Equal(orderResponses.Count, response.Count);
        }

        [Fact]
        public async Task GetOrdersWithoutManager_ReturnsServerError_OnException()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetOrdersWithoutManager()).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetOrdersWithoutManager();

            // Assert
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverErrorResult.StatusCode);
        }

        [Fact]
        public async Task TakeOrderInProcess_ReturnsOk_WhenValidRequest()
        {
            // Arrange
            var request = new TakeTaskRequest { ManagerId = Guid.NewGuid(), TaskId = Guid.NewGuid() };

            // Act
            var result = await _controller.TakeOrderInProcess(request);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task TakeOrderInProcess_ReturnsServerError_OnException()
        {
            // Arrange
            var request = new TakeTaskRequest { ManagerId = Guid.NewGuid(), TaskId = Guid.NewGuid() };
            _serviceMock.Setup(s => s.TakeOrderInProcess(It.IsAny<Guid>(), It.IsAny<Guid>())).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.TakeOrderInProcess(request);

            // Assert
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverErrorResult.StatusCode);
        }

        [Fact]
        public async Task LeaveOrder_ReturnsOk_WhenValidRequest()
        {
            // Arrange
            var dealId = Guid.NewGuid();

            // Act
            var result = await _controller.LeaveOrder(dealId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task LeaveOrder_ReturnsServerError_OnException()
        {
            // Arrange
            var dealId = Guid.NewGuid();
            _serviceMock.Setup(s => s.LeaveOrder(It.IsAny<Guid>())).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.LeaveOrder(dealId);

            // Assert
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverErrorResult.StatusCode);
        }
    }
}
