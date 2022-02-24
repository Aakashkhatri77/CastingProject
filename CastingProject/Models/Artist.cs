﻿using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CastingProject.Models
{
    public class Artist : EnumField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EthnicityId { get; set; }
        public Ethnicity Ethnicity { get; set; }
        public Gender? Gender { get; set; }
        public decimal? Height { get; set; }
        public int Weight { get; set; }
        public SkinColor? Skin_Color { get; set; }
        public EyeColor Eye_Color { get; set; }
        public HairColor Hair_Color { get; set; }
        public SkinType Skin_Type { get; set; }
        public HairLength Hair_Length { get; set; }
        public HairType Hair_Type { get; set; }
        public string Experience { get; set; }
        public string Wages { get; set; }
        public DateTime DOB { get; set; }
        public string Dp { get; set; }


        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [NotMapped]
        public List<IFormFile> GalleryFile { get; set; }

        public ICollection<ArtistGallery> ArtistGalleries { get; set; } = new HashSet<ArtistGallery>();

        [NotMapped]
        public string hobbies { get; set; }
        public ICollection<Hobby> Hobbies { get; set; } = new HashSet<Hobby>();
        [NotMapped]
        public string skills { get; set; }
        public ICollection<Skill> Skills { get; set; } = new HashSet<Skill>();
        public ICollection<ArtistRole> ArtistRoles { get; set; } = new HashSet<ArtistRole>();
    }
}
