using System.ComponentModel.DataAnnotations.Schema;

namespace CastingProject.Models
{
    public class Blogs: EnumField
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string HighlightLine { get; set; }
        public PostStatus PostStatus { get; set; }
        public DateTime PublishDate { get; set; }
        public string Slug { get; set; }
        public string Picture { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
