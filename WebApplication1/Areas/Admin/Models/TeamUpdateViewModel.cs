using Microsoft.AspNetCore.Mvc;

namespace EllaSuper.Areas.Admin.Models
{
    public class TeamUpdateViewModel
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}
