using bookShopSolution.ViewModels.Catalog.IdentityServerResponses;
using bookShopSolution.ViewModels.Catalog.User;
using bookShopSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.Application.System.Users
{
    public interface IUserService
    {
        Task<AuthenticateResponseViewModel> Authenticate(LoginRequest request);

        Task<bool> Register(RegisterRequest request);

        Task<UserViewModel> GetUserInfo(string accessToken);
    }
}