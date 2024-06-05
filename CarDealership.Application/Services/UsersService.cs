using CarDealership.Application.Auth;
using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;

namespace CarDealership.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IPasswordVerifier _passwordVerifier;
        private readonly IJwtProvider _jwtProvider;
        public UsersService(IUsersRepository usersRepository, IRolesRepository rolesRepository, IPasswordVerifier passwordVerifier, JwtProvider jwtProvider)
        { 
            _usersRepository = usersRepository;
            _rolesRepository = rolesRepository;
            _passwordVerifier = passwordVerifier;
            _jwtProvider = jwtProvider;
        }

        public async Task AddAsync(User user)
        {
            try
            {
                bool _exist = await _usersRepository.ExistsAsync(user.Id);
                if(_exist)
                {
                    await _usersRepository.UpdateAsync(user);
                }
                else
                {
                    await _usersRepository.AddAsync(user);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AssignSenior(Guid id)
        {
            try
            {
                var user = await GetByIdAsync(id);
                if (!user.Roles.Any(ur => ur.Id == (int)Roles.Manager))
                {
                    throw new InvalidOperationException("Назначить старшим иожно только менеджера системы");
                }

                var role = await _rolesRepository.GetByIdAsync((int)Roles.SeniorManager);
                user.AddRole(role);
                await _usersRepository.UpdateAsync(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SuspendSenior(Guid id)
        {
            try
            {
                var user = await GetByIdAsync(id);
                if (!user.Roles.Any(ur => ur.Id == (int)Roles.SeniorManager))
                {
                    throw new InvalidOperationException("Это не старший менеджер");
                }
                var role = await _rolesRepository.GetByIdAsync((int)Roles.SeniorManager);
                user.RemoveRole(role);
                await _usersRepository.UpdateAsync(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            try
            {
                var user = await _usersRepository.GetByEmailAsync(email);
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            try
            {
                var user = await _usersRepository.GetByIdAsync(id);
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            try
            {
                var user = await _usersRepository.GetByUsernameAsync(username);
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        async Task<string> IUsersService.Login(string identifier, string password)
        {
            try
            {
                bool isEmail = new EmailAddressAttribute().IsValid(identifier);

                User user = null;
                if (isEmail)
                {
                    user = await _usersRepository.GetByEmailAsync(identifier);
                }
                else
                {
                    user = await _usersRepository.GetByUsernameAsync(identifier);
                }

                if (user == null) throw new InvalidOperationException("Login -> пользователь ввел несуществующую email/username");

                var verified = _passwordVerifier.Verify(password, user.PasswordHash);

                if (!verified) throw new InvalidOperationException("Не удалось авторизироваться в системе");

                var token = _jwtProvider.GenerateToken(user);
                return token;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<User>> GetUsersAsync(int? roleId = null)
        {
            try
            {
                var users = await _usersRepository.GetUsersAsync(roleId);
                return users;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Guid> DeleteAsync(Guid userId)
        {
            try
            {
                var id = await _usersRepository.DeleteAsync(userId);
                return id;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
