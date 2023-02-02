using Microsoft.AspNetCore.Identity;

namespace Ella.Core.Entity
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        
    }
}
