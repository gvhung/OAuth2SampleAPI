using IdentityServer3.Core.Models;
using PatientJourney.GlobalConstants;
using System.Collections.Generic;


namespace PatientSpectrum.IdentityServer.Config
{
    public static class Clients
    {
        public static IEnumerable<Client> Get()
        {

            return new[]
            {
                new Client
                {
                    ClientId=OAuthPJConstants.MVCClientId,
                    ClientName="Secure MVC Client using Client Credentials Flow",
                    Enabled = true,
                    Flow = Flows.ClientCredentials,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret(OAuthPJConstants.MVCClientSecret.Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        OAuthPJConstants.PJAPIScope,
                        OAuthPJConstants.PJAdminAPIScope
                    },
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = OAuthPJConstants.AccessTokenLifetime
                },
                new Client
                {
                    ClientId=OAuthPJConstants.JavascriptClientId,
                    ClientName="Unsecure Javascript Client using Resource Owner Password Credentials Flow",
                    Enabled = true,
                    Flow = Flows.ResourceOwner,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret(OAuthPJConstants.JavascriptClientSecret.Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        OAuthPJConstants.PJAPIScope,
                        OAuthPJConstants.PJAdminAPIScope
                    },
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = OAuthPJConstants.AccessTokenLifetime
                },
                new Client
                {
                    ClientId = OAuthPJConstants.CustomClientId,
                    ClientName = "Custom Grant Client",
                    Enabled = true,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret(OAuthPJConstants.CustomClientSecret.Sha256()),
                    },

                    Flow = Flows.Custom,
                    AllowedCustomGrantTypes = new List<string>
                    {
                        "custom"
                    },
                    AllowedScopes = new List<string>
                    {
                        OAuthPJConstants.PJAPIScope,
                        OAuthPJConstants.PJAdminAPIScope,
                        StandardScopes.OfflineAccess.Name, //"offline_access" -for refresh tokens
                    },
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = OAuthPJConstants.AccessTokenLifetime
                },
                new Client
                {
                    ClientId = OAuthPJConstants.ROFCClientId,
                    ClientName = "Resource Owner Flow Client",
                    Enabled = true,
                    Flow = Flows.ResourceOwner,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret(OAuthPJConstants.ROFCClientSecret.Sha256())
                    },

                    AllowedScopes = new List<string>
                    {
                        OAuthPJConstants.PJAPIScope,
                        OAuthPJConstants.PJAdminAPIScope,
                        StandardScopes.OfflineAccess.Name
                    },

                    // used by JS resource owner sample
                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:13048"
                    },

                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = OAuthPJConstants.AccessTokenLifetime,

                    // refresh token settings
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AbsoluteRefreshTokenLifetime = 86400,
                    SlidingRefreshTokenLifetime = 43200,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding
                },
            };

        }
    }
}