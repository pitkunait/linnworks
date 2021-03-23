using System.Threading.Tasks;

namespace Server.Repositories.Users
{
    public interface IUserRepository
    {
        Task CreateAsync(string user, string pass);
        Task<User> FindAsync(string username);
    }
}