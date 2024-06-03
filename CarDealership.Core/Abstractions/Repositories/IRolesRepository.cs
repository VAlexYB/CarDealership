using CarDealership.Core.Models.Auth;

namespace CarDealership.Core.Abstractions.Repositories
{
    public interface IRolesRepository
    {
        Task<Role> GetByIdAsync(int id);
    }
}
