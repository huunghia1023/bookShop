using bookShopSolution.ViewModels.Catalog.IdentityServerResponses;
using bookShopSolution.ViewModels.Catalog.User;
using bookShopSolution.ViewModels.System.Users;

namespace bookShopSolution.Customer.Services
{
    public interface IUserApiClient
    {
        Task<AuthenticateResponseViewModel> Authenticate(LoginRequest request);

        Task<UserViewModel> GetUserInfo(string accessToken);
    }
}