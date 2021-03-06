using CastingProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CastingProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<ArtistGallery> ArtistGalleries { get; set; }
        public DbSet<Ethnicity> Ethnicities { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ArtistHobbies> ArtistHobbies { get; set; }
        public DbSet<ArtistRole> ArtistRoles { get; set; }
        public DbSet<Category> Categories{ get; set; }
        public DbSet<ArtistSkills> ArtistSkills{ get; set; }
        public DbSet<Blogs> Blogs{ get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ArtistRole>().HasKey(x => new { x.ArtistId, x.RoleId });
            builder.Entity<ArtistHobbies>().HasKey(x => new { x.ArtistId, x.HobbyId });
            builder.Entity<ArtistSkills>().HasKey(x => new { x.ArtistId, x.SkillId });
        }
    }
}