using CarDealership.Application.Auth;
using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Exceptions;
using CarDealership.Core.Models.Auth;
using System.ComponentModel.DataAnnotations;

namespace CarDealership.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IPasswordVerifier _passwordVerifier;
        private readonly IJwtProvider _jwtProvider;
        public UsersService(IUsersRepository usersRepository, IRolesRepository rolesRepository, IPasswordVerifier passwordVerifier, IJwtProvider jwtProvider)
        { 
            _usersRepository = usersRepository;
            _rolesRepository = rolesRepository;
            _passwordVerifier = passwordVerifier;
            _jwtProvider = jwtProvider;
        }

        public async Task AddAsync(User user)
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

        public async Task AssignSenior(Guid id)
        {
            var user = await GetByIdAsync(id);
            if (!user.Roles.Any(ur => ur.Id == (int)Roles.Manager))
            {
                throw new ClientInformationException("Назначить старшим иожно только менеджера системы");
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
                throw new ClientInformationException("Это не старший менеджер");
            }
            var role = await _rolesRepository.GetByIdAsync((int)Roles.SeniorManager);
            user.RemoveRole(role);
            await _usersRepository.UpdateAsync(user);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _usersRepository.GetByEmailAsync(email);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _usersRepository.GetByIdAsync(id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _usersRepository.GetByUsernameAsync(username);
        }

        async Task<string> IUsersService.Login(string identifier, string password)
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

            if (user == null) throw new ClientInformationException("Проверьте правильность введенных данных.");

            var verified = _passwordVerifier.Verify(password, user.PasswordHash);

            if (!verified) throw new ClientInformationException("Не удалось авторизироваться в системе");

            var token = _jwtProvider.GenerateToken(user);
            return token;
        }

        public async Task<List<User>> GetUsersAsync(int? roleId = null)
        {
             return await _usersRepository.GetUsersAsync(roleId);
        }

        public async Task<Guid> DeleteAsync(Guid userId)
        {
                return await _usersRepository.DeleteAsync(userId);
        }
    }
}
