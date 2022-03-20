using CastingProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CastingProject.Components
{
    public class ArtistViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext context;

        public ArtistViewComponent(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var artists = await context.Artists.Take(4).OrderByDescending(x => x.Id).ToListAsync();
                return View(artists);
        }
    }
}
