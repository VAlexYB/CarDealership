using CarDealership.Core.Models.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarDealership.Application.Auth
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }

    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;
        public JwtProvider(IOptions<JwtOptions> options) => _options = options.Value;

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString())
            };
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Value)));
           
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256
            );
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours)
            );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }

    public class JwtOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public int ExpiresHours { get; set; }
    }
}
