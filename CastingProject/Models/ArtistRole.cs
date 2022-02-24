namespace CastingProject.Models
{
    public class ArtistRole
    {
        public int ArtistId { get; set; }
        public int RoleId { get; set; }
        public Artist Artist { get; set; }
        public Role Role { get; set; }
    }
}
