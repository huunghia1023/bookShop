using bookShopSolution.ViewModels.System.Users;
using bookShopSolution.WebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace bookShopSolution.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;

        public AccountController(IUserApiClient userApiClient,
            IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var response = await _userApiClient.Login(request);
            if (!response.IsSuccessed)
            {
                ViewBag.LoginError = response.Message;
                return View();
            }
            if (response.Results == null)
            {
                ViewBag.LoginError = "Login Failed";
                return View();
            }
            var claims = new List<Claim>
            {
                new Claim("Expire", response.Results.expires_in.ToString()),
                new Claim("Token", response.Results.access_token),
                new Claim(ClaimTypes.Name, request.UserName),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            HttpContext.Session.SetString("Token", response.Results.access_token);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // clear seesion
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(
             CookieAuthenticationDefaults.AuthenticationScheme);
            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var response = await _userApiClient.Register(request);
            if (!response.IsSuccessed)
            {
                ModelState.AddModelError("", response.Message);
                return View();
            }
            var loginResult = await _userApiClient.Login(new LoginRequest()
            {
                UserName = request.UserName,
                Password = request.Password,
                RememberMe = true,
                IsFromAdmin = false
            });

            var claims = new List<Claim>
            {
                new Claim("Expire", loginResult.Results.expires_in.ToString()),
                new Claim("Token", loginResult.Results.access_token),
                new Claim(ClaimTypes.Name, request.UserName),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            HttpContext.Session.SetString("Token", loginResult.Results.access_token);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

            return RedirectToAction("Index", "Home");
        }
    }
}