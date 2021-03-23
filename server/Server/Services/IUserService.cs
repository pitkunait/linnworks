using System.Threading.Tasks;

namespace Server.Services
{
    public interface IUserService
    {
        Task<bool> IsAnExistingUser(string userName);
        Task<bool> IsValidUserCredentials(string userName, string password);
        Task<string> GetUserRole(string userName);
        Task CreateUser(string user, string pass);
    }
}