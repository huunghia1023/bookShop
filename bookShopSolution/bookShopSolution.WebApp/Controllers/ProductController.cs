using bookShopSolution.ViewModels.Catalog.Products;
using bookShopSolution.WebApp.Models;
using bookShopSolution.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookShopSolution.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;

        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IActionResult> Detail(int id, string culture)
        {
            var productDetail = await _productApiClient.GetProductById(id, culture);
            var productImages = await _productApiClient.GetAllImages(id);
            return View(new ProductDetailViewModel()
            {
                product = productDetail,
                images = productImages.Items
            });
        }

        public async Task<IActionResult> Category(int id, string culture, int page = 1)
        {
            var products = await _productApiClient.GetProductByCategory(new GetManageProductPagingRequest()
            {
                CategoryId = id,
                Keyword = "",
                LanguageId = culture,
                PageIndex = page,
                PageSize = 9
            });
            var category = await _categoryApiClient.GetCategoryById(culture, id);
            return View(new ProductCategoryViewModel()
            {
                Category = category,
                Products = products
            });
        }
    }
}