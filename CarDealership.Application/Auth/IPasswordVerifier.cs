using BCrypt.Net;

namespace CarDealership.Application.Auth
{
    public interface IPasswordVerifier
    {
        bool Verify(string password, string hashPassword);
    }

    public class PasswordVerifier : IPasswordVerifier
    {
        public bool Verify(string password, string hashPassword) =>
            BCrypt.Net.BCrypt.EnhancedVerify(password, hashPassword);
    }
}
