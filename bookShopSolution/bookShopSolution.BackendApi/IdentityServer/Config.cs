using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace bookShopSolution.BackendApi.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
          new IdentityResource[]
          {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResource("roles", new[] { "role" }),
                new IdentityResource("UserInfo", new List<string>{"firstname", "lastname", "birthday", "role"}),
          };

        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("api1", "My API"),
                new ApiResource("roles", "My Roles", new[] { "role" })
            };

        //new ApiResource[]
        //{
        //    new ApiResource {
        //        Name="api.BackendApi",
        //        ApiSecrets={ new Secret("swagger_RookiesB4_BookShopBackendApi".Sha256()) },

        //        UserClaims =
        //        {
        //            JwtClaimTypes.Name,
        //            JwtClaimTypes.Profile,
        //        },
        //    } // "api.BackendApi", "Backend API"
        //};

        public static IEnumerable<ApiScope> ApiScopes =>
                new ApiScope[]
                {
            new ApiScope(){
                Name = "BackendApiScope",
                DisplayName = "Backend API Scope",
                Enabled = true,
                UserClaims =
                {   JwtClaimTypes.Name,
                    JwtClaimTypes.Email,
                    JwtClaimTypes.Subject,
                    JwtClaimTypes.Role,
                    JwtClaimTypes.Address,
                    JwtClaimTypes.Confirmation,
                    JwtClaimTypes.EmailVerified,
                    JwtClaimTypes.Id,
                    JwtClaimTypes.Profile
                }
            },
                };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "BackendApi",
                    ClientSecrets = { new Secret("RookiesB4_BookShopBackendApi".Sha256()) },//  mã hóa theo Sha256

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequireConsent = false,
                    RequirePkce = true,
                    AllowAccessTokensViaBrowser = false,
                    AllowedCorsOrigins={"https://localhost:5000" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "BackendApiScope"
                    }
                 },
                new Client
                {
                    ClientName = "Swagger Client",
                    ClientId = "swagger",
                    ClientSecrets = { new Secret("swagger_RookiesB4_BookShopBackendApi".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedCorsOrigins = { "https://localhost:5000", "http://localhost:3000" }, // cho phép nguồn gốc cores

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        "BackendApiScope",
                        //"api.BackendApi",
                        "UserInfo",
                        "roles"
                    }
                },
                new Client
                {
                    ClientName = "Customer",
                    ClientId = "customer",
                    ClientSecrets = { new Secret("customer_RookiesB4_BookShop".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedCorsOrigins = { "https://localhost:5000", "http://localhost:3000" }, // cho phép nguồn gốc cores

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        "BackendApiScope",
                        //"api.BackendApi",
                        "UserInfo",
                        "roles"
                    }
                },
            };
    }
}