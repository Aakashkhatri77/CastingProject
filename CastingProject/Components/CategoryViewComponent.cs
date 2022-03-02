using CastingProject.Data;
using Microsoft.AspNetCore.Mvc;

namespace CastingProject.Components
{
    public class CategoryViewComponent: ViewComponent
    {

        private readonly ApplicationDbContext context;

        public CategoryViewComponent(ApplicationDbContext context)
        {
            this.context = context;
        }

        private IViewComponentResult Invoke()
        {
            var categories = context.Categories.ToList();
            return View(categories);
        }
    }
}
