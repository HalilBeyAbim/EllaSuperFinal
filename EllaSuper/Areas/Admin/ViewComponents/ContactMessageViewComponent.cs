using Ella.DAL.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EllaSuper.Areas.Admin.ViewComponents
{
    public class ContactMessageViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbcontext;

        public ContactMessageViewComponent(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var messages = await _dbcontext.ContactMessages.ToListAsync();
            return View(messages);
        }
    }
}
