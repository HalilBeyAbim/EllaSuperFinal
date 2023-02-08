using Ella.BLL.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ella.Core.Entity
{
    public class Product : Entity
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public byte Discount { get; set; }
        public decimal DiscountPrice { get; set; }
        public int SizeId { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public ProductSize ProductSize { get; set; }
        public Size Size { get; set; }
        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public ICollection<WishListProduct> WishListProducts { get; set; }

    }
}
