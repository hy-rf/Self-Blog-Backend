using BBS.Data;
using BBS.IRepository;
using BBS.IService;
using BBS.Models;
using Microsoft.EntityFrameworkCore;

namespace BBS.Services
{
    public class PostService(IPostRepository postRepository, ITagRepository tagRepository, IPostTagRepository postTagRepository) : IPostService
    {
        public int CountPost()
        {
            return postRepository.CountPost().Result;
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
        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Title"></param>
        /// <param name="Content"></param>
        /// <param name="Tag"></param>
        /// <returns></returns>
        public bool EditPost(int Id, string Title, string Content, string Tag)
        {
            var EditPost = postRepository.GetAsync(Id).Result;
            EditPost.Title = Title;
            EditPost.Content = Content;
            EditPost.Modified = DateTime.Now;
            postRepository.UpdateAsync(EditPost);
            //Compare between old posttags and new posttags then remove old posttag thats not in new posttags
            var oldtags = postTagRepository.GetPostTagsByPostId(Id).Result;
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
                    if (!postTagRepository.IsExist(pt => pt.TagId == tagRepository.GetTagByNameAsync(tag).Result.Id && pt.PostId == Id).Result)
                    {
                        var posttag = new PostTag { TagId = tagRepository.GetTagByNameAsync(tag).Result.Id, PostId = Id };
                        postTagRepository.CreateAsync(posttag);
                    }
                }
            }
            return true;
        }
        public async Task<Post> GetPost(int Id)
        {
            return await postRepository.GetPostById(Id);
        }
        #endregion
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
    }
}
