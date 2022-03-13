using bookShopSolution.ViewModels.Catalog.IdentityServerResponses;
using bookShopSolution.ViewModels.Common;
using bookShopSolution.ViewModels.System.Users;
using Newtonsoft.Json;
using System.Text;

namespace bookShopSolution.WebApp.Services
{
    public class UserApiClient : BaseApiClient, IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public UserApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<ApiResult<AuthenticateResponseViewModel>> Login(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var url = $"/api/Users/authenticate";
            var response = await client.PostAsync(url, httpContent);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<AuthenticateResponseViewModel>>(await response.Content.ReadAsStringAsync());
            }

            return JsonConvert.DeserializeObject<ApiErrorResult<AuthenticateResponseViewModel>>(await response.Content.ReadAsStringAsync());
        }
    }
}