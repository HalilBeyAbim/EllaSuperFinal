namespace EllaSuper.Areas.Admin.Models
{
    public class BannerCreateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}
