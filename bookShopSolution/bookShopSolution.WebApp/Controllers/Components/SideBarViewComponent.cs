using bookShopSolution.ViewModels.Catalog.Products;
using bookShopSolution.WebApp.Models;
using bookShopSolution.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace bookShopSolution.WebApp.Controllers.Components
{
    public class SideBarViewComponent : ViewComponent
    {
        private readonly ICategoryApiClient _categoryApiClient;

        public SideBarViewComponent(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<ProductViewModel> topViewProducts)
        {
            TempData["topViewProduct"] = topViewProducts;
            var categories = await _categoryApiClient.GetAllCategory(CultureInfo.CurrentCulture.Name);
            return View(categories);
        }
    }
}