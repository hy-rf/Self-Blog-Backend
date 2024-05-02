using BBS.Models;
using Microsoft.EntityFrameworkCore;

namespace BBS.Data
{
    public class ForumContext : DbContext
    {
        private readonly IConfiguration configuration;
        public ForumContext(DbContextOptions<ForumContext> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(configuration.GetConnectionString("AzureDB"));
        //}
        public DbSet<User> User { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Reply> Reply { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<PostTag> PostTag { get; set; }
        public DbSet<Friend> Friend { get; set; }
        public DbSet<FriendRequest> FriendRequest { get; set; }
        public DbSet<ChatRoom> ChatRoom { get; set; }
        public DbSet<ChatRoomMember> ChatRoomMember { get; set; }
        public DbSet<ChatRoomMessage> ChatRoomMessage { get; set; }
        public DbSet<LikedPost> LikedPost { get; set; }
        public DbSet<LikedReply> LikedReply { get; set; }
        public DbSet<Notification> Notification { get; set; }
    }
}
