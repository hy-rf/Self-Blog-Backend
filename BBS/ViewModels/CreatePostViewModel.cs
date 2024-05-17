namespace BBS.ViewModels
{
    public class CreatePostViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<CreateTagViewModel> NewTags { get; set; }
    }
}
