using Ella.Core.Entity;

namespace EllaSuper.ViewModels
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Size> Sizes { get; set; }   
    }
}
