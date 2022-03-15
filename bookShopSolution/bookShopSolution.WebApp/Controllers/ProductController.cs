using bookShopSolution.ViewModels.Catalog.Products;
using bookShopSolution.WebApp.Models;
using bookShopSolution.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace bookShopSolution.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IConfiguration _config;

        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient, IConfiguration config)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
            _config = config;
        }

        public async Task<IActionResult> Detail(int id, string culture)
        {
            var productDetail = await _productApiClient.GetProductById(id, culture);
            await _productApiClient.AddViewCount(id);
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

        [HttpPost]
        public async Task<IActionResult> Rating(RatingRequest request)
        {
            var productId = int.Parse(TempData["productId"].ToString());
            var handler = new JwtSecurityTokenHandler();
            var token = HttpContext.Session.GetString("Token");
            var tokenDecode = handler.ReadJwtToken(token);
            if (tokenDecode.Payload.Sub == null)
            {
            }
            request.UserId = Guid.Parse(tokenDecode.Payload.Sub);
            if (!ModelState.IsValid)
            {
                return Redirect($"https://localhost:5001/en/products/{productId}");
                return BadRequest();
            }
            var response = await _productApiClient.Rating(productId, request);
            if (!response.IsSuccessed)
            {
                ModelState.AddModelError("", response.Message);
            }
            ViewData["ratingResult"] = "Rating successed";

            return Redirect($"https://localhost:5001/en/products/{productId}");
        }
    }
}