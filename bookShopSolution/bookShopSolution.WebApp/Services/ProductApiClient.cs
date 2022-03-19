using bookShopSolution.Utilities.Constants;
using bookShopSolution.ViewModels.Catalog.ProductImages;
using bookShopSolution.ViewModels.Catalog.ProductRatings;
using bookShopSolution.ViewModels.Catalog.Products;
using bookShopSolution.ViewModels.common;
using bookShopSolution.ViewModels.Common;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace bookShopSolution.WebApp.Services
{
    public class ProductApiClient : BaseApiClient, IProductApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public ProductApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)

        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task AddViewCount(int productId)
        {
            var url = $"/api/products/{productId}/addview";
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            await client.GetAsync(url);
        }

        public async Task<PagedResult<ProductImageViewModel>> GetAllImages(int id)
        {
            var url = $"/api/products/{id}/images";
            var data = await GetAsync<PagedResult<ProductImageViewModel>>(url);
            return data;
        }

        public async Task<List<ProductViewModel>> GetFeaturedProduct(int take, string languageId)
        {
            var url = $"/api/products/featured/{languageId}/{take}";
            var data = await GetListAsync<ProductViewModel>(url);
            return data;
        }

        public async Task<List<ProductViewModel>> GetLatestProduct(int take, string languageId)
        {
            var url = $"/api/products/latest/{languageId}/{take}";
            var data = await GetListAsync<ProductViewModel>(url);
            return data;
        }

        public async Task<List<ProductViewModel>> GetTopViewProduct(int take, string languageId)
        {
            var url = $"/api/products/viewest/{languageId}/{take}";
            var data = await GetListAsync<ProductViewModel>(url);
            return data;
        }

        public async Task<PagedResult<ProductViewModel>> GetProductByCategory(GetManageProductPagingRequest request)
        {
            var url = $"/api/products/paging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                $"&keyword={request.Keyword}&languageId={request.LanguageId}&categoryId={request.CategoryId}";
            var data = await GetAsync<PagedResult<ProductViewModel>>(url);
            return data;
        }

        public async Task<ProductViewModel> GetProductById(int id, string languageId)
        {
            var url = $"/api/products/{id}/{languageId}";
            var data = await GetAsync<ProductViewModel>(url);
            return data;
        }

        public async Task<ApiResult<bool>> Rating(int productId, RatingRequest request, string token)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.PostAsync($"/api/products/{productId}/rating", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<List<ProductRatingViewModel>> GetAllRating(int id, int take)
        {
            var url = $"/api/products/{id}/rating/{take}";
            var data = await GetAsync<List<ProductRatingViewModel>>(url);
            return data;
        }
    }
}