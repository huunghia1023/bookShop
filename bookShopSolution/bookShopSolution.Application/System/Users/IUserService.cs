using bookShopSolution.ViewModels.Catalog.IdentityServerResponses;
using bookShopSolution.ViewModels.Catalog.User;
using bookShopSolution.ViewModels.common;
using bookShopSolution.ViewModels.Common;
using bookShopSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.Application.System.Users
{
    public interface IUserService
    {
        Task<ApiResult<AuthenticateResponseViewModel>> Authenticate(LoginRequest request);

        Task<ApiResult<string>> Register(RegisterRequest request);

        Task<ApiResult<string>> Update(Guid id, UserUpdateRequest request);

        Task<ApiResult<PagedResult<UserVm>>> GetUserPaging(GetUserPagingRequest request);

        Task<ApiResult<UserVm>> GetUserInfo(string token);

        Task<ApiResult<UserVm>> GetUserById(Guid id);

        Task<ApiResult<bool>> Delete(Guid id);

        Task<ApiResult<List<string>>> DeleteMultiple(List<Guid> ids);
    }
}