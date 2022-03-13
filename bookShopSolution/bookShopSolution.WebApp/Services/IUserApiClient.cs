using bookShopSolution.ViewModels.Catalog.IdentityServerResponses;
using bookShopSolution.ViewModels.Common;
using bookShopSolution.ViewModels.System.Users;

namespace bookShopSolution.WebApp.Services
{
    public interface IUserApiClient
    {
        Task<ApiResult<AuthenticateResponseViewModel>> Login(LoginRequest request);
    }
}