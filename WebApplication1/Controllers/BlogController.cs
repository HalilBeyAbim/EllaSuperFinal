using Ella.DAL.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EllaSuper.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _AppDbContext;

        public BlogController(AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var blog = await _AppDbContext.Blogs.Where(e => !e.IsDeleted).ToListAsync();
            return View(blog);
        }
    }
}
