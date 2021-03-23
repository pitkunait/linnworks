using Dapper.Contrib.Extensions;

namespace LinnworksTechTest.Repositories.Users
{
    public class User
    {
        
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}