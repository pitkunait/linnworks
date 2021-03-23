using System.Collections.Generic;
using System.Threading.Tasks;
using LinnworksTechTest.Controllers;
using LinnworksTechTest.Repositories.Users;
using Microsoft.Extensions.Logging;

namespace LinnworksTechTest.Services
{
    public interface IUserService
    {
        Task<bool> IsAnExistingUser(string userName);
        Task<bool> IsValidUserCredentials(string userName, string password);
        Task<string> GetUserRole(string userName);
        Task CreateUser(string user, string pass);
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly UserRepository _userRepository;

        public UserService(ILogger<UserService> logger, UserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<bool> IsValidUserCredentials(string userName, string password)
        {
            _logger.LogInformation($"Validating user [{userName}]");
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var user = await _userRepository.FindAsync(userName);
            return user != null && user.Password == password;
        }

        public async Task<bool> IsAnExistingUser(string userName)
        {
            var user = await _userRepository.FindAsync(userName);
            return user != null;
        }
        
        public async Task CreateUser(string user, string pass)
        {
            await _userRepository.CreateAsync(user, pass);
        }
        
        public async Task<string> GetUserRole(string userName)
        {
            
            if (!await IsAnExistingUser(userName))
            {
                return string.Empty;
            }

            if (userName == "admin")
            {
                return UserRoles.Admin;
            }

            return UserRoles.BasicUser;
        }
    }

    public static class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string BasicUser = nameof(BasicUser);
    }
}