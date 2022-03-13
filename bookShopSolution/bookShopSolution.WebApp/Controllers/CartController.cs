using bookShopSolution.Utilities.Constants;
using bookShopSolution.WebApp.Models;
using bookShopSolution.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace bookShopSolution.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductApiClient _productApiClient;

        public CartController(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, string languageId)
        {
            var product = await _productApiClient.GetProductById(id, languageId);
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            var quantity = 1;
            if (currentCart.Any(x => x.ProductId == id))
            {
                quantity = currentCart.First(x => x.ProductId == id).Quantity + 1;
            }
            var cartItem = new CartItemViewModel()
            {
                ProductId = id,
                ProductName = product.Name,
                Description = product.Description,
                Quantity = quantity,
                Image = product.Thumbnail
            };
            if (currentCart == null)
            {
                currentCart = new List<CartItemViewModel>();
            }
            currentCart.Add(cartItem);
            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok();
        }
    }
}