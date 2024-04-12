using BBS.Data;
using BBS.IService;
using BBS.Models;
using Microsoft.EntityFrameworkCore;

namespace BBS.Services
{
    public class UserService(AppDbContext ctx) : IUserService
    {
        public bool Signup(string Name, string Pwd)
        {
            if (CheckDuplicatedName(Name))
            {
                var newUser = new User
                {
                    Name = Name,
                    Pwd = Pwd,
                    Created = DateTime.Now,
                    LastLogin = DateTime.Now,
                    Avatar = "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",
                    LoggedIn = 1
                };
                ctx.User.Add(newUser);
                ctx.SaveChanges();
                return true;
            }
            return false;
        }
        public bool CheckDuplicatedName(string Name)
        {
            var Used = ctx.User.Any(u => u.Name == Name);
            if (Used)
            {
                return false;
            }
            return true;
        }
        public Task<bool> Login(string Name, string Pwd, out int id)
        {
            try
            {
                var Matched = ctx.User.First(u => u.Name == Name && u.Pwd == Pwd);
                if (Matched != null)
                {
                    var updateLastLogin = ctx.User.Single(u => u.Id == Matched.Id);
                    updateLastLogin.LastLogin = DateTime.Now;
                    updateLastLogin.LoggedIn = 1;
                    id = Matched.Id;
                    ctx.SaveChanges();
                    return Task.FromResult(true);
                }
            }
            catch
            {
                id = -1;
                return Task.FromResult(false);
            }
            id = -1;
            return Task.FromResult(false);
        }
        public User GetUser(int Id)
        {
            var User = ctx.User.Include(u => u.Posts).Include(u => u.Replies).Single(u => u.Id == Id);
            return User;
        }
        public bool EditAvatar(int Id, string Avatar)
        {
            var EditAvatar = ctx.User.Single(u => u.Id == Id);
            EditAvatar.Avatar = Avatar;
            ctx.SaveChanges();
            return true;
        }
        public bool EditName(int Id, string Name)
        {
            var EditName = ctx.User.Single(u => u.Id == Id);
            EditName.Name = Name;
            ctx.SaveChanges();
            return true;
        }
        public Task<bool> Logoff(int Id)
        {
            try
            {
                var logoff = ctx.User.Single(u => u.Id == Id);
                logoff.LoggedIn = 0;
                ctx.SaveChanges();
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public User GetUserBasic(int Id)
        {
            return ctx.User.Single(u => u.Id == Id);
        }
    }
}
