using BBS.Models;

namespace BBS.Interfaces
{
    public interface IUserService
    {
        public bool CheckDuplicatedName(string Name);
        public bool Signup(string Name, string Pwd);
        public bool Login(string Name, string Pwd, out int id);
        public User GetUser(int Id);
        public User GetUserBasic(int Id);
        public bool EditAvatar(int Id, string Avatar);
        public bool EditName(int Id, string Name);
        public bool Logoff(int Id);
    }
}
