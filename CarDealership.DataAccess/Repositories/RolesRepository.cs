using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models.Auth;
using CarDealership.DataAccess.Entities.Auth;

namespace CarDealership.DataAccess.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly CarDealershipDbContext _context;
        public RolesRepository(CarDealershipDbContext context)
        {
            _context = context;
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            RoleEntity roleEntity =  await _context.Roles.FindAsync(id);

            var role = roleEntity != null ? Role.Create(
                roleEntity.Id,
                roleEntity.Value
            ).Value : null;

            return role;
        }
    }
}
