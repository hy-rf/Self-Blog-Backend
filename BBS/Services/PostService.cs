using BBS.Data;
using BBS.Interfaces;
using BBS.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BBS.Services
{
    public class PostService(AppDbContext ctx, ITagService tagService) : IPostService
    {
        public int CountPost()
        {
            return ctx.Post.Count();
        }

        public bool CreatePost(string Title, string Content, string Tag, int UserId)
        {
            var newPost = new Post
            {
                UserId = UserId,
                Title = Title,
                Content = Content,
                Created = DateTime.Now,
                Modified = DateTime.Now
            };
            Tag.Split("#").ToList().ForEach(tag =>
            {
                if (tag != "")
                {
                    if (!ctx.Tag.Any(t => t.Name == tag))
                    {
                        tagService.AddTag(tag);
                    }
                    var posttag = new PostTag { TagId = ctx.Tag.Single(t => t.Name == tag).Id, PostId = ctx.Post.Count() + 1 };
                    ctx.PostTag.Add(posttag);
                }
            });
            ctx.Post.Add(newPost);
            ctx.SaveChanges();
            return true;
        }
        public bool EditPost(int Id, string Title, string Content, string Tag)
        {
            var EditPost = ctx.Post.Single(p => p.Id == Id);
            EditPost.Title = Title;
            EditPost.Content = Content;
            EditPost.Modified = DateTime.Now;
            //Compare between old posttags and new posttags then remove old posttag thats not in new posttags
            var oldtags = ctx.PostTag.Where(t => t.PostId == Id).Include(t => t.Tag);
            var newtags = Tag.Split("#").ToList();
            foreach (var t in oldtags)
            {
                if (!newtags.Contains(t.Tag.Name))
                {
                    ctx.PostTag.Remove(t);
                }
            }
            //Compare between old posttags and new posttags then remove old posttag thats not in new posttags end
            foreach (var tag in newtags)
            {
                if (tag != "")
                {
                    if (!ctx.Tag.Any(t => t.Name == tag))
                    {
                        tagService.AddTag(tag);
                    }
                    if (!ctx.PostTag.Any(pt => pt.TagId == ctx.Tag.Single(t => t.Name == tag).Id && pt.PostId == Id))
                    {
                        var posttag = new PostTag { TagId = ctx.Tag.Single(t => t.Name == tag).Id, PostId = Id };
                        ctx.PostTag.Add(posttag);
                    }
                }
            }
            ctx.SaveChanges();
            return true;
        }
        public Post GetPost(int Id)
        {
            var GetPost = ctx.Post.Include(p => p.PostTags)!.ThenInclude(pt => pt.Tag).Include(p => p.Replies)!.ThenInclude(r => r.User).Include(p => p.User).Include(p => p.Likes).ThenInclude(l => l.User).Single(p => p.Id == Id);
            return GetPost;
        }
        public List<Post> GetPosts()
        {
            var GetPosts = ctx.Post.Include(p => p.User).Include(p => p.Likes).ThenInclude(l => l.User).ToList();
            return GetPosts;
        }

        public List<Post> GetPostsByPage(int PageIndex, int NumPostPerPage)
        {
            var GetPosts = ctx.Post.Include(p => p.User).Include(p => p.Likes).ThenInclude(l => l.User).Include(p => p.Replies).OrderByDescending(p => p.Id).Skip((PageIndex - 1) * NumPostPerPage).Take(NumPostPerPage).ToList();
            return GetPosts;
        }

        public List<Post> GetPostsLite()
        {
            return ctx.Post.Include(p => p.User).Select(p => new Post{
                Id = p.Id,
                Title = p.Title,
                Created = p.Created,
                Modified = p.Modified,
                UserId = p.UserId,
                User = new User{
                    Id = p.User.Id,
                    Name = p.User.Name,
                    Avatar = p.User.Avatar
                }
            }).ToList();
        }
    }
}
