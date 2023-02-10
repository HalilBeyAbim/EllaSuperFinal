using Ella.Core.Entity;
using Ella.DAL.DAL;
using EllaSuper.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Telegram.Bot;

namespace EllaSuper.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _dbcontext;
        private readonly UserManager<User> _userManager;

        public ContactController(AppDbContext dbcontext, UserManager<User> userManager)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
        }

        public async  Task<IActionResult> Index()
        {
            var model = new ContactViewModel();
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                model.ContactMessage = new ViewModels.ContactMessageViewModel
                {
                    Name = user.FirstName,
                    Email = user.Email 
                };
            }
            return View(model);
        }
        public async Task<IActionResult> AddMessage(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(viewName: nameof (Index), model);
            }
            var message = new ContactMessage
            {
                Name = model.ContactMessage.Name,
                Email = model.ContactMessage.Email,
                Subject = model.ContactMessage.Subject,
                Message = model.ContactMessage.Message
            };
            await _dbcontext.ContactMessages.AddAsync(message);

            await _dbcontext.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

    }
   
}
