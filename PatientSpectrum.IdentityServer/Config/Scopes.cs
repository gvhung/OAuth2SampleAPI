using IdentityServer3.Core.Models;
using System.Collections.Generic;
using PatientJourney.GlobalConstants;


namespace PatientSpectrum.IdentityServer.Config
{
    public static class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            return new List<Scope>()
            {
                StandardScopes.OfflineAccess,
                new Scope {
                    Name=OAuthPJConstants.PJAPIScope,
                    DisplayName ="Patient Journey API Resource Scope",
                    Description = "The Clients must request this scope to access the API. Allows applications to manage Patient Journey details on the your behalf.",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role", false)
                    }
                },
                new Scope {
                    Name=OAuthPJConstants.PJAdminAPIScope,
                    DisplayName ="Admin API Scope",
                    Description = "The Clients must request this scope to access the Admin API. Allows applications to manage Admin details on the your behalf.",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role", false)
                    }
                }
            };
        }
    }
}