using bookShopSolution.ViewModels.Catalog.ProductImages;
using bookShopSolution.ViewModels.Catalog.ProductRatings;
using bookShopSolution.ViewModels.Catalog.Products;
using bookShopSolution.ViewModels.common;
using bookShopSolution.ViewModels.Common;

namespace bookShopSolution.WebApp.Services
{
    public interface IProductApiClient
    {
        Task<List<ProductViewModel>> GetFeaturedProduct(int take, string languageId);

        Task<PagedResult<ProductViewModel>> GetProductByCategory(GetManageProductPagingRequest request);

        Task<List<ProductViewModel>> GetLatestProduct(int take, string languageId);

        Task<List<ProductViewModel>> GetTopViewProduct(int take, string languageId);

        Task<PagedResult<ProductImageViewModel>> GetAllImages(int id);

        Task<ProductViewModel> GetProductById(int id, string languageId);

        Task AddViewCount(int productId);

        Task<ApiResult<bool>> Rating(int productId, RatingRequest request, string token);

        Task<List<ProductRatingViewModel>> GetAllRating(int product, int take);
    }
}