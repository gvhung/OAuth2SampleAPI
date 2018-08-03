using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.Core.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace PatientSpectrum.IdentityServer.Services
{
    class CustomClaimsProvider : DefaultClaimsProvider
    {
        public CustomClaimsProvider(IUserService users) : base(users)
        { }

        public override async Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ClaimsPrincipal subject, Client client, IEnumerable<Scope> scopes, ValidatedRequest request)
        {
            var claims = await base.GetAccessTokenClaimsAsync(subject, client, scopes, request);

            var newClaims = claims.ToList();
            if (subject != null)
            {
                newClaims.Add(subject.FindFirst("user511id"));
                newClaims.Add(subject.FindFirst("userrole"));
                newClaims.Add(subject.FindFirst("greetingname"));
                newClaims.Add(subject.FindFirst("useremail"));
                newClaims.Add(subject.FindFirst("encrypted511Id"));
                newClaims.Add(subject.FindFirst("userroleid"));
                newClaims.Add(subject.FindFirst("usercountry"));
                newClaims.Add(subject.FindFirst("isvaliduser"));
                newClaims.Add(subject.FindFirst("name"));
            }
            return newClaims;
        }
    }
}