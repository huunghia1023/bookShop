using bookShopSolution.ViewModels.Catalog.IdentityServerResponses;
using bookShopSolution.ViewModels.Catalog.User;
using bookShopSolution.ViewModels.Common;
using bookShopSolution.ViewModels.System.Users;

namespace bookShopSolution.Customer.Services
{
    public interface IUserApiClient
    {
        Task<ApiResult<AuthenticateResponseViewModel>> Authenticate(LoginRequest request);

        Task<GetUserInfoViewModel> GetUserInfo(string accessToken);
    }
}