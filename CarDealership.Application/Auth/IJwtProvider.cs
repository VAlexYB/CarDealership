using CarDealership.Core.Models.Auth;

namespace CarDealership.Application.Auth
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
