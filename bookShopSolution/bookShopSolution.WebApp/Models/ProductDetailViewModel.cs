using bookShopSolution.ViewModels.Catalog.ProductImages;
using bookShopSolution.ViewModels.Catalog.Products;

namespace bookShopSolution.WebApp.Models
{
    public class ProductDetailViewModel
    {
        public ProductViewModel product { get; set; }
        public List<ProductImageViewModel> images { get; set; }
        public RatingRequest ratingRequest { get; set; }
        public int ProductId { get; set; }
    }
}