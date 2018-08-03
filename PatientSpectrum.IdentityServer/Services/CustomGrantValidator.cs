using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Validation;
using System.Threading.Tasks;

namespace PatientSpectrum.IdentityServer.Services
{
    public class CustomGrantValidator : ICustomGrantValidator
    {
        private IUserService _users;

        public CustomGrantValidator(IUserService users)
        {
            _users = users;
        }

        public async Task<CustomGrantValidationResult> ValidateAsync(ValidatedTokenRequest request)
        {
            var useremail = request.Raw.Get("useremail");
            var id = request.Raw.Get("user511id");
            var secret = string.Empty;

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(useremail))
            {
                return
                    new CustomGrantValidationResult("Missing parameters.");
            }

            var message = new SignInMessage { Tenant = useremail };
            var context = new LocalAuthenticationContext
            {
                UserName = id,
                Password = secret,
                SignInMessage = message
            };
            await _users.AuthenticateLocalAsync(context);

            var result = context.AuthenticateResult;
            if (result.IsError)
            {
                return
                    new CustomGrantValidationResult("Authentication failed.");
            }

            return new CustomGrantValidationResult(
                result.User.GetSubjectId(),
                "password",
                result.User.Claims);
        }

        public string GrantType
        {
            get { return "password"; }
        }
    }
}