using bookShopSolution.ViewModels.Catalog.Categories;
using bookShopSolution.ViewModels.Catalog.ProductImages;
using bookShopSolution.ViewModels.Catalog.ProductRatings;
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

            productDetail.AverageStar = Math.Round(productDetail.AverageStar);

            return View(new ProductDetailViewModel()
            {
                product = productDetail,
                images = productImages != null ? productImages.Items : new List<ProductImageViewModel>()
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
            var category = new CategoryVm();
            if (id != 0)
            {
                category = await _categoryApiClient.GetCategoryById(culture, id);
            }
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
                //await Response.WriteAsync("<script language='javascript'>window.alert('Popup message ')");
                //return RedirectToAction("Detail", new { id = productId, culture = "en" });
                return Redirect($"/en/products/{productId}");
            }
            var response = await _productApiClient.Rating(productId, request, token);
            if (!response.IsSuccessed)
            {
                ModelState.AddModelError("", response.Message);
            }
            ViewData["ratingResult"] = "Rating successed";
            //return RedirectToAction("detail", new { id = productId, culture = "en" });
            return Redirect($"/en/products/{productId}");
        }
    }
}