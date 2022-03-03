using CastingProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CastingProject.Components
{
    public class CategoryViewComponent: ViewComponent
    {

        private readonly ApplicationDbContext context;

        public CategoryViewComponent(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var category = await context.Categories.ToListAsync();
            ViewBag.Ethnicity = await context.Ethnicities.ToListAsync();
            return View(category);
        }
    }
}
