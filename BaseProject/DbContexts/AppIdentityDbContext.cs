using BaseProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.DbContexts
{
    public class AppIdentityDbContext:IdentityDbContext
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options):base(options)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; }
    }
}
