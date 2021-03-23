using System.Threading.Tasks;
using Dapper;

namespace LinnworksTechTest.Repositories.Users
{
    public class UserRepository : SqlConnectionProvider
    {

        public UserRepository(string connectionString) : base(connectionString)
        {
        }
        
        public async Task CreateAsync(string user, string pass)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = @"INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
                await conn.ExecuteAsync(sql, new {Username = user, Password = pass});
            }
        }

        public async Task<User> FindAsync(string username)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "SELECT * FROM Users WHERE Username = @Username";
                return await conn.QueryFirstOrDefaultAsync<User>(sql, new {Username = username});
            }
        }
    }
}