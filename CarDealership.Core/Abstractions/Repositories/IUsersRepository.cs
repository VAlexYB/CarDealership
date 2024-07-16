using CarDealership.Core.Models.Auth;

namespace CarDealership.Core.Abstractions.Repositories
{
    public interface IUsersRepository
    {
        Task AddAsync(User user);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByIdAsync(Guid id);
        Task<Guid> UpdateAsync(User user);
        Task<List<User>> GetUsersAsync(int? roleId = null);
        Task<Guid> DeleteAsync(Guid userId);
        Task<bool> ExistsAsync(Guid entityId);
    }
}
