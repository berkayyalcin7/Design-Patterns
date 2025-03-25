using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Strategy.Models;

namespace WebApp.Strategy.Controllers
{
    public class SettingsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public SettingsController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            Settings settings = new();

            if (User.Claims.Where(x=>x.Type==Settings.claimDatabaseType).FirstOrDefault() != null)
            {
                settings.DatabaseType = (Enums.EDatabaseType)int.Parse(User.Claims.First(x => x.Type == Settings.claimDatabaseType).Value);


            }
            else
            {
                settings.DatabaseType = settings.GetDefaultType;
            }


            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDatabase(int DatabaseType)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var newClaim = new Claim(Settings.claimDatabaseType, DatabaseType.ToString());

            var claims = await _userManager.GetClaimsAsync(user);

            var hasDbTypeClaim = claims.FirstOrDefault(x => x.Type == Settings.claimDatabaseType);


            // varsa replace yoksa ekleme
            if (hasDbTypeClaim != null)
            {
                await _userManager.ReplaceClaimAsync(user, hasDbTypeClaim, newClaim);
            }
            else
            {
                await _userManager.AddClaimAsync(user, newClaim);
            }

            // Kullanıcı hissetmeden çıkış giriş yaptır.
            await _signInManager.SignOutAsync();

            var authenticateResult =await HttpContext.AuthenticateAsync();

            await _signInManager.SignInAsync(user, authenticateResult.Properties);

            return RedirectToAction(nameof(Index));

        }
    }
}
