using Ella.BLL.Helpers;
using Ella.Core.Entity;
using Ella.DAL.DAL;
using EllaSuper.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EllaSuper.Controllers
{
    public class WishListController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public WishListController(AppDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<WishListViewModel> model = new();
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                var wishList = await _dbContext
                    .WishList
                    .Where(x => x.UserId == user.Id)
                    .Include(x => x.WishListProducts)
                    .ThenInclude(x => x.Product)
                    .FirstOrDefaultAsync();

                
                foreach (var item in wishList.WishListProducts)
                {
                    model.Add(new WishListViewModel
                    {
                        Id = item.Product.Id,
                        Name = item.Product.Title,
                        Price = item.Product.Price,
                        Discount = item.Product.Discount,
                        ImageUrl = item.Product.ImageUrl,
                    });

                    if (model is null)
                    {
                        return NoContent();
                    }
                }
            }
            else
            {
                if (Request.Cookies.TryGetValue(Constants.WISH_LIST_COOKIE_NAME, out var cookie))
                {
                   
                    var productIdList = JsonConvert.DeserializeObject<List<int>>(cookie);
                    if (model is null)
                    {
                        return NoContent();
                    }
                    foreach (var productId in productIdList)
                    {
                        var product = await _dbContext.Product
                            .Where(x=>x.Id==productId)
                            .FirstOrDefaultAsync();
                        
                        
                        model.Add(new WishListViewModel
                        {
                            Id=product.Id,
                            Name=product.Title,
                            Price=product.Price,
                            Discount=product.Discount,
                            ImageUrl=product.ImageUrl,
                        });

                       
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishList(int? productId)
        {
            if (productId == null)
            {
                return NotFound();
            }
            
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null) return BadRequest();

                var exsitWishList = await _dbContext.WishList
                    .Where(x => x.UserId == user.Id)
                    .Include(x=>x.WishListProducts)
                    .FirstOrDefaultAsync();
                
                if (exsitWishList != null)
                {
                    var createdWishList = new WishList
                    {
                        UserId = user.Id,
                        WishListProducts = new List<WishListProduct>()

                    };

                    var exsitProduct = await _dbContext.Product.FindAsync(productId);
                    
                    if (exsitProduct == null) return NotFound();

                    if (exsitWishList.WishListProducts.Any(x => x.ProductId == exsitProduct.Id))
                        return NoContent();

                    exsitWishList.WishListProducts.Add(new WishListProduct
                    {
                        WishListId = createdWishList.Id,
                        ProductId = exsitProduct.Id,
                    });
                    _dbContext.Update(exsitWishList);
                }
                else
                {
                    var createdWishList = new WishList
                    {
                        UserId = user.Id,
                        WishListProducts = new List<WishListProduct>()

                    };
                    List<WishListProduct> wishListProducts = new();
                    var exsitProduct = await _dbContext.Product.FindAsync(productId);
                    if (exsitProduct == null) return NotFound();

                    if (exsitWishList.WishListProducts.Any(x => x.ProductId == exsitProduct.Id))
                        return NoContent();

                    wishListProducts.Add(new WishListProduct
                    {
                        WishListId = createdWishList.Id,
                        ProductId = exsitProduct.Id,
                    });
                    createdWishList.WishListProducts = wishListProducts;
                    await _dbContext.WishList.AddAsync(createdWishList);
                }
                await _dbContext.SaveChangesAsync();
            }
            else
                await _dbContext.SaveChangesAsync();
            {
                if (Request.Cookies.TryGetValue(Constants.WISH_LIST_COOKIE_NAME, out var cookie))
                {
                    var productIdList = JsonConvert.DeserializeObject<List<int>>(cookie);
                    if (productIdList.Contains(productId.Value)) return NoContent();

                    productIdList.Add(productId.Value);

                    var productIdListJson = JsonConvert.SerializeObject(productIdList);

                    Response.Cookies.Append(Constants.WISH_LIST_COOKIE_NAME, productIdListJson);

                }
                else
                {
                    var productIdListJson = JsonConvert.SerializeObject(new List<int> { productId.Value });

                    Response.Cookies.Append(Constants.WISH_LIST_COOKIE_NAME, productIdListJson);
                }
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductFromWishList(int? productId)
        {
            if (productId == null) return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null) return BadRequest();

                var wishList = await _dbContext.WishList.Where(x => x.UserId == user.Id)
                    .Include(x => x.WishListProducts).FirstOrDefaultAsync();
                var existProduct = await _dbContext.Product.FindAsync(productId);
                if (existProduct == null) return NotFound();
                var existWishLitProduct = wishList.WishListProducts.FirstOrDefault(x => x.ProductId == existProduct.Id);
                wishList.WishListProducts.Remove(existWishLitProduct);
                _dbContext.Update(wishList);
                await _dbContext.SaveChangesAsync();
            }
            else
            {

                if (Request.Cookies.TryGetValue(Constants.WISH_LIST_COOKIE_NAME, out var cookie))
                {
                    var productIdList = JsonConvert.DeserializeObject<List<int>>(cookie);

                    productIdList.Remove(productId.Value);

                    var productIdListJson = JsonConvert.SerializeObject(productIdList);

                    Response.Cookies.Append(Constants.WISH_LIST_COOKIE_NAME, productIdListJson);
                }

            }
            
            
            return NoContent();
        }

    }

}
