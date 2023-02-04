using Ella.DAL.DAL;
using EllaSuper.ViewModels;
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
            var products = await _AppDbContext.Product.Where(p => !p.IsDeleted).ToListAsync();
            var categories = await _AppDbContext.Categories.Where(c => !c.IsDeleted).ToListAsync();
            var sizes = await _AppDbContext.Sizes.Where(s => !s.IsDeleted).ToListAsync();
            var brands = await _AppDbContext.Brands.Where(b => !b.IsDeleted).ToListAsync();

            var viewModel = new ProductViewModel
            {
                Products = products,
                Categories = categories,
                Sizes = sizes,
                Brands = brands
            };

            return View(viewModel);
        }
    }
}
