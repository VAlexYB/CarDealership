using CarDealership.Application.Auth;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using CarDealership.Core.Models.Auth;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using CarDealership.Web.Api.Controllers;
using CarDealership.Web.Api.Factories.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CarDealership.Tests.ControllersTests
{
    public class UsersControllerTests
    {
        private readonly Mock<IUsersService> _mockUsersService;
        private readonly Mock<IRolesService> _mockRolesService;
        private readonly Mock<IPasswordHasher> _mockPasswordHasher;
        private readonly Mock<ILogger<UsersController>> _mockLogger;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockUsersService = new Mock<IUsersService>();
            _mockRolesService = new Mock<IRolesService>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockLogger = new Mock<ILogger<UsersController>>();

            _controller = new UsersController(
                _mockUsersService.Object,
                _mockRolesService.Object,
                _mockPasswordHasher.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenUserCreationFails()
        {
            // Arrange
            var request = new RegistrationRequest(Guid.NewGuid(), "user", "email@example.com", "password", null, null, null, null, null, null);
            _mockPasswordHasher.Setup(p => p.Generate(request.Password)).Returns("hashedPassword");
            _mockUsersService.Setup(u => u.AddAsync(It.IsAny<User>())).Throws(new InvalidOperationException("Creation failed"));

            // Act
            var result = await _controller.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal("Внутренняя ошибка сервера", badRequestResult.Value);
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenUserCreatedSuccessfully()
        {
            // Arrange
            var request = new RegistrationRequest(Guid.NewGuid(), "user", "email@example.com", "password", "FirstName", "MiddleName", "LastName", "1234567890", "1234", "5678");
            _mockPasswordHasher.Setup(p => p.Generate(request.Password)).Returns("hashedPassword");

            var user = User.Create(request.Id, request.UserName, request.Email, "hashedPassword",
                request.FirstName, request.MiddleName, request.LastName, request.PhoneNumber, request.FirstCardDigits, request.LastCardDigits).Value;
            _mockUsersService.Setup(u => u.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Register(request);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task AddManager_ReturnsBadRequest_WhenUserCreationFails()
        {
            // Arrange
            var request = new RegistrationRequest(Guid.NewGuid(), "manager", "manager@example.com", "password", null, null, null, null, null, null);
            _mockPasswordHasher.Setup(p => p.Generate(request.Password)).Returns("hashedPassword");
            _mockUsersService.Setup(u => u.AddAsync(It.IsAny<User>())).Throws(new InvalidOperationException("Creation failed"));

            // Act
            var result = await _controller.AddManager(request);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal("Внутренняя ошибка", badRequestResult.Value);
        }

        [Fact]
        public async Task AddManager_ReturnsOk_WhenManagerCreatedSuccessfully()
        {
            // Arrange
            var request = new RegistrationRequest(Guid.NewGuid(), "manager", "manager@example.com", "password", "FirstName", "MiddleName", "LastName", "1234567890", "1234", "5678");
            _mockPasswordHasher.Setup(p => p.Generate(request.Password)).Returns("hashedPassword");

            var manager = User.Create(request.Id, request.UserName, request.Email, "hashedPassword", request.FirstName, request.MiddleName, request.LastName, request.PhoneNumber, request.FirstCardDigits, request.LastCardDigits).Value;
            _mockUsersService.Setup(u => u.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddManager(request);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task AssignSenior_ReturnsOk_WhenManagerAssignedSuccessfully()
        {
            // Arrange
            var mgrId = Guid.NewGuid();
            _mockUsersService.Setup(u => u.AssignSenior(mgrId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AssignSenior(mgrId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task SuspendSenior_ReturnsOk_WhenManagerSuspendedSuccessfully()
        {
            // Arrange
            var mgrId = Guid.NewGuid();
            _mockUsersService.Setup(u => u.SuspendSenior(mgrId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.SuspendSenior(mgrId);

            // Assert
            Assert.IsType<OkResult>(result);
        }


        [Fact]
        public async Task Logout_ReturnsOk_WhenLogoutIsSuccessful()
        {
            // Act
            var result = await _controller.Logout();

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task GetAllMgrs_ReturnsOk_WhenManagersExist()
        {
            // Arrange
            var managers = new List<User> { User.Create(Guid.NewGuid(), "manager", "manager@example.com", "hashedPassword").Value };
            _mockUsersService.Setup(u => u.GetUsersAsync((int)Roles.Manager)).ReturnsAsync(managers);

            // Act
            var result = await _controller.GetAllMgrs();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<List<UserResponse>>(okResult.Value);
            Assert.Single(response);
        }

        [Fact]
        public async Task GetOnlyUsers_ReturnsOk_WhenUsersExist()
        {
            // Arrange
            var users = new List<User> { User.Create(Guid.NewGuid(), "user", "user@example.com", "hashedPassword").Value };
            _mockUsersService.Setup(u => u.GetUsersAsync((int)Roles.User)).ReturnsAsync(users);

            // Act
            var result = await _controller.GetOnlyUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<List<UserResponse>>(okResult.Value);
            Assert.Single(response);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WhenUsersExist()
        {
            // Arrange
            var users = new List<User> { User.Create(Guid.NewGuid(), "user", "user@example.com", "hashedPassword").Value };
            _mockUsersService.Setup(u => u.GetUsersAsync(It.IsAny<int?>())).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<List<UserResponse>>(okResult.Value);
            Assert.Single(response);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenUserDeletedSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUsersService.Setup(u => u.DeleteAsync(userId)).ReturnsAsync(userId);

            // Act
            var result = await _controller.Delete(userId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task GetUserInfo_ReturnsOk_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = User.Create(userId, "user", "user@example.com", "hashedPassword").Value;
            _mockUsersService.Setup(u => u.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUserInfo(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<UserResponse>(okResult.Value);
            Assert.Equal(userId, response.Id);
        }
    }
}
