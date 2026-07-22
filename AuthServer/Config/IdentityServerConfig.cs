using Duende.IdentityServer.Models;

namespace AuthServer.Config
{
    public static class IdentityServerConfig
    {
        //my_api,client_app,api.read,these are the keys these only validated in token json

        public static IEnumerable<ApiScope> ApiScopes =>
    new List<ApiScope>
    {
    new ApiScope("assessment.read", "Read assessment data")
    {
        UserClaims = { "role" }
    },
    new ApiScope("assessment.write", "Write assessment data")
    {
        UserClaims = { "role" }
    },
    new ApiScope("exam.start", "Start exam")
    {
        UserClaims = { "role" }
    }
    };


        public static IEnumerable<ApiResource> ApiResources =>
new List<ApiResource>
{
    new ApiResource("assessment_api", "Assessment Protected API")
    {
        Scopes =
        {
            "assessment.read",
            "assessment.write",
            "exam.start"
        },
        UserClaims = { "role" }
    }
};


        //public static IEnumerable<Client> Clients =>
        //    new List<Client>
        //    {
        //    new Client
        //    {
        //        ClientId = "client_app",
        //         ClientName = "My Angular App",
        //        ClientSecrets = { new Secret("secret".Sha256()) },

        //        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
        //        AllowedScopes = { "api.read", "api.write" },

        //        AllowOfflineAccess = true // refresh token
        //    }
        //    };


        public static IEnumerable<Client> Clients =>
  new List<Client>
  {
    new Client
    {
        ClientId = "assessment_spa",
        ClientName = "Assessment Blazor WASM",

        AllowedGrantTypes = GrantTypes.Code,
        RequirePkce = true,
        RequireClientSecret = false,

      RedirectUris =
{
    "https://localhost:7065/auth-callback"
},
PostLogoutRedirectUris =
{
    "https://localhost:7065"
},
 AllowedCorsOrigins =
    {
        "https://localhost:7065"
    },
        AllowedScopes =
        {
            "openid",
            "profile",
            "assessment.read",
            "assessment.write",
            "exam.start",
            "offline_access"
        },
        AllowOfflineAccess = true,   // ⭐ REQUIRED
    RefreshTokenUsage = TokenUsage.OneTimeOnly,//Rotation
    RefreshTokenExpiration = TokenExpiration.Sliding,
    SlidingRefreshTokenLifetime = 1296000, // 15 days
   // AbsoluteRefreshTokenLifetime = 604800 // 7 days
    AccessTokenLifetime = 300, // 5 minutes (real-world)
        
        AllowAccessTokensViaBrowser = true
    }
  };





        public static IEnumerable<IdentityResource> IdentityResources =>
    new List<IdentityResource>
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),

    };

    }
}
//my_api give here is a audience it will check the token is issued for what.needs to match with api program cs