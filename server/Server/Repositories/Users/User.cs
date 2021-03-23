using Dapper.Contrib.Extensions;

namespace Server.Repositories.Users
{
    public class User
    {
        
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}