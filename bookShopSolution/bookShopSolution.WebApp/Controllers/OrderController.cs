using bookShopSolution.Utilities.Constants;
using bookShopSolution.ViewModels.Catalog.Orders;
using bookShopSolution.WebApp.Models;
using bookShopSolution.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace bookShopSolution.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderApiClient _orderApiClient;

        public OrderController(IOrderApiClient orderApiClient)
        {
            _orderApiClient = orderApiClient;
        }

        public async Task<IActionResult> Index(string id)
        {
            var order = new OrderViewModel();
            if (id != "all")
            {
                order = await _orderApiClient.GetById(int.Parse(id));
            }
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _orderApiClient.CancelOrder(id);

            return Ok(result);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetById(int id, string culture)
        //{
        //    culture = "en";
        //    var order = await _orderApiClient.GetById(id, culture);
        //    return RedirectToAction("index", order);
        //}

        //[HttpPost]
        //[Route("orders/Create")]
        //public async Task<IActionResult> Create(OrderCreateRequest request)
        //{
        //    request.Carts = new List<CartViewModel>();
        //    var session = HttpContext.Session.GetString(SystemConstants.CartSession);
        //    List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
        //    if (session != null)
        //        currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
        //    foreach (var cart in currentCart)
        //    {
        //        var orderCart = new CartViewModel()
        //        {
        //            ProductId = cart.ProductId,
        //            Quantity = cart.Quantity
        //        };
        //        request.Carts.Add(orderCart);
        //    }

        //    var token = HttpContext.Session.GetString("Token");
        //    if (token == null)
        //    {
        //        request.UserId = "";
        //    }
        //    else
        //    {
        //        var handler = new JwtSecurityTokenHandler();
        //        var tokenDecode = handler.ReadJwtToken(token);
        //        if (tokenDecode.Payload.Sub == null)
        //        {
        //            request.UserId = "";
        //        }
        //        request.UserId = tokenDecode.Payload.Sub;
        //    }

        //    var order = await _orderApiClient.Create(request);
        //    var orderId = 0;
        //    if (order == null)
        //    {
        //        TempData["checkoutResult"] = "Error";
        //    }
        //    else
        //    {
        //        orderId = order.Id;
        //        TempData["checkoutResult"] = "Success";
        //    }
        //    return RedirectToAction("index", new { id = orderId });
        //}
    }
}