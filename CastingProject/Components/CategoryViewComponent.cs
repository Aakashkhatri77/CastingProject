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

        public async Task<IViewComponentResult> InvokeAsync(string selectedcategory)
        {
            ViewBag.category = await context.Categories.ToListAsync();
            ViewBag.SelectedCategory = selectedcategory;
            return View();
        }
    }
}
