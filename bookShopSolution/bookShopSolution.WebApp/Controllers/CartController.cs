using bookShopSolution.Utilities.Constants;
using bookShopSolution.ViewModels.Catalog.Orders;
using bookShopSolution.WebApp.Models;
using bookShopSolution.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace bookShopSolution.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IOrderApiClient _orderApiClient;

        public CartController(IProductApiClient productApiClient, IOrderApiClient orderApiClient)
        {
            _productApiClient = productApiClient;
            _orderApiClient = orderApiClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetListItems()
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            return Ok(currentCart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, int quantity, string languageId)
        {
            var product = await _productApiClient.GetProductById(id, languageId);
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);

            foreach (CartItemViewModel item in currentCart)
            {
                if (item.ProductId == id)
                {
                    item.Quantity += quantity;
                    HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
                    return Ok(currentCart);
                }
            }

            var cartItem = new CartItemViewModel()
            {
                ProductId = id,
                ProductName = product.Name,
                Description = product.Description,
                Quantity = quantity,
                Image = product.Thumbnail,
                Price = product.Price
            };
            currentCart.Add(cartItem);
            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }

        [HttpPost]
        public IActionResult UpdateCart(int id, int quantity)
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);

            foreach (CartItemViewModel item in currentCart)
            {
                if (item.ProductId == id)
                {
                    if (quantity == 0)
                    {
                        currentCart.Remove(item);
                        break;
                    }
                    item.Quantity = quantity;
                }
            }
            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderCreateRequest request)
        {
            request.Carts = new List<CartViewModel>();
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            foreach (var cart in currentCart)
            {
                var orderCart = new CartViewModel()
                {
                    ProductId = cart.ProductId,
                    Quantity = cart.Quantity
                };
                request.Carts.Add(orderCart);
            }

            var token = HttpContext.Session.GetString("Token");
            if (token == null)
            {
                request.UserId = "";
            }
            else
            {
                var handler = new JwtSecurityTokenHandler();
                var tokenDecode = handler.ReadJwtToken(token);
                if (tokenDecode.Payload.Sub == null)
                {
                    request.UserId = "";
                }
                request.UserId = tokenDecode.Payload.Sub;
            }

            var response = await _orderApiClient.Create(request);

            return View();
        }
    }
}