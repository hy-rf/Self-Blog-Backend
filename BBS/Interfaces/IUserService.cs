using BBS.Models;

namespace BBS.Interfaces
{
    public interface IUserService
    {
        public bool CheckDuplicatedName(string Name);
        public bool Signup(string Name, string Pwd);
        public bool Login(string Name, string Pwd, out int id);
        public User GetUser(int Id);
        public bool EditAvatar(int Id, string Avatar);
        public User GetUserLight(int Id);
        public bool EditName(int Id, string Name);
    }
}
