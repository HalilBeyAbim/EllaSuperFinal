using Ella.BLL.Helpers;
using Ella.Core.Entity;
using EllaSuper.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EllaSuper.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            

            var existUser = await _userManager.FindByEmailAsync(model.UserName);
            if (existUser != null)
            {
                ModelState.AddModelError("", "This email is already exist");
                return View();
            }
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.UserName,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            var createdUser = await _userManager.FindByNameAsync(model.UserName);
           result = await _userManager.AddToRoleAsync(createdUser, Constants.UserRole);
            return RedirectToAction(nameof(Login));
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existUser = await _userManager.FindByNameAsync(model.UserName);

            if (existUser == null)
            {
                ModelState.AddModelError("", "This User is not exist");
                return View();
            }
            var signResult = await _signInManager.PasswordSignInAsync(existUser, model.Password, model.RememberMe, false);

            if (!signResult.Succeeded)
            {
                ModelState.AddModelError("", "Invalid Password");
                return View();
            }
            return RedirectToAction("Index", "Home");

        }
    
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
