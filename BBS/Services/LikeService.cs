using BBS.Data;
using BBS.IService;
using BBS.Models;
using Microsoft.EntityFrameworkCore;

namespace BBS.Services
{
    public class LikeService(ForumContext ctx) : ILikeService
    {
        public void AddLikePost(LikedPost likedPost)
        {
            // Check if the user has already liked the post
            if (!ctx.LikedPost.Any(lp => lp.UserId == likedPost.UserId && lp.PostId == likedPost.PostId))
            {
                ctx.LikedPost.Add(likedPost);
                ctx.SaveChanges();
                return;
            }
            RemoveLikePost(likedPost);
            ctx.SaveChanges();
        }

        public void AddLikeReply(LikedReply likedReply)
        {
            // Check if the user has already liked the reply
            if (!ctx.LikedReply.Any(lp => lp.UserId == likedReply.UserId && lp.ReplyId == likedReply.ReplyId))
            {
                ctx.LikedReply.Add(likedReply);
                ctx.SaveChanges();
                return;
            }
            RemoveLikeReply(likedReply);
            ctx.SaveChanges();
        }

        public List<LikedPost> likedPosts(int userId)
        {
            return ctx.LikedPost.Where(lp => lp.UserId == userId).Include(lp => lp.Post).Select(lp => new LikedPost
            {
                UserId = lp.UserId,
                PostId = lp.PostId,
                Post = new Post
                {
                    Title = lp.Post.Title,
                    Content = lp.Post.Content,
                    Created = lp.Post.Created,
                    Modified = lp.Post.Modified
                }
            }).ToList();
        }

        public List<LikedReply> likedReplies(int userId)
        {
            return ctx.LikedReply.Where(lr => lr.UserId == userId).Include(lr => lr.Reply).ToList();
        }

        public void RemoveLikePost(LikedPost likedPost)
        {
            var torm = ctx.LikedPost.Where(lp => lp.UserId == likedPost.UserId && lp.PostId == likedPost.PostId).Single();
            ctx.LikedPost.Remove(torm);
        }

        public void RemoveLikeReply(LikedReply likedReply)
        {
            var torm = ctx.LikedReply.Where(lr => lr.UserId == likedReply.UserId && lr.ReplyId == likedReply.ReplyId).Single();
            ctx.LikedReply.Remove(torm);
        }
    }
}
