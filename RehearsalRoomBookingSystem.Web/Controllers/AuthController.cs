using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RehearsalRoomBookingSystem.Service.Interface;
using RehearsalRoomBookingSystem.Web.Models.ViewModels;
using System.Security.Claims;

namespace RehearsalRoomBookingSystem.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAdministratorService _administratorService;

        public AuthController(IAdministratorService administratorService)
        {
            _administratorService = administratorService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            // 如果使用者已經登入，直接導向首頁
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Member");
            }

            var viewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var admin = _administratorService.ValidateLogin(model.Account, model.Password);
            if (admin == null)
            {
                ModelState.AddModelError("", "帳號或密碼錯誤");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Name),
                new Claim(ClaimTypes.NameIdentifier, admin.Account),
                new Claim(ClaimTypes.Role, admin.TypeId.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTime.UtcNow.AddHours(12)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Member");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}