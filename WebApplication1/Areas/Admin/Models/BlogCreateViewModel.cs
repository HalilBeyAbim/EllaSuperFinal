namespace EllaSuper.Areas.Admin.Models
{
    public class BlogCreateViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string Author { get; set; }
        public DateTime Created { get; set; }
    }
}
