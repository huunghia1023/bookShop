using bookShopSolution.ViewModels.Catalog.Categories;

namespace bookShopSolution.WebApp.Services
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public CategoryApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)

        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<List<CategoryVm>> GetAllCategory(string languageId)
        {
            var url = $"api/Categories?LanguageId={languageId}";
            var data = await GetListAsync<CategoryVm>(url);
            return data;
        }

        public async Task<CategoryVm> GetCategoryById(string languageId, int id)
        {
            var url = $"api/Categories/{id}/{languageId}";
            var data = await GetAsync<CategoryVm>(url);
            return data;
        }
    }
}