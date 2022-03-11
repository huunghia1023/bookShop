using bookShopSolution.Customer.Services;
using bookShopSolution.ViewModels.Catalog.User;
using bookShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace bookShopSolution.Customer.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;

        public UserController(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        public IActionResult Index()
        {
            var sessions = HttpContext.Session.GetString("Token");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        //[HttpGet]
        //public async Task<UserViewModel> GetUserInfo(string accessToken)
        //{
        //    if (string.IsNullOrEmpty(accessToken))
        //        return null;
        //    var user = await _userApiClient.GetUserInfo(accessToken);
        //    return user;
        //}

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(ModelState);
            var responseModel = await _userApiClient.Authenticate(request);
            //var user = await this.GetUserInfo(responseModel.access_token);
            var claims = new List<Claim>
            {
                //new Claim(ClaimTypes.Email, responseModel.Email),
                //new Claim(ClaimTypes.Name, request.UserName),
                //new Claim(ClaimTypes.GivenName, responseModel.FirstName),
                //new Claim(ClaimTypes.Surname, responseModel.LastName),
                //new Claim(ClaimTypes.Role, responseModel.Roles),
                //new Claim(ClaimTypes.Expired, responseModel.expires_in.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = true
            };
            //HttpContext.Session.SetString("Token", responseModel.access_token);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal,
                authProperties);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login", "User");
        }
    }
}