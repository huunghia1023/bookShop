using bookShopSolution.ViewModels.Catalog.Products;
using bookShopSolution.ViewModels.Common;
using bookShopSolution.WebApp.Models;
using bookShopSolution.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace bookShopSolution.WebApp.Controllers.Components
{
    public class RatingViewComponent : ViewComponent
    {
        private readonly IProductApiClient _productApiClient;

        public RatingViewComponent(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            //var productId = int.Parse(TempData["productId"].ToString());
            TempData["productId"] = productId;
            var ratings = await _productApiClient.GetAllRating(productId, 10);
            return View(new ProductReviewViewModel()
            {
                Reviews = ratings
            });
        }
    }
}