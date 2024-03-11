using BBS.Models;
using System.Diagnostics.Eventing.Reader;

namespace BBS.Interfaces
{
    public interface IUserService
    {
        public bool Signup(string Name, string Pwd);
        public bool Login(string Name, string Pwd, out int id);
        public User GetUser(int Id);
        public bool EditAvatar(int Id, string Avatar);
        public bool EditName(int Id, string Name);
    }
}
