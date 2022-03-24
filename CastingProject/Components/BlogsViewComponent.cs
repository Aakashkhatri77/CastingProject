using CastingProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CastingProject.Components
{
    public class BlogsViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext context;

        public BlogsViewComponent(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blogs =await context.Blogs.Take(3).Where(x=>x.PostStatus!=Models.EnumField.PostStatus.Trash).OrderByDescending(x=>x.Id).ToListAsync();
            
            return View(blogs);
        }
    }
}
