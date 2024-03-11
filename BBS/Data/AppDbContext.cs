using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BBS.Models;

namespace BBS.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){
    }
    public DbSet<User> User { get; set; }
    public DbSet<Post> Post { get; set; }
    public DbSet<Reply> Reply { get; set; }


}