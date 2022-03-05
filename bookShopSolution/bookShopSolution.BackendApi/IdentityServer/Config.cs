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
                new IdentityResources.Profile()
          };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("api.BackendApi", "Backend API")
            };

        public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
                new ApiScope("api.BackendApi", "Backend API"),
                new ApiScope(IdentityServerConstants.StandardScopes.OpenId),
                new ApiScope(IdentityServerConstants.StandardScopes.Profile)
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
                        "api.BackendApi"
                    }
                 },
                new Client
                {
                    ClientName = "Swagger Client",
                    ClientId = "swagger",
                    ClientSecrets = { new Secret("swagger_RookiesB4_BookShopBackendApi".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedCorsOrigins = { "https://localhost:5000" }, // cho phép nguồn gốc cores

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api.BackendApi"
                    }
                },
            };
    }
}