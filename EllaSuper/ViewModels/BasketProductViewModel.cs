using Ella.Core.Entity;

namespace EllaSuper.ViewModels
{
    public class BasketProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public string Images { get; set; }
        public string Description { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountPrice { get; set; }
        public Size size { get; set; }
        public Brand Brand { get; set; }
    }
}
