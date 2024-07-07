namespace CarDealership.Application.Auth
{
    public interface IPasswordHasher
    {
        string Generate(string password);
    }
}
