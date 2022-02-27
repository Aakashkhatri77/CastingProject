namespace CastingProject.Models
{
    public class ArtistHobbies
    {
        public int ArtistId { get; set; }
        public int HobbyId { get; set; }
        public Artist Artist{get; set;}
        public Hobby Hobby {get; set;}
    
    }
}
