using Microsoft.AspNetCore.Mvc;

namespace EllaSuper.Areas.Admin.Models
{
    public class GalleryUpdateViewModel 
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
    }
}
