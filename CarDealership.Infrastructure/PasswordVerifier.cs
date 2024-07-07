using CarDealership.Application.Auth;

namespace CarDealership.Infrastructure
{
    public class PasswordVerifier : IPasswordVerifier
    {
        public bool Verify(string password, string hashPassword) =>
            BCrypt.Net.BCrypt.EnhancedVerify(password, hashPassword);
    }
}
