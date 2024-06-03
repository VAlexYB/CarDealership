using CarDealership.Core.Models.Auth;

namespace CarDealership.Core.Abstractions.Services
{
    public interface IRolesService
    {
        Task<Role> GetByIdAsync(int id);
    }
}
