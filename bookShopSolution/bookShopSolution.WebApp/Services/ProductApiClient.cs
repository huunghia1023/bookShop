using bookShopSolution.Utilities.Constants;
using bookShopSolution.ViewModels.Catalog.ProductImages;
using bookShopSolution.ViewModels.Catalog.Products;
using bookShopSolution.ViewModels.common;

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

        public async Task<PagedResult<ProductViewModel>> GetProductByCategory(GetManageProductPagingRequest request)
        {
            var url = $"/api/products/paging?pageindex={request.PageIndex}&pagesize={request.PageSize}&keyword={request.Keyword}&languageid={request.LanguageId}&categoryid={request.CategoryId}";
            var data = await GetAsync<PagedResult<ProductViewModel>>(url);
            return data;
        }

        public async Task<ProductViewModel> GetProductById(int id, string languageId)
        {
            var url = $"/api/products/{id}/{languageId}";
            var data = await GetAsync<ProductViewModel>(url);
            return data;
        }
    }
}