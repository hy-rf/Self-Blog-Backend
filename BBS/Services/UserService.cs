﻿using BBS.Data;
using BBS.Interfaces;
using BBS.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;

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
                    Avatar = ""
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
        public bool Login(string Name, string Pwd, out int id)
        {
            try
            {
                var Matched = ctx.User.First(u => u.Name == Name && u.Pwd == Pwd);
                if (Matched != null)
                {
                    var updateLastLogin = ctx.User.Single(u => u.Id == Matched.Id);
                    updateLastLogin.LastLogin = DateTime.Now;
                    id = Matched.Id;
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch
            {
                id = -1;
                return false;
            }
            id = -1;
            return false;
        }
        public User GetUser(int Id)
        {
            var User = ctx.User.Include(u => u.Posts).Include(u => u.Replies).FirstOrDefault(u => u.Id == Id);
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
        public User GetUserLight(int Id)
        {
            var User = ctx.User.FirstOrDefault(u => u.Id == Id);
            return User;
        }
    }
}
