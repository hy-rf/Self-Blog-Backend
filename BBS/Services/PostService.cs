using BBS.Data;
using BBS.IRepository;
using BBS.IService;
using BBS.Models;
using BBS.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BBS.Services
{
    public class PostService(AppDbContext ctx, ITagService tagService, IPostRepository postRepository, ITagRepository tagRepository, IPostTagRepository postTagRepository) : IPostService
    {
        public int CountPost()
        {
            return ctx.Post.Count();
        }

        public Task<bool> CreatePost(string Title, string Content, string Tag, int UserId, out int Id)
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
                    if (!tagRepository.IsExist(t => t.Name == tag).Result)
                    {
                        tagRepository.CreateAsync(new Tag { Name = tag });
                    }
                    var posttag = new PostTag { TagId = tagRepository.GetTagByNameAsync(tag).Result.Id, PostId = postRepository.GetAllAsync().Result.Count + 1 };
                    postTagRepository.CreateAsync(posttag);
                }
            });
            _ = postRepository.CreateAsync(newPost);
            Id = newPost.Id;
            return Task.FromResult(true);
        }
        public bool EditPost(int Id, string Title, string Content, string Tag)
        {
            var EditPost = ctx.Post.Single(p => p.Id == Id);
            EditPost.Title = Title;
            EditPost.Content = Content;
            EditPost.Modified = DateTime.Now;
            postRepository.UpdateAsync(EditPost);
            //Compare between old posttags and new posttags then remove old posttag thats not in new posttags
            var oldtags = ctx.PostTag.Where(t => t.PostId == Id).Include(t => t.Tag);
            var newtags = Tag.Split("#").ToList();
            foreach (var t in oldtags)
            {
                if (!newtags.Contains(t.Tag.Name))
                {
                    postTagRepository.DeleteAsync(t.Id);
                }
            }
            //Compare between old posttags and new posttags then remove old posttag thats not in new posttags end
            foreach (var tag in newtags)
            {
                if (tag != "")
                {
                    if (!tagRepository.IsExist(t => t.Name == tag).Result)
                    {
                        tagRepository.CreateAsync(new Tag { Name = tag });
                    }
                    if (!ctx.PostTag.Any(pt => pt.TagId == ctx.Tag.Single(t => t.Name == tag).Id && pt.PostId == Id))
                    {
                        var posttag = new PostTag { TagId = tagRepository.GetTagByNameAsync(tag).Result.Id, PostId = Id };
                        postTagRepository.CreateAsync(posttag);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="NumPostPerPage"></param>
        /// <returns></returns>
        public List<Post> GetPostsByPage(int PageIndex, int NumPostPerPage)
        {
            return postRepository.GetPostsForPostList(PageIndex, NumPostPerPage).Result;
        }

        public List<Post> GetPostsLite()
        {
            return ctx.Post.Include(p => p.User).Select(p => new Post
            {
                Id = p.Id,
                Title = p.Title,
                Created = p.Created,
                Modified = p.Modified,
                UserId = p.UserId,
                User = new User
                {
                    Id = p.User.Id,
                    Name = p.User.Name,
                    Avatar = p.User.Avatar
                }
            }).ToList();
        }
    }
}
