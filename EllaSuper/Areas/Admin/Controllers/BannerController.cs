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
    public class BannerController : BaseController
    {
        private readonly AppDbContext _AppDbContext;

        public BannerController(AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var banners = await _AppDbContext.Banners.Where(e => !e.IsDeleted).ToListAsync();
            return View(banners);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BannerCreateViewModel model)
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
            var unicalName = await model.Image.GenerateFile(Constants.BannerPath);
            var banner = new Banner
            {
                Image = unicalName,
                Name = model.Name,
                Description = model.Description,
            };
            await _AppDbContext.Banners.AddAsync(banner);
            await _AppDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
                return NotFound();
            var banner = await _AppDbContext.Banners.FirstOrDefaultAsync(e => e.Id == id);
            if (banner == null)
                return NotFound();
            var bannerUpdateViewModel = new BannerUpdateViewModel
            {
                Id = banner.Id,
                Name = banner.Name,
                Description = banner.Description,
                ImageUrl = banner.Image,
            };
            return View(bannerUpdateViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(BannerUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var banner = await _AppDbContext.Banners.FirstOrDefaultAsync(e => e.Id == model.Id);
            if (banner == null)
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
                var unicalName = await model.Image.GenerateFile(Constants.BannerPath);
                banner.Image = unicalName;
            }
            banner.Name = model.Name;
            banner.Description = model.Description;
            await _AppDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var banner = await _AppDbContext.Banners.FirstOrDefaultAsync(e => e.Id == id);
            if (banner == null)
                return NotFound();
            _AppDbContext.Banners.Remove(banner);
            await _AppDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

