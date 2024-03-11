using BBS.Models;
using System.Diagnostics.Eventing.Reader;

namespace BBS.Interfaces
{
    public interface IUserService
    {
        public int GetUserId();
        public bool Signup(string Name, string Pwd);
        public void Login(string Name, string Pwd);
        public User GetUser(int Id);
        public bool EditAvatar(int Id, string Avatar);
        public bool EditName(int Id, string Name);
    }
}
