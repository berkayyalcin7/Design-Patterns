using Microsoft.AspNetCore.Identity;

namespace WebApp.Command.Models
{
    public class AppUser:IdentityUser
    {
        public string? Department { get; set; }

        public string? Title {  get; set; }
    }
}
