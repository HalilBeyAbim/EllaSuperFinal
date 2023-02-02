using Ella.BLL.Extensions;
using Ella.BLL.Helpers;
using Ella.Core.Entity;
using Ella.DAL.DAL;
using EllaSuper.Areas.Admin.Models;
using EllaSuperFinal.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EllaSuper.Areas.Admin.Controllers
{
    public class BlogController : BaseController
    {
        private readonly AppDbContext _appDbContext;

        public BlogController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _appDbContext.Blogs.ToListAsync();
            return View(blogs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateViewModel model)
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

            var unicalName = await model.Image.GenerateFile(Constants.BlogPath);

            var blog = new Blog
            {
                ImageUrl = unicalName,
                Title = model.Title,
                Description = model.Description,
                Author = model.Author,
                Created = DateTime.Now,
            };

            await _appDbContext.Blogs.AddAsync(blog);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var blogs = await _appDbContext.Blogs.FindAsync(id);

            if (blogs == null) return NotFound();

            var blogModel = new BlogUpdateViewModel
            {
                Id = blogs.Id,
                Title = blogs.Title,
                Description = blogs.Description,
                Author = blogs.Author,
                ImageUrl = blogs.ImageUrl
            };
            return View(blogModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, BlogUpdateViewModel model)
        {
            if (id == null) return NotFound();

            var blogs = await _appDbContext.Blogs.FindAsync(id);

            if (blogs == null) return NotFound();
            if (blogs.Id != id) return BadRequest();


            if (model.Image != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(new BlogUpdateViewModel
                    {
                        ImageUrl = blogs.ImageUrl
                    });
                }
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "You must choose photo");
                    return View(new BlogUpdateViewModel
                    {
                        ImageUrl = blogs.ImageUrl
                    });
                }
                if (!model.Image.IsAllowedSize(5))
                {
                    ModelState.AddModelError("Image", "Image size is over 5MB, Please select less than 5 mb");
                    return View(model);
                }
                var unicalName = await model.Image.GenerateFile(Constants.BlogPath);
                blogs.ImageUrl = unicalName;
            }

            blogs.Title = model.Title;
            blogs.Description = model.Description;
            blogs.Author = model.Author;

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var dbBlog = await _appDbContext.Blogs.FindAsync(id);

            if (dbBlog == null) return NotFound();
            if (dbBlog.Id != id) return BadRequest();

            var unicalPath = Path.Combine(Constants.RootPath, "assets", "images", "blog", dbBlog.ImageUrl);

            if (System.IO.File.Exists(unicalPath))
                System.IO.File.Delete(unicalPath);

            _appDbContext.Blogs.Remove(dbBlog);

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


    }
}
