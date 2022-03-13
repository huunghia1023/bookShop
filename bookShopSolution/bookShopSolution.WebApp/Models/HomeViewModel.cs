using bookShopSolution.ViewModels.Catalog.Categories;
using bookShopSolution.ViewModels.Catalog.Products;

namespace bookShopSolution.WebApp.Models
{
    public class HomeViewModel
    {
        public List<ProductViewModel> featuredProducts { get; set; }
        public List<CategoryVm> categories { get; set; }
        public List<ProductViewModel> productByCategory { get; set; }
        public List<ProductViewModel> latestProducts { get; set; }
    }
}