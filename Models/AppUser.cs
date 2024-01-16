using Microsoft.AspNetCore.Identity;

namespace Amado.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
