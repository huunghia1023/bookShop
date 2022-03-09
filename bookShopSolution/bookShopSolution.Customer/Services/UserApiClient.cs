using bookShopSolution.ViewModels.Catalog.IdentityServerResponses;
using bookShopSolution.ViewModels.Catalog.User;
using bookShopSolution.ViewModels.System.Users;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace bookShopSolution.Customer.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<AuthenticateResponseViewModel> Authenticate(LoginRequest request)
        {
            var responseViewModel = new AuthenticateResponseViewModel();
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5000");
            var response = await client.PostAsync("/api/users/authenticate", httpContent);
            if (response.IsSuccessStatusCode)
            {
                responseViewModel = await response.Content.ReadFromJsonAsync<AuthenticateResponseViewModel>();
            }
            //var token = await response.Content.ReadAsStringAsync();
            //var tokenJson = new Object();
            //if (response.IsSuccessStatusCode)
            //{
            //    using var contentStream =
            //        await response.Content.ReadAsStreamAsync();
            //    var serializer = new JsonSerializer();
            //    using (var sr = new StreamReader(contentStream))
            //    using (var jsonTextReader = new JsonTextReader(sr))
            //    {
            //        tokenJson = serializer.Deserialize(jsonTextReader);
            //    }
            //}
            return responseViewModel;
        }

        public async Task<GetUserInfoViewModel> GetUserInfo(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                return null;
            var user = new GetUserInfoViewModel();
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri("https://localhost:5000");

            //client.DefaultRequestHeaders.Add("Authorization", bearerToken);
            var response = await client.GetAsync("/api/users/info");
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadFromJsonAsync<GetUserInfoViewModel>();
            }
            return user;
        }
    }
}