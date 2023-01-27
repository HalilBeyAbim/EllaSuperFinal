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
    public class GalleryController : BaseController
    {
        private readonly AppDbContext _AppDbContext;
        public GalleryController(AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var gallery = await _AppDbContext.Galleries.ToListAsync();
            return View(gallery);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GalleryCreateViewModel model)
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
            var unicalName = await model.Image.GenerateFile(Constants.GalleryPath);
            var gallery = new Gallery
            {
                ImageUrl = unicalName,
            };
            await _AppDbContext.Galleries.AddAsync(gallery);
            await _AppDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
                return NotFound();
            var gallery = await _AppDbContext.Galleries.FindAsync(id);
            if (gallery == null)
                return NotFound();   
            var galleryupdatevie = new GalleryUpdateViewModel
            {
                Id = gallery.Id,
                ImageUrl = gallery.ImageUrl,
            };
            return View(gallery);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, GalleryUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var gallery = await _AppDbContext.Galleries.FirstOrDefaultAsync(e => e.Id == id);
            if (gallery == null)
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
                var unicalName = await model.Image.GenerateFile(Constants.GalleryPath);
                gallery.ImageUrl = unicalName;
            }
            if (model.Image == null)
            {
                model.ImageUrl = gallery.ImageUrl;
            }

            await _AppDbContext.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(GalleryUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var gallery = await _AppDbContext.Galleries.FindAsync(model.Id);
            if (gallery == null)
                return NotFound();
            _AppDbContext.Galleries.Remove(gallery);
            await _AppDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
