using Microsoft.AspNetCore.Mvc;

namespace EllaSuper.Areas.Admin.Models
{
    public class TeamCreateViewModel 
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public IFormFile Image { get; set; }
    }
}
