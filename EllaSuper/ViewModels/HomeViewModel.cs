using Ella.Core.Entity;

namespace EllaSuper.ViewModels
{
    public class HomeViewModel
    {
        public List<Product> Products { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<About> Abouts { get; set; }
        public List<Gallery> Galleries { get; set; }
        public List<WishList> WishLists { get; set; }
        public List<Basket> Baskets { get; set; }
        public Banner Banner { get; set; }

    }
}
