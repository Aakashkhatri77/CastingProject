namespace CastingProject.Models
{
    public class TalentProfile
    {
        public int Id { get; set; }
        public string ProfileName { get; set; }
        public int TalentId { get; set; }
        public virtual Talent Talent{ get; set; }
    }
}
