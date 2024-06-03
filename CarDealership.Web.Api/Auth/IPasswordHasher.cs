namespace CarDealership.Web.Api.Auth
{
    public interface IPasswordHasher
    {
        string Generate(string password);
    }

    public class PasswordHasher : IPasswordHasher
    {
        public string Generate(string password) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }
}
