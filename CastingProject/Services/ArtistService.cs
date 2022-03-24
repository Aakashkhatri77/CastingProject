using CastingProject.Data;
using CastingProject.Models;

namespace CastingProject.Services
{
    public class ArtistService
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment HostEnvironment;

        public ArtistService(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            this.context = context;
            HostEnvironment = hostEnvironment;
        }
    }
}
