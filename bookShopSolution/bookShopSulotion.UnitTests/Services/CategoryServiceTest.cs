using bookShopSolution.Application.Catalog.Categories;
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
using Xunit;

namespace bookShopSulotion.UnitTests.Services
{
    public class CategoryServiceTest
    {
        private readonly BookShopDbContext _context;
        private readonly CategoryService _categoryService;

        public CategoryServiceTest()
        {
            _context = GetInmemoryContext();

            _categoryService = new CategoryService(_context);
        }

        ~CategoryServiceTest()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task Create_ReturnSuccess_CreateCategorySuccess()
        {
            // Arrage
            var request = GetCategoryCreateRequest();

            var resultExpect = 1;

            // Act
            var actualResult = await _categoryService.Create(request);

            // Assert
            Assert.Equal(resultExpect, actualResult);
        }

        [Fact]
        public async Task Update_ReturnError_CategoryIdInvalid()
        {
            // Arrage
            var request = GetCategoryUpdateRequest("en");

            var resultExpect = new ApiErrorResult<int>("Category not found"); ;
            var categoryIdInvalid = 10;
            // Act
            var actualResult = await _categoryService.Update(categoryIdInvalid, request);

            // Assert
            Assert.Equal(resultExpect.IsSuccessed, actualResult.IsSuccessed);
            Assert.Equal(resultExpect.Message, actualResult.Message);
        }

        [Fact]
        public async Task Update_ReturnError_CategoryTranslationInvalid()
        {
            // Arrage
            var invalidLanguageId = "fr";
            var request = GetCategoryUpdateRequest(invalidLanguageId);

            var resultExpect = new ApiErrorResult<int>("Category not found");
            var categoryIdValid = 1;
            _context.Categories.Add(new Category()
            {
                Id = 1,
                IsShowOnHome = true,
            });
            await _context.SaveChangesAsync();
            // Act
            var actualResult = await _categoryService.Update(categoryIdValid, request);

            // Assert
            Assert.Equal(resultExpect.IsSuccessed, actualResult.IsSuccessed);
            Assert.Equal(resultExpect.Message, actualResult.Message);
        }

        [Fact]
        public async Task Update_ReturnSuccess_UpdateCategorySuccess()
        {
            // Arrage
            var request = GetCategoryUpdateRequest("en");
            _context.Categories.Add(new Category()
            {
                Id = 1,
                IsShowOnHome = true,
            });
            _context.CategoryTranslations.Add(new CategoryTranslation()
            {
                Id = 1,
                CategoryId = 1,
                CategoryName = "Test",
                LanguageId = "en",
                SeoAlias = "test",
                SeoDescription = "test",
                SeoTitle = "test"
            });
            await _context.SaveChangesAsync();
            var resultExpect = new ApiSuccessResult<int>();

            // Act
            var actualResult = await _categoryService.Update(1, request);

            // Assert
            Assert.Equal(resultExpect.IsSuccessed, actualResult.IsSuccessed);
            Assert.Equal(resultExpect.Message, actualResult.Message);
        }

        private BookShopDbContext GetInmemoryContext()
        {
            var builder = new DbContextOptionsBuilder<BookShopDbContext>();
            builder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            return new BookShopDbContext(builder.Options);
        }

        private CategoryCreateRequest GetCategoryCreateRequest()
        {
            return new CategoryCreateRequest
            {
                IsShowOnHome = true,
                LanguageId = "en",
                Name = "Category Create Test",
            };
        }

        private CategoryUpdateRequest GetCategoryUpdateRequest(string languageId)
        {
            return new CategoryUpdateRequest
            {
                IsShowOnHome = true,
                Name = "test category update",
                LanguageId = languageId
            };
        }
    }
}