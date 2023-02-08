using Microsoft.AspNetCore.Mvc;

namespace EllaSuper.Controllers
{
    public class ErrorPageController : Controller
    {
        public IActionResult Error1(int code)
        {
            return View();
        }
    }
}
