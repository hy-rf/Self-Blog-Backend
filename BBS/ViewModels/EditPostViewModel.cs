namespace BBS.ViewModels
{
    public class EditPostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<CreateTagViewModel> NewTags { get; set; }
    }
}
