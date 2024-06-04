using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models.Auth;
using CarDealership.DataAccess.Entities.Auth;
using CarDealership.DataAccess.Extensions;
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
            var existUserEmail = await _context.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .Where(u => !u.IsDeleted)
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existUserEmail != null)
            {
                throw new InvalidOperationException("В системе уже используется такая почта");
            }

            var existUserName = await _context.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .Where(u => !u.IsDeleted)
                .FirstOrDefaultAsync(u => u.UserName == user.UserName);

            if (existUserName != null)
            {
                throw new InvalidOperationException("В системе уже используется такой логин");
            }

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

            if (userEntity == null) throw new InvalidOperationException("Не найден пользователь с таким Email");

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

            if (userEntity == null) throw new InvalidOperationException("Не найден пользователь с таким Username");
                
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

            var rolesToRemove = existUser.Roles.Where(ur => !user.Roles.Any(r => r.Id == ur.Id)).ToList();
            foreach (var role in rolesToRemove)
            {
                existUser.Roles.Remove(role);
                role.Users.Remove(existUser);
            }

            await _context.SaveChangesAsync();
            return user.Id;
        }


        public async Task<List<User>> GetUsersAsync(int? roleId = null)
        {
            var entities = await _context.Users
                .AsNoTracking()
                .Where(u => !u.IsDeleted)
                .WhereIf(roleId.HasValue, u => u.Roles.Any(r => r.Id == roleId))
                .OrderBy(u => u.Id)
                .ToListAsync();

            var users = new List<User>();

            foreach (var userEntity in entities)
            {
                var tempUser = userEntity != null ? User.Create(
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

                if(tempUser != null)
                {
                    users.Add(tempUser);
                }
            }

            return users;
        }

        public async Task<Guid> DeleteAsync(Guid userId)
        {
            var userEntity = await _context.Users.FindAsync(userId);
            if (userEntity != null)
            {
                userEntity.IsDeleted = true;
            }
            await _context.SaveChangesAsync(); 
            return userId;
        }

        public async Task<bool> ExistsAsync(Guid userId)
        {
            return await _context
                .Users
                .AsNoTracking()
                .AnyAsync(u => u.Id == userId);
        }
    }
}
