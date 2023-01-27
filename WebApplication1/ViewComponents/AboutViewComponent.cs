using Ella.DAL.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EllaSuper.ViewComponents
{
    public class AboutViewComponent : ViewComponent
    {
        private readonly AppDbContext _AppDbContext;
        public AboutViewComponent(AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var about = await _AppDbContext.Abouts.FirstOrDefaultAsync();
            return View(about);
        }
    }
}
