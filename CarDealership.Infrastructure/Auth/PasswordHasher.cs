using CarDealership.Application.Auth;

namespace CarDealership.Infrastructure.Auth
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Generate(string password) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }
}
