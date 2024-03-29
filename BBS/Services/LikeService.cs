using BBS.Data;
using BBS.Interfaces;
using BBS.Models;

namespace BBS.Services
{
    public class LikeService(AppDbContext ctx) : ILikeService
    {
        public void AddLikePost(LikedPost likedPost)
        {
            // Check if the user has already liked the post
            if (ctx.LikedPost.Any(lp => lp.UserId == likedPost.UserId && lp.PostId == likedPost.PostId))
            {
                return;
            }
            ctx.LikedPost.Add(likedPost);
            ctx.SaveChanges();
        }

        public void AddLikeReply(LikedReply likedReply)
        {
            ctx.LikedReply.Add(likedReply);
            ctx.SaveChanges();
        }

        public List<LikedPost> likedPosts(int userId)
        {
            return ctx.LikedPost.Where(lp => lp.UserId == userId).ToList();
        }

        public List<LikedReply> likedReplies(int userId)
        {
            return ctx.LikedReply.Where(lr => lr.UserId == userId).ToList();
        }

        public void RemoveLikePost(LikedPost likedPost)
        {
            ctx.LikedPost.Remove(likedPost);
        }

        public void RemoveLikeReply(LikedReply likedReply)
        {
            ctx.LikedReply.Remove(likedReply);
        }
    }
}
