using EllaSuperFinal.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EllaSuper.Areas.Admin.Controllers
{
    public class WishListController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
