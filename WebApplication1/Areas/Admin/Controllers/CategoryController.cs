using Ella.Core.Entity;
using Ella.DAL.DAL;
using EllaSuper.Areas.Admin.Models;
using EllaSuperFinal.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EllaSuper.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly AppDbContext _appDbContext;
        public CategoryController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _appDbContext.Categories.ToListAsync();
            return View(categories);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(CategoryCreateViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }
        //    await _context.Categories.AddAsync(category);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var existCategory = await _appDbContext.Categories.Where(c => c.IsDeleted).ToListAsync();

            if (existCategory.Any(c => c.Name.ToLower().Trim().Equals(model.Name.ToLower().Trim())))
            {
                ModelState.AddModelError("Name", "This Category is already exist");
                return View();
            }
            var category = new Category
            {
                Name = model.Name,
            };

            await _appDbContext.Categories.AddAsync(category);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            var category = await _appDbContext.Categories.FindAsync(id);
            if (category.Id != id) return BadRequest();

            var existCategory = new CategoryUpdateViewModel
            {
                Name = category.Name,
            };
            return View(existCategory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, CategoryUpdateViewModel model)
        {
            if (id == null) return NotFound();
            var categories = await _appDbContext.Categories.FindAsync(id);
            if (categories == null) return NotFound();
            if (categories.Id != id) return BadRequest();

            var existName = await _appDbContext.Categories.AnyAsync(c => c.Name.
            ToLower().Trim() == model.Name.
            ToLower().Trim() && c.Id != id);

            if (existName)
            {
                ModelState.AddModelError("Name", "This Category is already exist!");
                return View(model);
            }
            categories.Name = model.Name;

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _appDbContext.Categories.FindAsync(id);

            if (category == null) return NotFound();
            if (category.Id != id) return BadRequest();

            _appDbContext.Categories.Remove(category);

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }



    }

}
