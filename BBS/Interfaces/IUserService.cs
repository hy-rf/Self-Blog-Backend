using BBS.Models;
using System.Diagnostics.Eventing.Reader;

namespace BBS.Interfaces
{
    public interface IUserService
    {
        public int GetUserId();
        public bool Signup(string username, string password);
        public bool Login(string username, string password);
        public User GetUser(int Id);
        public bool EditAvatar(int Id, string avatar);
        public bool EditName(int Id, string name);
    }
}
