using CarDealership.Core.Models.Auth;

namespace CarDealership.Core.Abstractions.Services
{
    public interface IUsersService
    {
        Task AddAsync(User user);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByIdAsync(Guid id);
        Task<string> Login(string identifier, string password);
        Task AssignSenior(Guid id);
        Task SuspendSenior(Guid id);
        Task<List<User>> GetUsersAsync(int? roleId = null);
        Task<Guid> DeleteAsync(Guid userId);
    }
}
