using CastingProject.Data;
using Microsoft.AspNetCore.Mvc;

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
            var artists = context.Artists.ToList();
            return View(artists);
        }
    }
}
