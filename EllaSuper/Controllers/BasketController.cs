using Ella.BLL.BasketViewModels;
using Ella.Core.Entity;
using Ella.DAL.DAL;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EllaSuper.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _AppDbContext;
        public BasketController (AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
        }
        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id is null || id == 0) return NotFound();
            Product product = await _AppDbContext.Product.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            string basketStr = HttpContext.Request.Cookies["Basket"];

            BasketViewModel basket;
            if (string.IsNullOrEmpty(basketStr))
            {
                basket = new BasketViewModel();
                BasketCookieItemViewModel cookieItem = new BasketCookieItemViewModel
                {
                    Id = product.Id,
                    Quantity = 1
                };
                basket.basketCookieItemViewModels = new List<BasketCookieItemViewModel>();
                basket.basketCookieItemViewModels.Add(cookieItem);
                basket.TotalPrice = product.Price;
            }
            else
            {
                basket = JsonConvert.DeserializeObject<BasketViewModel>(basketStr);
                BasketCookieItemViewModel existed = basket.basketCookieItemViewModels.FirstOrDefault(p => p.Id == product.Id);
                if (existed == null)
                {
                    BasketCookieItemViewModel cookieItem = new BasketCookieItemViewModel
                    {
                        Id = product.Id,
                        Quantity = 1
                    };
                    basket.basketCookieItemViewModels.Add(cookieItem);
                    basket.TotalPrice += product.Price;
                }
                else
                {
                    basket.TotalPrice += product.Price;
                    existed.Quantity++;
                }
            }
            

            basketStr = JsonConvert.SerializeObject(basket);

            HttpContext.Response.Cookies.Append("Basket", basketStr);

            return RedirectToAction(nameof(ShowBasket));

        }
        public IActionResult ShowBasket()
        {
            if (HttpContext.Request.Cookies["Basket"] == null) return NotFound();

            BasketViewModel basket = JsonConvert.DeserializeObject<BasketViewModel>(HttpContext.Request.Cookies["Basket"]);
            return Json(basket);
        }
    }
}
