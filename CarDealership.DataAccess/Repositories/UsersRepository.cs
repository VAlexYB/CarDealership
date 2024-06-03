using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models.Auth;
using CarDealership.DataAccess.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.DataAccess.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly CarDealershipDbContext _context;

        public UsersRepository(CarDealershipDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(User user)
        {
            var userEntity = new UserEntity
            {
                Id = user.Id,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                Email = user.Email,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                FirstCardDigits = user.FirstCardDigits,
                LastCardDigits = user.LastCardDigits,
                HasLinkedCard = user.HasLinkedCard
            };

            foreach (var role in user.Roles)
            {
                var roleEntity = await _context.Roles.FindAsync(role.Id);
                if (roleEntity != null)
                {
                    userEntity.Roles.Add(roleEntity);
                    roleEntity.Users.Add(userEntity);
                }
            }

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .Where(u => !u.IsDeleted)
                .FirstOrDefaultAsync(u => u.Email == email);

            var user = userEntity != null ? User.Create(
                userEntity.Id,
                userEntity.UserName,
                userEntity.Email,
                userEntity.PasswordHash,
                userEntity.FirstName,
                userEntity.MiddleName,
                userEntity.LastName,
                userEntity.PhoneNumber,
                userEntity.FirstCardDigits,
                userEntity.LastCardDigits
            ).Value : null;

            foreach (var roleEntity in userEntity.Roles)
            {
                var role = Role.Create(roleEntity.Id, roleEntity.Value).Value;
                user.AddRole(role);
            }

            return user;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .Where(u => !u.IsDeleted)
                .FirstOrDefaultAsync(u => u.Id == id);

            var user = userEntity != null ? User.Create(
                userEntity.Id,
                userEntity.UserName,
                userEntity.Email,
                userEntity.PasswordHash,
                userEntity.FirstName,
                userEntity.MiddleName,
                userEntity.LastName,
                userEntity.PhoneNumber,
                userEntity.FirstCardDigits,
                userEntity.LastCardDigits
            ).Value : null;

            foreach (var roleEntity in userEntity.Roles)
            {
                var role = Role.Create(roleEntity.Id, roleEntity.Value).Value;
                user.AddRole(role);
            }
            return user;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .Where(u => !u.IsDeleted)
                .FirstOrDefaultAsync(u => u.UserName == username);

            var user = userEntity != null ? User.Create(
                userEntity.Id,
                userEntity.UserName,
                userEntity.Email,
                userEntity.PasswordHash,
                userEntity.FirstName,
                userEntity.MiddleName,
                userEntity.LastName,
                userEntity.PhoneNumber,
                userEntity.FirstCardDigits,
                userEntity.LastCardDigits
            ).Value : null;

            foreach (var roleEntity in userEntity.Roles)
            {
                var role = Role.Create(roleEntity.Id, roleEntity.Value).Value;
                user.AddRole(role);
            }
            return user;
        }

        public async Task<Guid> UpdateAsync(User user)
        {
            var existUser = await _context
                .Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existUser == null) throw new Exception("UsersRepository -> UpdateAsync");

            existUser.UserName = user.UserName;
            existUser.PasswordHash = user.PasswordHash;
            existUser.Email = user.Email;
            existUser.FirstName = user.FirstName;
            existUser.MiddleName = user.MiddleName;
            existUser.LastName = user.LastName;
            existUser.PhoneNumber = user.PhoneNumber;
            existUser.FirstCardDigits = user.FirstCardDigits;
            existUser.LastCardDigits = user.LastCardDigits;
            existUser.HasLinkedCard = user.HasLinkedCard;
            existUser.IsDeleted = user.IsDeleted;
            

            foreach (var role in user.Roles)
            {
                var roleEntity = await _context.Roles.FirstOrDefaultAsync(r => r.Id == role.Id);
                if (roleEntity != null && !existUser.Roles.Any(ur => ur.Id == role.Id))
                {
                    existUser.Roles.Add(roleEntity);
                    roleEntity.Users.Add(existUser);
                }
            }

            await _context.SaveChangesAsync();
            return user.Id;
        }
    }
}
