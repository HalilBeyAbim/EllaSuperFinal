using Ella.DAL.DAL;
using EllaSuper.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EllaSuper.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _AppDbContext;
        public AboutController(AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var about = await _AppDbContext.Abouts.ToListAsync();
            var team = await _AppDbContext.Teams.ToListAsync();

            var model = new AboutViewModel
            {
                Abouts = about,
                Teams = team
            };

            return View(model);
        }
    }
}
