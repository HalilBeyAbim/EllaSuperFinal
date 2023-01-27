using Microsoft.AspNetCore.Mvc;

namespace EllaSuper.Areas.Admin.Models
{
    public class AboutCreateViewModel 
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}
