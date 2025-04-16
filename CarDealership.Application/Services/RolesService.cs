using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models.Auth;

namespace CarDealership.Application.Services
{
    public class RolesService : IRolesService
    {
        private readonly IRolesRepository _rolesRepository;

        public RolesService(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository ?? throw new ArgumentNullException(nameof(rolesRepository));
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            return await _rolesRepository.GetByIdAsync(id);
        }
    }
}
