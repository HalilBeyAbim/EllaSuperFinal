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
        public string Price { get; set; }
        public string Discount { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int SizeId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Barcode { get; set; }
        public string DiscountPrice { get; set; }
    }
}
