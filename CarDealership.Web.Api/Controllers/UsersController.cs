using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models.Auth;
using CarDealership.Web.Api.Auth;
using CarDealership.Web.Api.Contracts.Requests;
using CarDealership.Web.Api.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CarDealership.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IRolesService _rolesService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger _logger;

        public UsersController(IUsersService usersService, IRolesService rolesService, IPasswordHasher passwordHasher, ILogger logger)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _rolesService = rolesService ?? throw new ArgumentNullException(nameof(rolesService));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _logger = logger;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            try
            {
                var hashedPassword = _passwordHasher.Generate(request.Password);

                var userCreateResult = CarDealership.Core.Models.Auth.User.Create(
                    request.Id,
                    request.UserName,
                    request.Email,
                    hashedPassword,
                    request.FirstName,
                    request.MiddleName,
                    request.LastName,
                    request.PhoneNumber,
                    request.FirstCardDigits,
                    request.LastCardDigits
                );


                if (userCreateResult.IsFailure)
                {
                    return BadRequest(userCreateResult.Error);
                }

                var user = userCreateResult.Value;

                var role = await _rolesService.GetByIdAsync((int)Roles.User);
                user.AddRole(role);


                await _usersService.AddAsync(user);
                return Ok();
            } 
            catch (InvalidOperationException e) 
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка возникла в UsersController ->  Register()");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
           
        }


        [Authorize(Roles = "Admin")]
        [Route("addmgr")]
        [HttpPost]
        public async Task<IActionResult> AddManager([FromBody] RegistrationRequest request)
        {
            try
            {
                var hashedPassword = _passwordHasher.Generate(request.Password);

                var userCreateResult = CarDealership.Core.Models.Auth.User.Create(
                    request.Id,
                    request.UserName,
                    request.Email,
                    hashedPassword,
                    request.FirstName,
                    request.MiddleName,
                    request.LastName,
                    request.PhoneNumber,
                    request.FirstCardDigits,
                    request.LastCardDigits
                );


                if (userCreateResult.IsFailure)
                {
                    return BadRequest(userCreateResult.Error);
                }

                var user = userCreateResult.Value;

                var role = await _rolesService.GetByIdAsync((int)Roles.Manager);
                user.AddRole(role);


                await _usersService.AddAsync(user);
                return Ok();
            } 
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка возникла в UsersController ->  AddManager()");
                return StatusCode(500, "Внутренняя ошибка");
            }
        }

        [Authorize(Roles = "Admin")]
        [Route("assignSenior/{mgrId}")]
        [HttpGet]
        public async Task<IActionResult> AssignSenior(Guid mgrId)
        {
            try
            {
                await _usersService.AssignSenior(mgrId);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка возникла в UsersController ->  AssignSenior()");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "Admin")]
        [Route("suspendSenior/{mgrId}")]
        [HttpGet]
        public async Task<IActionResult> SuspendSenior(Guid mgrId)
        {
            try
            {
                await _usersService.SuspendSenior(mgrId);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка возникла в UsersController ->  SuspendSenior()");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _usersService.Login(request.Identifier, request.Password);
                HttpContext.Response.Cookies.Append("altertroublesuckykey", token);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка возникла в UsersController ->  Login()");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                HttpContext.Response.Cookies.Delete("altertroublesuckykey");
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка возникла в UsersController ->  Logout()");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "Admin")]
        [Route("getMgrs")]
        [HttpGet]
        public async Task<IActionResult> GetAllMgrs()
        {
            try
            {
                var users = await _usersService.GetUsersAsync((int)Roles.Manager);
                var response = users.Select(u => new UserResponse(u.Id)
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName ?? "",
                    MiddleName = u.MiddleName ?? "",
                    LastName = u.LastName ?? "",
                    PhoneNumber = u.PhoneNumber ?? "",
                    CardDigits = u.FirstCardDigits != null && u.FirstCardDigits != null ? $"{u.FirstCardDigits}########{u.LastCardDigits}" : ""
                }).ToList();
                return Ok(response);
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка возникла в UsersController -> GetAllMgrs()");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "Admin")]
        [Route("getUsers")]
        [HttpGet]
        public async Task<IActionResult> GetOnlyUsers()
        {
            try
            {
                var users = await _usersService.GetUsersAsync((int)Roles.User);
                var response = users.Select(u => new UserResponse(u.Id)
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName ?? "",
                    MiddleName = u.MiddleName ?? "",
                    LastName = u.LastName ?? "",
                    PhoneNumber = u.PhoneNumber ?? "",
                    CardDigits = u.FirstCardDigits != null && u.FirstCardDigits != null ? $"{u.FirstCardDigits}########{u.LastCardDigits}" : ""
                }).ToList();
                return Ok(response);
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка возникла в UsersController ->  GetOnlyUsers()");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "Admin")]
        [Route("getAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _usersService.GetUsersAsync();
                var response = users.Select(u => new UserResponse(u.Id)
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    MiddleName = u.MiddleName,
                    LastName = u.LastName,
                    PhoneNumber = u.PhoneNumber,
                    CardDigits = u.FirstCardDigits != null && u.FirstCardDigits != null ? $"{u.FirstCardDigits}########{u.LastCardDigits}" : ""
                }).ToList();
                return Ok(response);
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка возникла в UsersController ->  GetAll()");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }


        [Authorize(Roles = "Admin")]
        [Route("delete/{userId}")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid userId)
        {
            try
            {
                await _usersService.DeleteAsync(userId);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка возникла в UsersController ->  Delete()");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }


        [Route("getById/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetUserInfo(Guid userId)
        {
            try
            {
                var user = await _usersService.GetByIdAsync(userId);
                var response = new UserResponse(user.Id)
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    CardDigits = user.FirstCardDigits != null && user.FirstCardDigits != null ? $"{user.FirstCardDigits}########{user.LastCardDigits}" : ""
                };
                return Ok(response);
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка возникла в UsersController ->  GetUserInfo()");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}
