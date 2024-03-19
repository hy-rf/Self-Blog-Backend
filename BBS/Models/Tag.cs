using System.ComponentModel.DataAnnotations;

namespace BBS.Models
{
    public class Tag
    {
        [Key]
        public string Name { get; set; }
        public int PostId { get; set; }
    }
}
