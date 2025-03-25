using Microsoft.AspNetCore.Identity;

namespace BaseProject.Models
{
    public class AppUser:IdentityUser
    {
        public string? Department { get; set; }

        public string? Title {  get; set; }
    }
}
