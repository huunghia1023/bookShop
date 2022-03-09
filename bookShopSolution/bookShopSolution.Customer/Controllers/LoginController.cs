using bookShopSolution.Customer.Services;
using bookShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace bookShopSolution.Customer.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;

        public LoginController(IUserApiClient userApiClient,
            IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(ModelState);
            var responseModel = await _userApiClient.Authenticate(request);
            //var user = await this.GetUserInfo(responseModel.access_token);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, responseModel.Email),
                new Claim(ClaimTypes.Name, request.UserName),
                new Claim(ClaimTypes.GivenName, responseModel.FirstName),
                new Claim(ClaimTypes.Surname, responseModel.LastName),
                new Claim(ClaimTypes.Role, responseModel.Roles),
                new Claim(ClaimTypes.Expired, responseModel.expires_in.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = true
            };
            HttpContext.Session.SetString("Token", responseModel.access_token);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal,
                authProperties);
            return RedirectToAction("Index", "Home");
        }
    }
}