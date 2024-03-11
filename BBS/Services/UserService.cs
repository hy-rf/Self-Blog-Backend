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
        public int GetUserId()
        {
            return this.UserId;
        }
        public bool Signup(string username, string password)
        {
            if (CheckSignup(username))
            {
                var newUser = new User
                {
                    Name = username,
                    Pwd = password,
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
        public bool CheckSignup(string username)
        {
            var Used = ctx.User.Any(u => u.Name == username);
            if (Used)
            {
                return false;
            }
            return true;
        }
        public bool Login(string username, string password)
        {
            var Matched = ctx.User.Where(u => u.Name == username && u.Pwd == password).Select(u => new { u.Id, u.Name }).ToList();
            if (Matched.Count != 0)
            {
                this.UserId = Matched[0].Id;
                var updateLastLogin = ctx.User.Single(u => u.Id == Matched[0].Id);
                updateLastLogin.LastLogin = DateTime.Now;
                ctx.SaveChanges();
                return true;
            }
            return false;
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
