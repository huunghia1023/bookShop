using bookShopSolution.ViewModels.Catalog.Categories;
using bookShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        public Task<List<CategoryVm>> GetAll(string languageId);

        Task<int> Create(CategoryCreateRequest request);

        Task<CategoryVm> GetCategoryById(int categoryId, string languageId);

        Task<ApiResult<int>> Update(int id, CategoryUpdateRequest request);

        Task<ApiResult<List<int>>> DeleteMultiple(List<int> ids);
    }
}