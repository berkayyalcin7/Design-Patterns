using WebApp.Command.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace WebApp.Command.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken] // CSRF koruması için
        public async Task<IActionResult> Login(string Email, string Password, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ModelState.AddModelError(string.Empty, "Email ve şifre gereklidir.");
                return View();
            }

            var hasUser = await _userManager.FindByEmailAsync(Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifre hatalı.");
                return View();
            }

            var signIn = await _signInManager.PasswordSignInAsync(hasUser, Password, true, false);
            if (!signIn.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifre hatalı.");
                return View();
            }

            // Başarılı giriş, yönlendirme yap
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }



    }
}
