namespace CastingProject.Models
{
    public class ArtistSkills
    {
        public int ArtistId { get; set; }
        public int SkillId { get; set; }
        public Artist Artist { get; set; }
        public Skill Skill { get; set; }
    }
}
