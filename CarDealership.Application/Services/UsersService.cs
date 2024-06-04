using CarDealership.Application.Auth;
using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace CarDealership.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IPasswordVerifier _passwordVerifier;
        private readonly IJwtProvider _jwtProvider;
        public UsersService(IUsersRepository usersRepository, IPasswordVerifier passwordVerifier, JwtProvider jwtProvider)
        { 
            _usersRepository = usersRepository;
            _passwordVerifier = passwordVerifier;
            _jwtProvider = jwtProvider;
        }

        public Task AddAsync(User user)
        {
            try
            {
                _usersRepository.AddAsync(user);
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AssignSenior(Guid id)
        {
            var user = await GetByIdAsync(id);
            if(!user.Roles.Any(ur => ur.Id == (int)Roles.Manager))
            {
                throw new InvalidOperationException("Назначить старшим иожно только менеджера системы");
            }
            var role = await _rolesRepository.GetByIdAsync((int)Roles.SeniorManager);
            user.AddRole(role);
            await _usersRepository.UpdateAsync(user);
        }

        public async Task SuspendSenior(Guid id)
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

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _usersRepository.GetByEmailAsync(email);
            return user;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var user = await _usersRepository.GetByIdAsync(id);
            return user;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var user = await _usersRepository.GetByUsernameAsync(username);
            return user;
        }

        async Task<string> IUsersService.Login(string identifier, string password)
        {
            bool isEmail = new EmailAddressAttribute().IsValid(identifier);

            User user = null;
            if(isEmail)
            {
                user = await _usersRepository.GetByEmailAsync(identifier);
            }
            else
            {
                user = await _usersRepository.GetByUsernameAsync(identifier);
            }

            if (user == null) throw new InvalidOperationException("Login -> пользователь ввел несуществующую email/username");

            var verified = _passwordVerifier.Verify(password, user.PasswordHash);

            if (!verified) throw new Exception("Не удалось авторизироваться в системе");

            var token = _jwtProvider.GenerateToken(user);
            return token;
        }

        public async Task<List<User>> GetUsersAsync(int? roleId = null)
        {
            var users = await _usersRepository.GetUsersAsync(roleId);
            return users;
        }

        public async Task<Guid> DeleteAsync(Guid userId)
        {
            var id = await _usersRepository.DeleteAsync(userId);
            return id;
        }

    }
}
