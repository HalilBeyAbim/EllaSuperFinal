using Ella.Core.Entity;
using Ella.DAL.DAL;
using EllaSuper.Areas.Admin.Models;
using EllaSuperFinal.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EllaSuper.Areas.Admin.Controllers
{
    public class BrandController : BaseController
    {
        private readonly AppDbContext _AppDbContext;
        public BrandController(AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var brand = await _AppDbContext.Brands.ToListAsync();
            return View(brand);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _AppDbContext.Brands.AddAsync(brand);
            await _AppDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            var brand = await _AppDbContext.Brands.FindAsync(id);
            if (brand.Id != id) return BadRequest();

            var existBrand = new BrandUpdateViewModel
            {
                Name = brand.Name,
            };
            return View(existBrand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, BrandUpdateViewModel model)
        {
            if (id == null) return NotFound();
            var brands = await _AppDbContext.Brands.FindAsync(id);
            if (brands == null) return NotFound();
            if (brands.Id != id) return BadRequest();

            var existName = await _AppDbContext.Brands.AnyAsync(c => c.Name.
            ToLower().Trim() == model.Name.
            ToLower().Trim() && c.Id != id);

            if (existName)
            {
                ModelState.AddModelError("Name", "This Brand is already exist!");
                return View(model);
            }
            brands.Name = model.Name;

            await _AppDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var brand = await _AppDbContext.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteBrand(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var brand = await _AppDbContext.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            _AppDbContext.Brands.Remove(brand);
            await _AppDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

