using Ella.DAL.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EllaSuper.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _AppDbContext;
        public ProductController(AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var product = await _AppDbContext.Product.ToListAsync();
            return View(product);
        }
    }
}
