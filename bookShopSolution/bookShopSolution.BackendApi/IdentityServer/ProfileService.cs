using bookShopSolution.Data.Entities;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace bookShopSolution.BackendApi.IdentityServer
{
    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsFactory;
        private readonly UserManager<AppUser> _userManager;

        public ProfileService(
            UserManager<AppUser> userManager,
            IUserClaimsPrincipalFactory<AppUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.IssuedClaims.AddRange(context.Subject.Claims);

            var user = await _userManager.GetUserAsync(context.Subject);

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, role));
            }
            //var sub = context.Subject.GetSubjectId();
            //var user = await _userManager.FindByIdAsync(sub);

            //var principal = await _claimsFactory.CreateAsync(user);

            //var claims = principal.Claims.ToList();

            //claims.AddRange(new List<Claim>()
            //{
            //    new Claim("firstname", user.FirstName),
            //    new Claim("lastname", user.LastName),
            //    new Claim("birthday", user.BirthDay.ToString())
            //});

            //// specific role of user
            //var roles = await _userManager.GetRolesAsync(user);

            ////claims.Add(new Claim(JwtClaimTypes.Role, String.Join(";", roles)));

            ////
            //claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            ////if (claims == null)
            ////    throw new ECommerceException("Don't have claim to request");

            //context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}