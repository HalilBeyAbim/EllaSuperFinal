using Ella.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ella.BLL.BasketViewModels
{
    public class LayoutBasketViewModel
    {
        public List<BasketItemViewModel> basketItemViewModels { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
