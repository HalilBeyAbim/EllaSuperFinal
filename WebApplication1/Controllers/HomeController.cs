using Microsoft.AspNetCore.Mvc;

namespace EllaSuperFinal.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
