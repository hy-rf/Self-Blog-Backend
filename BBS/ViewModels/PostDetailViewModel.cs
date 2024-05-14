﻿using BBS.Models;

namespace BBS.ViewModels
{
    public class PostDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<Tag> Tags { get; set; }
        public int Likes { get; set; }
        public int Replies { get; set; }
    }
}
