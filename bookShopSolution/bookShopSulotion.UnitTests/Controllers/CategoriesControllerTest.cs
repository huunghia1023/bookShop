using bookShopSolution.Application.Catalog.Categories;
using bookShopSolution.BackendApi.Controllers;
using bookShopSolution.Data.Entities;
using bookShopSolution.ViewModels.Catalog.Categories;
using bookShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace bookShopSulotion.UnitTests.Controllers
{
    public class CategoriesControllerTest
    {
        [Fact]
        public async void GetById_ReturnsBadRequest_InvalidLanguageIdOrCategoryId()
        {
            // Arrange

            var mockCategoryService = new Mock<ICategoryService>();
            mockCategoryService.Setup(sv => sv.GetCategoryById(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(It.IsAny<CategoryVm>());
            var controller = new CategoriesController(mockCategoryService.Object);

            // Act
            var actionResult = await controller.GetById(It.IsAny<int>(), It.IsAny<string>());

            // Assert

            Assert.IsType<BadRequestObjectResult>(actionResult);
            var message = Assert.IsType<BadRequestObjectResult>(actionResult).Value;
            Assert.Equal("Can not find category", message);
        }

        [Fact]
        public async void GetById_ReturnsOk_ValidLanguageIdAndCategoryId()
        {
            // Arrange
            var categoryId = 1;
            var languageId = "en";
            CategoryVm category = new CategoryVm()
            {
                Id = 1,
                Name = "test",
                LanguageId = languageId,
                IsShowOnHome = true
            };
            var mockCategoryService = new Mock<ICategoryService>();
            mockCategoryService.Setup(sv => sv.GetCategoryById(categoryId, languageId)).ReturnsAsync(category);
            var controller = new CategoriesController(mockCategoryService.Object);

            // Act
            var actionResult = await controller.GetById(categoryId, languageId);

            // Assert

            Assert.IsType<OkObjectResult>(actionResult);
            var result = Assert.IsType<OkObjectResult>(actionResult).Value;
            Assert.Equal(category, result);
        }

        [Fact]
        public async void Create_ReturnsBadRequest_InvalidModel()
        {
            // Arrange
            var request = new CategoryCreateRequest()
            {
                Name = "",
                IsShowOnHome = true,
                LanguageId = "en"
            };
            var mockCategoryService = new Mock<ICategoryService>();
            mockCategoryService.Setup(sv => sv.Create(request)).ReturnsAsync(It.IsAny<int>());
            var controller = new CategoriesController(mockCategoryService.Object);
            controller.ModelState.AddModelError("error", "Name is required");
            // Act
            var actionResult = await controller.Create(request);

            // Assert

            Assert.IsType<BadRequestObjectResult>(actionResult);
            var messages = Assert.IsType<BadRequestObjectResult>(actionResult).Value;
            var messagesText = JsonConvert.SerializeObject(messages);
            Assert.Contains("Name is required", messagesText);
        }

        [Fact]
        public async void Create_ReturnsBadRequest_FailedCreate()
        {
            // Arrange
            var request = new CategoryCreateRequest()
            {
                Name = "Test",
                IsShowOnHome = true,
                LanguageId = "en"
            };
            var mockCategoryService = new Mock<ICategoryService>();
            mockCategoryService.Setup(sv => sv.Create(request)).ReturnsAsync(0);
            var controller = new CategoriesController(mockCategoryService.Object);
            // Act
            var actionResult = await controller.Create(request);

            // Assert

            Assert.IsType<BadRequestObjectResult>(actionResult);
            var messages = Assert.IsType<BadRequestObjectResult>(actionResult).Value;

            Assert.Equal("Create failed", messages);
        }

        [Fact]
        public async void Create_ReturnsCreatedAtAction_SuccessedCreate()
        {
            // Arrange
            var request = new CategoryCreateRequest()
            {
                Name = "Test",
                IsShowOnHome = true,
                LanguageId = "en"
            };
            var mockCategoryService = new Mock<ICategoryService>();
            mockCategoryService.Setup(sv => sv.Create(request)).ReturnsAsync(1);
            var controller = new CategoriesController(mockCategoryService.Object);
            // Act
            var actionResult = await controller.Create(request);

            // Assert

            Assert.IsType<CreatedAtActionResult>(actionResult);
            var resultObj = Assert.IsType<CreatedAtActionResult>(actionResult).Value;
            var resultText = JsonConvert.SerializeObject(resultObj);
            Assert.Contains("{\"id\":1}", resultText);
        }

        [Fact]
        public async void Update_ReturnsBadRequest_InvalidModel()
        {
            // Arrange
            var request = new CategoryUpdateRequest()
            {
                Name = "",
                IsShowOnHome = true,
                LanguageId = "en"
            };
            var mockCategoryService = new Mock<ICategoryService>();
            mockCategoryService.Setup(sv => sv.Update(It.IsAny<int>(), request)).ReturnsAsync(It.IsAny<ApiResult<int>>);
            var controller = new CategoriesController(mockCategoryService.Object);
            controller.ModelState.AddModelError("error", "Name is required");
            // Act
            var actionResult = await controller.Update(It.IsAny<int>(), request);

            // Assert

            Assert.IsType<BadRequestObjectResult>(actionResult);
            var resultObj = Assert.IsType<BadRequestObjectResult>(actionResult).Value;
            var resultText = JsonConvert.SerializeObject(resultObj);
            Assert.Contains("Name is required", resultText);
        }

        [Fact]
        public async void Update_ReturnsBadRequest_FailedUpdate()
        {
            // Arrange
            var request = new CategoryUpdateRequest()
            {
                Name = "Test",
                IsShowOnHome = true,
                LanguageId = "en"
            };
            var actualResult = new ApiErrorResult<int>("Category not found");
            var mockCategoryService = new Mock<ICategoryService>();
            mockCategoryService.Setup(sv => sv.Update(1, request)).ReturnsAsync(actualResult);
            var controller = new CategoriesController(mockCategoryService.Object);
            // Act
            var actionResult = await controller.Update(1, request);

            // Assert

            Assert.IsType<BadRequestObjectResult>(actionResult);
            var resultObj = Assert.IsType<BadRequestObjectResult>(actionResult).Value;
            var resultText = JsonConvert.SerializeObject(resultObj);
            Assert.Contains("Category not found", resultText);
        }

        [Fact]
        public async void Update_ReturnsOk_SuccessedUpdate()
        {
            // Arrange
            var request = new CategoryUpdateRequest()
            {
                Name = "Test",
                IsShowOnHome = true,
                LanguageId = "en"
            };
            var actualResult = new ApiSuccessResult<int>();
            var mockCategoryService = new Mock<ICategoryService>();
            mockCategoryService.Setup(sv => sv.Update(1, request)).ReturnsAsync(actualResult);
            var controller = new CategoriesController(mockCategoryService.Object);
            // Act
            var actionResult = await controller.Update(1, request);

            // Assert

            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Fact]
        public async void DeleteMultiple_ReturnsBadRequest_FailedDelete()
        {
            // Arrange
            var ids = new List<int>()
            {
                1,2,3
            };
            var actualResult = new ApiErrorResult<List<int>>("Delete failed");
            var mockCategoryService = new Mock<ICategoryService>();
            mockCategoryService.Setup(sv => sv.DeleteMultiple(ids)).ReturnsAsync(actualResult);
            var controller = new CategoriesController(mockCategoryService.Object);
            // Act
            var actionResult = await controller.DeleteMultiple(ids);

            // Assert

            Assert.IsType<BadRequestObjectResult>(actionResult);
            var resultObj = Assert.IsType<BadRequestObjectResult>(actionResult).Value;
            var resultText = JsonConvert.SerializeObject(resultObj);
            Assert.Contains("Delete failed", resultText);
        }

        [Fact]
        public async void DeleteMultiple_ReturnsOk_SuccessedDelete()
        {
            // Arrange
            var ids = new List<int>()
            {
                1,2,3
            };
            var actualResult = new ApiSuccessResult<List<int>>(ids);
            var mockCategoryService = new Mock<ICategoryService>();
            mockCategoryService.Setup(sv => sv.DeleteMultiple(ids)).ReturnsAsync(actualResult);
            var controller = new CategoriesController(mockCategoryService.Object);
            // Act
            var actionResult = await controller.DeleteMultiple(ids);

            // Assert

            Assert.IsType<OkObjectResult>(actionResult);
            var resultObj = Assert.IsType<OkObjectResult>(actionResult).Value;
            var resultText = JsonConvert.SerializeObject(resultObj);
            Assert.Contains(string.Join(",", ids), resultText);
        }
    }
}