using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models.Auth;
using CarDealership.Web.Api.Auth;
using CarDealership.Web.Api.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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


        [Authorize(Roles = "Admin")]
        [Route("addmgr")]
        [HttpPost]
        public async Task<IActionResult> AddManager([FromBody] RegistrationRequest request)
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

        [Authorize(Roles = "Admin")]
        [Route("assignSenior/{mgrId}")]
        [HttpGet]
        public async Task<IActionResult> AssignSenior(Guid mgrId)
        {
            await _usersService.AssignSenior(mgrId);
            return Ok();
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _usersService.Login(request.Identifier, request.Password);
            HttpContext.Response.Cookies.Append("altertroublesuckykey", token);
            return Ok();
        }

        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete("altertroublesuckykey");
            return Ok();
        }
    }
}
