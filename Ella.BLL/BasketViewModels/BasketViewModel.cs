using Ella.Core.Entity;

namespace Ella.BLL.BasketViewModels
{
    public class BasketViewModel
    {
        public List<BasketCookieItemViewModel> basketCookieItemViewModels { get; set; }
        public decimal TotalPrice { get; set; }
        public int Id { get; set; }
        public int Count { get; set; }
    }
}
