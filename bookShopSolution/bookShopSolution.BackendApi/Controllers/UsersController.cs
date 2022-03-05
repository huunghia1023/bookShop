using bookShopSolution.Application.System.Users;
using bookShopSolution.ViewModels.Catalog.User;
using bookShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace bookShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Authenticate(request);
            if (result == null)
            {
                return BadRequest("Username or Password is incorrect");
            }
            return Ok(result);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.Register(request);
            if (!result)
            {
                return BadRequest("Register failed");
            }
            return Ok();
        }

        [HttpGet("info")]
        public async Task<UserViewModel> GetUserInfo()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(accessToken))
                return null;
            var user = await _userService.GetUserInfo(accessToken);
            return user;
        }
    }
}