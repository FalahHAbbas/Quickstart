using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;

namespace IdentityServer {
    public static class Config {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope> {new ApiScope("cinema", "My API")};

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[] {new IdentityResources.OpenId()};

        public static IEnumerable<Client> Clients => new List<Client> {
            new Client {
                ClientId = "client",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets = {new Secret("secret".Sha256())},

                // scopes that client has access to
                AllowedScopes = {"api1", "cinema"}
            },
            // JavaScript Client
            new Client {
                ClientId = "js",
                ClientName = "JavaScript Client",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = {"http://localhost:5003/callback.html"},
                PostLogoutRedirectUris = {"http://localhost:5003/index.html"},
                AllowedCorsOrigins = {"http://localhost:5003"},
                AllowedScopes = {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api1"
                }
            }
        };
    }
}