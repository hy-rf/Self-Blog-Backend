using BBS.Models;

namespace BBS.Interfaces
{
    public interface IUserService
    {
        public bool Signup(string username, string password);
        public bool Login(string username, string password);
        public User GetUser(int Id);
    }
}
