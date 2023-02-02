using Ella.DAL.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EllaSuper.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private readonly AppDbContext _AppDbContext;
        public ProductViewComponent(AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var product = await _AppDbContext.Product.ToListAsync();
            return View(product);
        }
    }
}
