using BBS.Data;
using BBS.Interfaces;
using BBS.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BBS.Services
{
    public class PostService(AppDbContext ctx) : IPostService
    {
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
                        var newtag = new Tag
                        {
                            Name = tag
                        };
                        ctx.Tag.Add(newtag);
                        ctx.SaveChanges();
                    }
                    var posttag = new PostTag { TagId = ctx.Tag.Count() + 1, PostId = ctx.Post.Count() + 1 };
                    ctx.PostTag.Add(posttag);
                    ctx.SaveChanges();
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
                        var newtag = new Tag
                        {
                            Name = tag
                        };
                        ctx.Tag.Add(newtag);
                        ctx.SaveChanges();
                    }
                    if (!ctx.PostTag.Any(pt => pt.TagId == ctx.Tag.Single(t => t.Name == tag).Id))
                    {
                        var posttag = new PostTag { TagId = ctx.Tag.Single(t => t.Name == tag).Id, PostId = Id };
                        ctx.PostTag.Add(posttag);
                    }
                    ctx.SaveChanges();
                }
            }

            ctx.SaveChanges();
            return true;
        }
        public Post GetPost(int Id)
        {
            var GetPost = ctx.Post.Include(p => p.PostTags).ThenInclude(pt => pt.Tag).Include(p => p.Replies).ThenInclude(r => r.User).Include(p => p.User).Single(p => p.Id == Id);
            return GetPost;
        }
        public List<Post> GetPosts()
        {
            var GetPosts = ctx.Post.Include(p => p.User).ToList();
            return GetPosts;
        }
    }
}
