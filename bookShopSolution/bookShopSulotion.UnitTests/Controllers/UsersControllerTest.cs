using bookShopSolution.Application.System.Users;
using bookShopSolution.BackendApi.Controllers;
using bookShopSolution.ViewModels.Catalog.IdentityServerResponses;
using bookShopSolution.ViewModels.Common;
using bookShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace bookShopSulotion.UnitTests
{
    public class UsersControllerTest
    {
        [Fact]
        public async void Authenticate_ReturnsBadRequest_InvalidModel()
        {
            // Arrange

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(sv => sv.Authenticate(It.IsAny<LoginRequest>())).ReturnsAsync(It.IsAny<ApiResult<AuthenticateResponseViewModel>>());
            var controller = new UsersController(mockUserService.Object);
            controller.ModelState.AddModelError("error", "Input invalid");
            // Act
            var actionResult = await controller.Authenticate(It.IsAny<LoginRequest>());

            // Assert

            Assert.IsType<BadRequestObjectResult>(actionResult);

            var resultObj = Assert.IsType<BadRequestObjectResult>(actionResult).Value;
            var resultText = JsonConvert.SerializeObject(resultObj);
            Assert.Contains("Input invalid", resultText);
        }

        [Fact]
        public async void Authenticate_ReturnsBadRequest_FailedAuthenticate()
        {
            // Arrange
            var request = new LoginRequest()
            {
                UserName = "test",
                Password = "test",
                RememberMe = true,
                IsFromAdmin = true
            };
            var actualResult = new ApiErrorResult<AuthenticateResponseViewModel>()
            {
                IsSuccessed = false,
                Message = "Access denied"
            };
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(sv => sv.Authenticate(request)).ReturnsAsync(actualResult);
            var controller = new UsersController(mockUserService.Object);
            // Act
            var actionResult = await controller.Authenticate(request);

            // Assert

            Assert.IsType<BadRequestObjectResult>(actionResult);

            var resultObj = Assert.IsType<BadRequestObjectResult>(actionResult).Value;
            var resultText = JsonConvert.SerializeObject(resultObj);
            Assert.Contains("Access denied", resultText);
        }

        [Fact]
        public async void Authenticate_ReturnsOk_SuccessedAuthenticate()
        {
            // Arrange
            var request = new LoginRequest()
            {
                UserName = "test",
                Password = "test",
                RememberMe = true,
                IsFromAdmin = false
            };
            var actualResult = new ApiSuccessResult<AuthenticateResponseViewModel>()
            {
                IsSuccessed = true,
                Results = new AuthenticateResponseViewModel()
                {
                    access_token = "access_token",
                    expires_in = 3600,
                    scope = "scope",
                    token_type = "Bearer"
                }
            };
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(sv => sv.Authenticate(request)).ReturnsAsync(actualResult);
            var controller = new UsersController(mockUserService.Object);
            // Act
            var actionResult = await controller.Authenticate(request);

            // Assert

            Assert.IsType<OkObjectResult>(actionResult);

            var resultObj = Assert.IsType<OkObjectResult>(actionResult).Value;
            var resultText = JsonConvert.SerializeObject(resultObj);
            Assert.Contains("access_token", resultText);
        }
    }
}