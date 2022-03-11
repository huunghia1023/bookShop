using bookShopSolution.Data.EF;
using bookShopSolution.Data.Entities;
using bookShopSolution.ViewModels.Catalog.Categories;
using bookShopSolution.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly BookShopDbContext _context;

        public CategoryService(BookShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(CategoryCreateRequest request)
        {
            var category = new Category()
            {
                IsShowOnHome = request.IsShowOnHome,
                CategoryTranslations = new List<CategoryTranslation>()
                {
                    new CategoryTranslation()
                    {
                        CategoryName = request.Name,
                        LanguageId = request.LanguageId,
                        SeoAlias = request.SeoAlias,
                        SeoDescription = request.SeoDescription,
                        SeoTitle = request.SeoTitle
                    }
                }
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.Id;
        }

        public async Task<ApiResult<List<int>>> DeleteMultiple(List<int> ids)
        {
            var listCategorySuccessed = new List<Category>();
            var listIdSuccessed = new List<int>();
            foreach (int id in ids)
            {
                var category = await _context.Categories.FindAsync(id);
                if (category != null)
                {
                    listCategorySuccessed.Add(category);
                    listIdSuccessed.Add(id);
                    continue;
                }
            }
            _context.Categories.RemoveRange(listCategorySuccessed);
            await _context.SaveChangesAsync();

            return new ApiSuccessResult<List<int>>(listIdSuccessed);
        }

        public async Task<List<CategoryVm>> GetAll(string languageId)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageId
                        select new { c, ct };
            return await query.Select(x => new CategoryVm
            {
                Id = x.c.Id,
                Name = x.ct.CategoryName,
                IsShowOnHome = x.c.IsShowOnHome,
                SeoAlias = x.ct.SeoAlias,
                SeoDescription = x.ct.SeoDescription,
                SeoTitle = x.ct.SeoTitle
            }).ToListAsync();
        }

        public async Task<CategoryVm> GetCategoryById(int categoryId, string languageId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            var categoryTranslation = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == categoryId && x.LanguageId == languageId);
            var categoryModel = new CategoryVm
            {
                Id = category.Id,
                Name = categoryTranslation.CategoryName,
                IsShowOnHome = category.IsShowOnHome,
                SeoAlias = categoryTranslation.SeoAlias,
                SeoDescription = categoryTranslation.SeoDescription,
                SeoTitle = categoryTranslation.SeoTitle
            };
            return categoryModel;
        }

        public async Task<ApiResult<int>> Update(int id, CategoryUpdateRequest request)
        {
            var category = await _context.Categories.FindAsync(id);
            var categoryTranslation = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == id && x.LanguageId == request.LanguageId);
            if (category == null || categoryTranslation == null)
            {
                return new ApiErrorResult<int>("Category not found");
            }
            category.IsShowOnHome = request.IsShowOnHome;
            categoryTranslation.CategoryName = request.Name;
            categoryTranslation.SeoAlias = request.SeoAlias;
            categoryTranslation.SeoDescription = request.SeoDescription;
            categoryTranslation.SeoTitle = request.SeoTitle;
            var result = await _context.SaveChangesAsync();
            return new ApiSuccessResult<int>();
        }
    }
}