using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EllaSuper.Areas.Admin.Models
{
    public class ProductCreateViewModel 
    {
        public List<SelectListItem>? Categories { get; set; }
        public List<SelectListItem>? Brands { get; set; }
        public List<SelectListItem>? Sizes { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public byte Discount { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int SizeId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Barcode { get; set; }
        public decimal DiscountPrice { get; set; }
    }
}
