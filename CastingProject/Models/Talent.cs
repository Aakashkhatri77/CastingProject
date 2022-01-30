using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CastingProject.Models
{
  

    public class Talent: EnumField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public string Nationality { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public SkinColor SkinColor { get; set; }
        public EyeColor EyeColor { get; set; }
        public HairColor HairColor { get; set; }
        public SkinType SkinType { get; set; }
        public HairLength HairLength { get; set; }
        public HairType HairType { get; set; }
        public string DOB { get; set; }
        public int Age { get; set; }
        [NotMapped]
        public List<IFormFile> ImageFile { get; set; }

        public ICollection<TalentProfile> talentProfiles { get; set; } = new HashSet<TalentProfile>();
        
        //public Talent()
        //{
        //    talentProfiles = new Collection<TalentProfile>();
        //}
    }
}
