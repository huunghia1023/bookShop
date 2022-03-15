using bookShopSolution.Data.Entities;
using bookShopSolution.Utilities.Constants;
using bookShopSolution.ViewModels.Catalog.IdentityServerResponses;
using bookShopSolution.ViewModels.Catalog.User;
using bookShopSolution.ViewModels.common;
using bookShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
using IdentityModel.Client;
using bookShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Web.Resource;
using Microsoft.AspNetCore.Authorization;

namespace bookShopSolution.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPasswordHasher<AppUser> _passwordHasher;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration config, IHttpClientFactory httpClientFactory, IPasswordHasher<AppUser> passwordHasher)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
            _httpClientFactory = httpClientFactory;
            _passwordHasher = passwordHasher;
        }

        public async Task<ApiResult<AuthenticateResponseViewModel>> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return new ApiErrorResult<AuthenticateResponseViewModel>("Username or Password is invalid");

            if (request.IsFromAdmin)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Where(x => x == "admin").Any())
                {
                    return new ApiErrorResult<AuthenticateResponseViewModel>("Access Denied");
                }
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
                return new ApiErrorResult<AuthenticateResponseViewModel>("Username or Password is invalid");
            // admin
            IdentityServerRequest getTokenRequest = new IdentityServerRequest(request.UserName, request.Password, SystemConstants.BackendGrandType, SystemConstants.BackendClientId, SystemConstants.BackendClientSecret);

              // identity server 4 only accept "application/x-www-form-urlencoded"

            var content = getTokenRequest.GetTokenContent();

            var httpContent = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_config.GetSection("AuthorityUrl")["value"]);
            var response = await client.PostAsync("/connect/token", httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var myDeserializedObjList = (AuthenticateResponseViewModel)JsonConvert.DeserializeObject(body,
                    typeof(AuthenticateResponseViewModel));

                return new ApiSuccessResult<AuthenticateResponseViewModel>(myDeserializedObjList);
            }
            return new ApiErrorResult<AuthenticateResponseViewModel>("Login failed");

            //var responseModel = await response.Content.ReadFromJsonAsync<AuthenticateResponseViewModel>();
            //    return new ApiSuccessResult<AuthenticateResponseViewModel>(responseModel);
            //}
            //return new ApiErrorResult<AuthenticateResponseViewModel>("Login failed");
        }

        public async Task<ApiResult<string>> Register(RegisterRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                return new ApiErrorResult<string>("Username is existed");
            }
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return new ApiErrorResult<string>("Email is existed");
            }
            user = new AppUser()
            {
                BirthDay = request.BirthDay,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                UserName = request.UserName
            };
            var isAddToUserTable = await _userManager.CreateAsync(user, request.Password);

            if (isAddToUserTable.Succeeded)
            {
                var currentUser = await _userManager.FindByNameAsync(user.UserName);
                var roles = request.Roles;
                if (roles != null)
                {
                    await _userManager.AddToRolesAsync(currentUser, roles);
                }
                return new ApiSuccessResult<string>(currentUser.Id.ToString());
            }
            else
            {
                return new ApiErrorResult<string>("Register failed");
            }
        }

        public async Task<ApiResult<PagedResult<UserVm>>> GetUserPaging(GetUserPagingRequest request)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword) || x.PhoneNumber.Contains(request.Keyword));
            }
            // paging
            var totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new UserVm()
                {
                    Id = x.Id,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    LastName = x.LastName
                }).ToListAsync();
            // set result to PageResult and return
            var pageResult = new PagedResult<UserVm>()
            {
                TotalRecord = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            return new ApiSuccessResult<PagedResult<UserVm>>(pageResult);
        }

        public async Task<ApiResult<UserVm>> GetUserInfo(string token)
        {
            var userModel = new UserVm();
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.BaseAddress = new Uri(_config.GetSection("AuthorityUrl")["value"]);
            var response = await client.GetAsync("/connect/userinfo");
            if (response.IsSuccessStatusCode)
            {
                //var a = await response.Content.ReadAsStringAsync();
                //var b = JsonConvert.DeserializeObject<GetUserInfoViewModel>(a);

                var responseModel = await response.Content.ReadFromJsonAsync<GetUserInfoViewModel>();
                userModel.Id = responseModel.sub;
                userModel.UserName = responseModel.preferred_username;
                userModel.Roles = string.Join(";", responseModel.role);
                userModel.FirstName = responseModel.firstname;
                userModel.LastName = responseModel.lastname;
                userModel.BirthDay = DateTime.Parse(responseModel.birthday);
                userModel.Email = responseModel.email;
                userModel.EmailVerified = responseModel.email_verified;
                return new ApiSuccessResult<UserVm>(userModel);
            }
            return new ApiErrorResult<UserVm>("Get user info failed");
        }

        public async Task<ApiResult<string>> Update(Guid id, UserUpdateRequest request)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id))
            {
                return new ApiErrorResult<string>("Email is existed");
            }
            // update user
            var user = await _userManager.FindByIdAsync(id.ToString());
            user.BirthDay = request.BirthDay;
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;

            var updateUserInfoRs = await _userManager.UpdateAsync(user);

            if (updateUserInfoRs.Succeeded)
            {
                // update roles
                var oldRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, oldRoles);

                var isAddToRoleTable = await _userManager.AddToRolesAsync(user, request.Roles);
                if (isAddToRoleTable.Succeeded)
                    return new ApiSuccessResult<string>(user.Id.ToString());
            }
            return new ApiErrorResult<string>("Update failed");
        }

        public async Task<ApiResult<UserVm>> GetUserById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<UserVm>("User invalid");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userVm = new UserVm()
            {
                Id = user.Id,
                Email = user.Email,
                BirthDay = user.BirthDay,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                EmailVerified = user.EmailConfirmed,
                Roles = String.Join(";", roles)
            };

            return new ApiSuccessResult<UserVm>(userVm);
        }

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("User invalid");
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Delete failed");
        }

        public async Task<ApiResult<List<string>>> DeleteMultiple(List<Guid> ids)
        {
            var listIdSuccessed = new List<string>();
            var listIdFailed = new List<string>();
            foreach (Guid id in ids)
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    listIdFailed.Add(id.ToString());
                    continue;
                }
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    listIdFailed.Add(id.ToString());
                    continue;
                }
                listIdSuccessed.Add(id.ToString());
            };
            if (listIdFailed.Count() == ids.Count())
            {
                return new ApiErrorResult<List<string>>("Delete Failed");
            }
            return new ApiSuccessResult<List<string>>(listIdSuccessed);
        }
    }
}