using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>()
            {
                new ApiResource("ApiOne", "API #1")
                {
                    Scopes = new[] { "ApiOne" }
                },
                new ApiResource("ApiTwo", "API #2")
                {
                    Scopes = new[] { "ApiTwo" }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>()
            {
                new ApiScope("ApiOne"),
                new ApiScope("ApiTwo"),
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>()
            {
                new Client()
                {
                    ClientId = "#001",
                    ClientSecrets = { new Secret("#001".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "ApiOne" }
                },
                new Client()
                {
                    ClientId = "webappmvc",
                    ClientSecrets = { new Secret("webappmvc".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = new[] { "https://localhost:44353/signin-oidc" },
                    AllowedScopes = { "ApiOne", "ApiTwo", IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile }
                }
            };
    }
}
