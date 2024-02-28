using BBS.Models;

namespace BBS.Interfaces
{
    public interface IUserService
    {
        public bool Signup(string username, string password);
        public bool CheckSignup(string username);
        public bool Login(string username, string password);
        public bool VerifyPassword(string username);
        public User GetUser(int Id);
    }
}
