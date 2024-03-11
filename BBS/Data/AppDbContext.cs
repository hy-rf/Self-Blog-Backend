using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BBS.Models;

namespace BBS.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(){
        
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Reply> Replies { get; set; }


}