using Ella.DAL.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EllaSuper.ViewComponents
{
    public class GalleryViewComponent : ViewComponent
    {
        private readonly AppDbContext _AppDbContext;
        public GalleryViewComponent(AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var gallery = await _AppDbContext.Galleries.FirstOrDefaultAsync();
            return View(gallery);
        }
    }
}
