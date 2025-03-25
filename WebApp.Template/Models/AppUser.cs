using Microsoft.AspNetCore.Identity;
using System.Drawing;

namespace WebApp.Template.Models
{
    public class AppUser:IdentityUser
    {
        public string? Department { get; set; }

        public string? Title {  get; set; }

        public string? PictureUrl { get; set; }

        public string? Description { get; set; }
    }
}
