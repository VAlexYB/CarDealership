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
            try
            {
                RoleEntity roleEntity = await _context.Roles.FindAsync(id);

                var role = roleEntity != null ? Role.Create(
                    roleEntity.Id,
                    roleEntity.Value
                ).Value : null;

                foreach (var userEntity in roleEntity.Users)
                {
                    var tempUser = User.Create(
                        userEntity.Id,
                        userEntity.UserName,
                        userEntity.Email,
                        userEntity.PasswordHash,
                        userEntity.FirstName,
                        userEntity.MiddleName,
                        userEntity.LastName,
                        userEntity.PhoneNumber,
                        userEntity.FirstCardDigits,
                        userEntity.LastCardDigits,
                        userEntity.IsDeleted
                    ).Value;
                    role.AddUser(tempUser);
                }
                return role;
            } 
            catch (Exception)
            {
                throw;
            }
           
        }
    }
}
