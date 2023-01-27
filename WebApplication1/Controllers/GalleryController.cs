using Ella.DAL.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EllaSuper.Controllers
{
    public class GalleryController : Controller
    {
        private readonly AppDbContext _AppDbContext;

        public GalleryController(AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var gallery = await _AppDbContext.Galleries.ToListAsync();
            return View(gallery);
        }
    }
}
