using bookShopSolution.Data.Entities;
using bookShopSolution.Utilities.Constants;
using bookShopSolution.ViewModels.Catalog.IdentityServerResponses;
using bookShopSolution.ViewModels.Catalog.User;
using bookShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace bookShopSolution.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<AuthenticateResponseViewModel> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return null;
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
                return null;
            var roles = await _userManager.GetRolesAsync(user);

            var responseModel = new AuthenticateResponseViewModel();
            IdentityServerRequest getTokenRequest = new IdentityServerRequest(request.UserName, request.Password, SystemConstants.BackendGrandType, SystemConstants.BackendClientId, SystemConstants.BackendClientSecret);
            // identity server 4 only accept "application/x-www-form-urlencoded"

            var content = getTokenRequest.GetTokenContent();

            var httpContent = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_config.GetSection("AuthorityUrl")["value"]);
            var response = await client.PostAsync("/connect/token", httpContent);
            if (response.IsSuccessStatusCode)
            {
                responseModel = await response.Content.ReadFromJsonAsync<AuthenticateResponseViewModel>();
                responseModel.Email = user.Email;
                responseModel.FirstName = user.FirstName;
                responseModel.LastName = user.LastName;
                responseModel.Roles = String.Join(";", roles);
            }

            return responseModel;

            //var claims = new[]
            //{
            //    new Claim(ClaimTypes.Email, user.Email),
            //    new Claim(ClaimTypes.GivenName, user.FirstName),
            //    new Claim(ClaimTypes.Surname, user.LastName),
            //    new Claim(ClaimTypes.Role, String.Join(";", roles))
            //};

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Tokens")["Key"]));

            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var token = new JwtSecurityToken(_config.GetSection("Tokens")["Issuer"], _config.GetSection("Tokens")["Issuer"], claims, expires: DateTime.Now.AddHours(3), signingCredentials: creds);
            //return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var user = new AppUser()
            {
                BirthDay = request.BirthDay,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                UserName = request.UserName
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
                return true;
            return false;
        }

        public async Task<UserViewModel> GetUserInfo(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                return null;
            var user = new UserViewModel();
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri("https://localhost:5000");

            //client.DefaultRequestHeaders.Add("Authorization", bearerToken);
            var response = await client.GetAsync("/connect/userinfo");
            if (response.IsSuccessStatusCode)
            {
                //var jsonUser = await response.Content.ReadAsStringAsync();
                user = await response.Content.ReadFromJsonAsync<UserViewModel>();
                AppUser appUser = await _userManager.FindByIdAsync(user.sub);

                var roles = await _userManager.GetRolesAsync(appUser);
                user.Roles = roles;
            }
            return user;
        }
    }
}