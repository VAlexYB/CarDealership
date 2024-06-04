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

        public UsersController(IUsersService usersService, IRolesService rolesService, IPasswordHasher passwordHasher)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _rolesService = rolesService ?? throw new ArgumentNullException(nameof(rolesService));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
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

                var role = await _rolesService.GetByIdAsync((int)Roles.Admin);
                user.AddRole(role);


                await _usersService.AddAsync(user);
                return Ok();
            } 
            catch (InvalidOperationException e) 
            {
                return StatusCode(404, e.Message);
            }
            catch (Exception)
            {
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
            } catch (InvalidOperationException e)
            {
                return StatusCode(404, e.Message);
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
                return StatusCode(404, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "Admin")]
        [Route("suspendSenior/{mgrId}")]
        [HttpGet]
        public async Task<IActionResult> SuspendSenior(Guid mgrId)
        {
            await _usersService.SuspendSenior(mgrId);
            return Ok();
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
                return StatusCode(404, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete("altertroublesuckykey");
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("getMgrs")]
        [HttpGet]
        public async Task<IActionResult> GetAllMgrs()
        {
            var users = await _usersService.GetUsersAsync((int)Roles.Manager);
            var response = users.Select(u => new UserResponse(u.Id)
            {
                UserName = u.UserName,
                Email = u.Email,
                FirstName = u.FirstName,
                MiddleName = u.MiddleName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                CardDigits = $"{u.FirstCardDigits}########{u.LastCardDigits}"
            }).ToList();
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [Route("getUsers")]
        [HttpGet]
        public async Task<IActionResult> GetOnlyUsers()
        {
            var users = await _usersService.GetUsersAsync((int)Roles.User);
            var response = users.Select(u => new UserResponse(u.Id)
            {
                UserName = u.UserName,
                Email = u.Email,
                FirstName = u.FirstName,
                MiddleName = u.MiddleName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                CardDigits = $"{u.FirstCardDigits}########{u.LastCardDigits}"
            }).ToList();
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [Route("getAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
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
                CardDigits = $"{u.FirstCardDigits}########{u.LastCardDigits}"
            }).ToList();
            return Ok(response);
        }


        [Authorize(Roles = "Admin")]
        [Route("delete/{userId}")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid userId)
        {
            await _usersService.DeleteAsync(userId);
            return Ok();
        }
    }
}
