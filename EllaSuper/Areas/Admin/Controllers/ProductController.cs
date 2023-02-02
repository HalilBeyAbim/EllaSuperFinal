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
    public class ProductController : BaseController
    {
        private readonly AppDbContext _appDbContext;

        public ProductController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var product = await _appDbContext.Product
                .Include(b => b.Category)
                .Include(b => b.Brand)
                .Include(b => b.Size)
                .ToListAsync();
            
            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _appDbContext.Categories.Where(e => !e.IsDeleted).ToListAsync();
            var categoryList = new List<SelectListItem>
            {
                new SelectListItem("Select Category" , "0")
            };
            var brands = await _appDbContext.Brands.Where(e => !e.IsDeleted).ToListAsync();
            var brandsList = new List<SelectListItem>
            {
                new SelectListItem("Select Brand" , "0")
            };
            var sizes = await _appDbContext.Sizes.Where(e => !e.IsDeleted).ToListAsync();
            var sizeList = new List<SelectListItem>
            {
                new SelectListItem("Select Size" , "0")
            };
            categories.ForEach(c => categoryList.Add(new SelectListItem(c.Name, c.Id.ToString())));
            sizes.ForEach(c => sizeList.Add(new SelectListItem(c.Name, c.Id.ToString())));
            brands.ForEach(c => brandsList.Add(new SelectListItem(c.Name, c.Id.ToString())));


            var model = new ProductCreateViewModel
            {
                Categories = categoryList,
                Brands = brandsList,
                Sizes = sizeList
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {

            var categories = await _appDbContext
                .Categories
                .Where(e => !e.IsDeleted)
                .Include(e => e.Products)
                .ToListAsync();
            var brands = await _appDbContext
                .Brands.Where(e => !e.IsDeleted)
                .Include(e => e.Products)
                .ToListAsync();
            var sizes = await _appDbContext
                .Sizes
                .Where(e => !e.IsDeleted)
                .Include(e => e.Products)
                .ToListAsync();
            if (!ModelState.IsValid) return View(model);
            var categoryList = new List<SelectListItem>
            {
                new SelectListItem("Category does't select","0")

            };
            var brandList = new List<SelectListItem>
            {
                new SelectListItem("Publisher does't select","0")

            };
            var sizeList = new List<SelectListItem>
            {
                new SelectListItem("Author does't select","0")

            };
            categories.ForEach(e => categoryList.Add(new SelectListItem(e.Name, e.Id.ToString())));
            brands.ForEach(e => brandList.Add(new SelectListItem(e.Name, e.Id.ToString())));
            sizes.ForEach(e => sizeList.Add(new SelectListItem(e.Name, e.Id.ToString())));

            var productViewModel = new ProductCreateViewModel()
            {
                Categories = categoryList,
                Brands = brandList,
                Sizes = sizeList,
            };

            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "You must choose image");
                return View();
            }
            if (!model.Image.IsAllowedSize(5))
            {
                ModelState.AddModelError("Image", "Image size is over 5mb!!!");
                return View();
            }

            if (model.CategoryId == 0)
            {
                ModelState.AddModelError("", "Category does't select");
                return View();
            }
            if (model.BrandId == 0)
            {
                ModelState.AddModelError("", "Publisher does't select");
                return View();
            }
            if (model.SizeId == 0)
            {
                ModelState.AddModelError("", "Author does't select");
                return View();
            }
            var unicalName = await model.Image.GenerateFile(Constants.ProductPath);
            var newProduct = new Product
            {
                ImageUrl = unicalName,
                CategoryId = model.CategoryId,
                BrandId = model.BrandId,
                SizeId = model.SizeId,
                Title = model.Title,
                Price = model.Price,
                Discount = model.Discount,
                DiscountPrice = model.DiscountPrice,
                Description = model.Description,
                SubTitle = model.Subtitle,
                Barcode = model.Barcode,
            };

            await _appDbContext.Product.AddAsync(newProduct);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            var category = await _appDbContext.Categories.Where(e => !e.IsDeleted).ToListAsync();
            var brand = await _appDbContext.Brands.Where(e => !e.IsDeleted).ToListAsync();
            var size = await _appDbContext.Sizes.Where(e => !e.IsDeleted).ToListAsync();

            if (category == null || brand == null || size == null) return NotFound();
            var product = await _appDbContext.Product
              .Where(e => !e.IsDeleted && e.Id == id)
              .Include(e => e.Category)
              .Include(e => e.Size)
              .Include(e => e.Brand)
              .FirstOrDefaultAsync();
            if (product == null) return NotFound();
            if (product.Id != id) return NotFound();

            var selectCategory = new List<SelectListItem>();
            var selectBrand = new List<SelectListItem>();
            var selectSize = new List<SelectListItem>();

            var viewModel = new ProductUpdateViewModel
            {
                Categories = selectCategory,
                Brands = selectBrand,
                Sizes = selectSize,
            };

            if (!ModelState.IsValid) return View(viewModel);

            category.ForEach(e => selectCategory.Add(new SelectListItem(e.Name, e.Id.ToString())));
            brand.ForEach(e => selectBrand.Add(new SelectListItem(e.Name, e.Id.ToString())));
            size.ForEach(e => selectSize.Add(new SelectListItem(e.Name, e.Id.ToString())));


            var productUpdateViewModel = new ProductUpdateViewModel
            {
                Title = product.Title,
                Subtitle = product.SubTitle,
                Description = product.Description,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                BrandId = product.BrandId,
                SizeId = product.SizeId,
                Categories = selectCategory,
                Brands = selectBrand,
                Sizes = selectSize,
                Barcode = product.Barcode,
                Discount = product.Discount,

            };
            return View(productUpdateViewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, ProductUpdateViewModel model)
        {
            if (id == null) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            var categories = await _appDbContext.Categories.Where(e => !e.IsDeleted).ToListAsync();
            var brands = await _appDbContext.Brands.Where(e => !e.IsDeleted).ToListAsync();
            var sizes = await _appDbContext.Sizes.Where(e => !e.IsDeleted).ToListAsync();
        

            if (categories == null || brands == null || sizes == null ) return NotFound();

            var product = await _appDbContext.Product.Where(e => !e.IsDeleted && e.Id == id)
              .Include(e => e.Category)
              .Include(e => e.Size)
              .Include(e => e.Brand)

              .FirstOrDefaultAsync();
        
            if (product == null) return NotFound();

            if (model.Image != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(new ProductUpdateViewModel
                    {
                        ImageUrl = model.ImageUrl,
                    });
                }
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "You must choose image");
                    return View(new ProductUpdateViewModel
                    {
                        ImageUrl = product.ImageUrl
                    });
                }
                if (!model.Image.IsAllowedSize(5))
                {
                    ModelState.AddModelError("Image", "Image size is over 5MB!!!!");

                    return View(model);
                }

                var unicalPath = Path.Combine(Constants.ProductPath, product.ImageUrl);

                if (System.IO.File.Exists(unicalPath))
                    System.IO.File.Delete(unicalPath);


                var unicalFile = await model.Image.GenerateFile(Constants.ProductPath);
                product.ImageUrl = unicalFile;
            }

            var selectedProduct = new ProductUpdateViewModel
            {
                CategoryId = model.CategoryId,
                BrandId = model.BrandId,
                SizeId = model.SizeId,
            };
            product.Title = model.Title;
            product.Description = model.Description;
            product.Price = model.Price;
            product.DiscountPrice = model.DiscountPrice;
            product.CategoryId = model.CategoryId;
            product.BrandId = model.BrandId;
            product.SizeId = model.SizeId;
            product.Barcode = model.Barcode;
            product.SubTitle = model.Subtitle;

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _appDbContext.Product.FirstOrDefaultAsync(e => e.Id == id);

            if (product == null) return NotFound();

            if (product.Id != id) return BadRequest();
            var unicalPath = Path.Combine(Constants.ProductPath, "images", "books", product.ImageUrl);

            if (System.IO.File.Exists(unicalPath))
                System.IO.File.Delete(unicalPath);

            _appDbContext.Product.Remove(product);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
     }
 }
