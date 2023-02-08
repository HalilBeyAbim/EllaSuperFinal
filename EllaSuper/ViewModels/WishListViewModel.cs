namespace EllaSuper.ViewModels
{
    public class WishListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountPrice => Price - Price * Discount / 100;

    }
}
