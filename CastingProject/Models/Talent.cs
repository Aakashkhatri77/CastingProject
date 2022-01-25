using System.ComponentModel.DataAnnotations.Schema;

namespace CastingProject.Models
{
    public class Talent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Profile { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
