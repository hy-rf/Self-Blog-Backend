using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BBS.Models;

namespace BBS.Data;

public class AppDbContext : DbContext
{
    private readonly string ConnectionString;
    public AppDbContext(IConfiguration configuration){
        ConnectionString = configuration.GetConnectionString("LocalDB");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder.UseSqlite(ConnectionString));
    }
    public DbSet<User> User { get; set; }
    public DbSet<Post> Post { get; set; }
    public DbSet<Reply> Reply { get; set; }


}