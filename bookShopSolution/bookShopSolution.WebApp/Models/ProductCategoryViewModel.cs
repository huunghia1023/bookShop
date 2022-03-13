using bookShopSolution.ViewModels.Catalog.Categories;
using bookShopSolution.ViewModels.Catalog.Products;
using bookShopSolution.ViewModels.common;

namespace bookShopSolution.WebApp.Models
{
    public class ProductCategoryViewModel
    {
        public CategoryVm Category { get; set; }
        public PagedResult<ProductViewModel> Products { get; set; }
    }
}