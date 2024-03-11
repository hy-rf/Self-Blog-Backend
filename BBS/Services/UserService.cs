using BBS.Data;
using BBS.Interfaces;
using BBS.Models;
using Microsoft.Data.Sqlite;
using System.Data.SqlTypes;

namespace BBS.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext ctx;
        public int UserId { get; set; }
        public UserService(AppDbContext appDbContext)
        {
            ctx = appDbContext;
        }
        public bool Signup(string Name, string Pwd)
        {
            if (CheckSignup(Name))
            {
                var newUser = new User
                {
                    Name = Name,
                    Pwd = Pwd,
                    Created = DateTime.Now,
                    LastLogin = DateTime.Now,
                    Avatar = ""
                };
                ctx.User.Add(newUser);
                ctx.SaveChanges();
                return true;
            }
            return false;
        }
        public bool CheckSignup(string Name)
        {
            var Used = ctx.User.Any(u => u.Name == Name);
            if (Used)
            {
                return false;
            }
            return true;
        }
        public int Login(string Name, string Pwd)
        {
            var Matched = ctx.User.Where(u => u.Name == Name && u.Pwd == Pwd).Select(u => new { u.Id, u.Name }).ToList();
            if (Matched.Count != 0)
            {
                var updateLastLogin = ctx.User.Single(u => u.Id == Matched[0].Id);
                updateLastLogin.LastLogin = DateTime.Now;
                ctx.SaveChangesAsync();
            }
            return Matched[0].Id;
        }
        public User GetUser(int Id)
        {
            var User = ctx.User.Where(u => u.Id == Id).Select(u => new User
            {
                Id = u.Id,
                Name = u.Name,
                Created = u.Created,
                LastLogin = u.LastLogin,
                Avatar = u.Avatar
            }).ToList();
            return User[0];
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
    }
}
