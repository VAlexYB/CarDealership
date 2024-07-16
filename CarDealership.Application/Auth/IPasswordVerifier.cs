namespace CarDealership.Application.Auth
{
    public interface IPasswordVerifier
    {
        bool Verify(string password, string hashPassword);
    }
}
