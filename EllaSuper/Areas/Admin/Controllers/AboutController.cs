using Ella.BLL.Extensions;
using Ella.BLL.Helpers;
using Ella.Core.Entity;
using Ella.DAL.DAL;
using EllaSuper.Areas.Admin.Models;
using EllaSuperFinal.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EllaSuper.Areas.Admin.Controllers
{
    public class AboutController : BaseController
    {
        private readonly AppDbContext _AppDbContext;

        public AboutController(AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var about = await _AppDbContext.Abouts.Where(e => !e.IsDeleted).ToListAsync();
            return View(about);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "You must choose photo");
                return View();
            }
            if (!model.Image.IsAllowedSize(5))
            {
                ModelState.AddModelError("Image", "Image size is over 5MB, Please select less than 5 mb");
                return View();
            }
            var unicalName = await model.Image.GenerateFile(Constants.AboutPath);
            var about = new About
            {
                ImageUrl = unicalName,
                Title = model.Title,
                Description = model.Description,
            };
            await _AppDbContext.Abouts.AddAsync(about);
            await _AppDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
                return NotFound();
            var about = await _AppDbContext.Abouts.FirstOrDefaultAsync(e => e.Id == id);
            if (about == null)
                return NotFound();
            var aboutUpdateViewModel = new AboutUpdateViewModel
            {
                Id = about.Id,
                Title = about.Title,
                Description = about.Description,
                ImageUrl = about.ImageUrl,
            };
            return View(aboutUpdateViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AboutUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var about = await _AppDbContext.Abouts.FirstOrDefaultAsync(e => e.Id == model.Id);
            if (about == null)
                return NotFound();
            if (model.Image != null)
            {
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "You must choose photo");
                    return View();
                }
                if (!model.Image.IsAllowedSize(5))
                {
                    ModelState.AddModelError("Image", "Image size is over 5MB, Please select less than 5 mb");
                    return View();
                }
                var unicalName = await model.Image.GenerateFile(Constants.AboutPath);
                about.ImageUrl = unicalName;
            }
            about.Title = model.Title;
            about.Description = model.Description;
            await _AppDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var about = await _AppDbContext.Abouts.FirstOrDefaultAsync(e => e.Id == id);
            if (about == null)
                return NotFound();
            _AppDbContext.Abouts.Remove(about);
            await _AppDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
