using WebApp.Command.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Command.DbContexts
{
    public class AppIdentityDbContext:IdentityDbContext
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options):base(options)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<Product > Products { get; set; }
    }
}
