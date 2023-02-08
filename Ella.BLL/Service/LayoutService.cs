using Ella.BLL.BasketViewModels;
using Ella.Core.Entity;
using Ella.DAL.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Ella.BLL.Service
{
    public class LayoutService
    {
        private readonly AppDbContext _AppDbContext;
        private readonly IHttpContextAccessor _http;
        public LayoutService(AppDbContext appDbContext, IHttpContextAccessor http)
        {
            _AppDbContext = appDbContext;
            _http = http;
        }

        public List<Setting> GetSettings()
        {
            List<Setting> settings = _AppDbContext.Settings.ToList();
            return settings;
        }
        public LayoutBasketViewModel GetBasket()
        {
          //  BasketViewModel basket = new BasketViewModel();
            string basketStr = _http.HttpContext.Request.Cookies["Basket"];
            if (!string.IsNullOrEmpty(basketStr))
            {
                BasketViewModel basket = JsonConvert.DeserializeObject<BasketViewModel>(basketStr);
                LayoutBasketViewModel layoutBasket   = new LayoutBasketViewModel();
                layoutBasket.basketItemViewModels = new List<BasketItemViewModel>();
                foreach (BasketCookieItemViewModel cookie in basket.basketCookieItemViewModels)
                {
                    Product exsited = _AppDbContext.Product.Include(p=>p.ImageUrl).FirstOrDefault(p => p.Id == cookie.Id);
                    if(exsited == null)
                    {
                        basket.basketCookieItemViewModels.Remove(cookie);
                        continue;
                    }
                    BasketItemViewModel basketItem = new BasketItemViewModel
                    {
                        Product = exsited,
                        Quantity = cookie.Quantity
                    };
                        layoutBasket.basketItemViewModels.Add(basketItem);
                    layoutBasket.TotalPrice = basket.TotalPrice;
                }
                return layoutBasket;
            }
            return null;
        }   


    }
}
