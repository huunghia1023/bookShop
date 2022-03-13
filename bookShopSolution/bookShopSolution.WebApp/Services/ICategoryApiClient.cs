using bookShopSolution.ViewModels.Catalog.Categories;

namespace bookShopSolution.WebApp.Services
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryVm>> GetAllCategory(string languageId);

        Task<CategoryVm> GetCategoryById(string languageId, int id);
    }
}