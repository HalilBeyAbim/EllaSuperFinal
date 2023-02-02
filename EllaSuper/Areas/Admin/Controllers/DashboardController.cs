using Microsoft.AspNetCore.Mvc;

namespace EllaSuperFinal.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
