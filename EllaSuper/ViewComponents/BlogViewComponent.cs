using Ella.DAL.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EllaSuper.ViewComponents
{
    public class BlogViewComponent : ViewComponent
    {
        private readonly AppDbContext _AppDbContext;
        public BlogViewComponent(AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blog = await _AppDbContext.Blogs.Where(e => !e.IsDeleted).ToListAsync();
            return View(blog);
        }

    }
}
