using Ella.Core.Entity;
using Ella.DAL.DAL;
using EllaSuper.Areas.Admin.Models;
using EllaSuperFinal.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EllaSuper.Areas.Admin.Controllers
{
    public class SizeController : BaseController
    {
        private readonly AppDbContext _AppDbContext;
            public SizeController(AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var size = await _AppDbContext.Sizes.ToListAsync();
            return View(size);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Size size)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _AppDbContext.Sizes.AddAsync(size);
            await _AppDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            var size = await _AppDbContext.Sizes.FindAsync(id);
            if (size.Id != id) return BadRequest();

            var existSize = new SizeUpdateViewModel
            {
                Name = size.Name,
            };
            return View(existSize);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SizeUpdateViewModel model)
        {
            if (id == null) return NotFound();
            var sizes = await _AppDbContext.Sizes.FindAsync(id);
            if (sizes == null) return NotFound();
            if (sizes.Id != id) return BadRequest();

            var existName = await _AppDbContext.Brands.AnyAsync(c => c.Name.
            ToLower().Trim() == model.Name.
            ToLower().Trim() && c.Id != id);

            if (existName)
            {
                ModelState.AddModelError("Name", "This Size is already exist!");
                return View(model);
            }
            sizes.Name = model.Name;

            await _AppDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var size = await _AppDbContext.Brands.FindAsync(id);
            if (size == null)
            {
                return NotFound();
            }
            return View(size);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteSize(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var size = await _AppDbContext.Sizes.FindAsync(id);
            if (size == null)
            {
                return NotFound();
            }
            _AppDbContext.Sizes.Remove(size);
            await _AppDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
