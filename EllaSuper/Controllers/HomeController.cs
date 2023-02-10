using Ella.DAL.DAL;
using EllaSuper.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EllaSuperFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _Dbcontext;
        public HomeController(AppDbContext Dbcontext)
        {
            _Dbcontext = Dbcontext;
        }
        public IActionResult Index()
        {
            var Products = _Dbcontext.Product.ToList();
            var Blogs = _Dbcontext.Blogs.ToList();
            var Abouts = _Dbcontext.Abouts.ToList();
            var Galleries = _Dbcontext.Galleries.ToList();
            var Banner = _Dbcontext.Banners.FirstOrDefault  ();
            HomeViewModel homeViewModel = new HomeViewModel
            {
                Products = Products,
                Blogs = Blogs,
                Abouts = Abouts,
                Galleries = Galleries,
                Banner = Banner
            };
            return View(homeViewModel);
        }
    }
}
