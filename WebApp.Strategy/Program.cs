using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using WebApp.Strategy.DbContexts;
using WebApp.Strategy.Enums;
using WebApp.Strategy.Models;
using WebApp.Strategy.Repos;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();



builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IProductRepository>(sp =>
{
    // RequiredService eğer yok ise geriye hata fırlatır
    var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();

    var claim = httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type == Settings.claimDatabaseType).FirstOrDefault();

    var context = sp.GetRequiredService<AppIdentityDbContext>();

    if (claim == null)
    {
        return new ProductRepositoryFromSqlServer(context);
    }
    else
    {
        var databaseType = (EDatabaseType)int.Parse(claim.Value);

        return databaseType switch
        {
            EDatabaseType.SqlServer => new ProductRepositoryFromSqlServer(context),
            EDatabaseType.MongoDB => new ProductRepositoryFromMongoDb(builder.Configuration)
        };
    }

});


builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));


builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<AppIdentityDbContext>();


var app = builder.Build();

// Veya program başlangıcında bir kerelik işlemler için
using (var scope = app.Services.CreateScope())
{
    var identityDbContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    // Uygulama ayağı kalktığında otomatik Migrate yapar
    identityDbContext.Database.Migrate();

    // Örnek: Admin kullanıcısını kontrol etme/oluşturma
    //var adminUser = await userManager.FindByNameAsync("admin");

    if (!userManager.Users.Any())
    {
        var newUser = new AppUser
        {
            UserName = "admin",
            Email = "admin@example.com",
            EmailConfirmed = true,
            Title="Owner",
            Department="IT"
        };

        var result = await userManager.CreateAsync(newUser, "Password1!");

        if (result.Succeeded)
        {
           
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
