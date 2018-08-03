using System;
using Microsoft.Owin;
using Owin;
using IdentityServer3.Core.Configuration;
using PatientSpectrum.IdentityServer.Config;
using System.Security.Cryptography.X509Certificates;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Logging;
using PatientSpectrum.IdentityServer.Services;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Web.Helpers;
using PatientJourney.GlobalConstants;
using Serilog;

[assembly: OwinStartup(typeof(PatientSpectrum.IdentityServer.Startup))]

namespace PatientSpectrum.IdentityServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Log.Logger = new LoggerConfiguration()
                           .MinimumLevel.Debug()
                           .WriteTo.Trace()
                           .CreateLogger();

            AntiForgeryConfig.UniqueClaimTypeIdentifier = IdentityServer3.Core.Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.Map("/identity", identityServer =>
            {
                var corsPolicyService = new DefaultCorsPolicyService()
                {
                    AllowAll = true
                };

                var identityServerFactory = new IdentityServerServiceFactory()
                    .UseInMemoryUsers(users: Users.Get())
                    .UseInMemoryClients(clients: Clients.Get())
                    .UseInMemoryScopes(scopes: Scopes.Get());

                identityServerFactory.ClaimsProvider =
                    new Registration<IClaimsProvider>(typeof(CustomClaimsProvider));

                identityServerFactory.UserService =
                    new Registration<IUserService>(typeof(CustomUserService));

                identityServerFactory.CustomGrantValidators.Add(
                    new Registration<ICustomGrantValidator>(typeof(CustomGrantValidator)));

                identityServerFactory.CorsPolicyService = new
                    Registration<IdentityServer3.Core.Services.ICorsPolicyService>(corsPolicyService);

                var options = new IdentityServerOptions()
                {
                    Factory = identityServerFactory,
                    SiteName = "Patient Journey OAuth Identity Server",
                    PublicOrigin = OAuthPJConstants.OAuthServerPublicOrigin,
                    IssuerUri = OAuthPJConstants.OAuthServerURI,
                    SigningCertificate = LoadCertificate(),
                    RequireSsl = true,
                    InputLengthRestrictions = new InputLengthRestrictions()
                    {
                        UserName = 10000,
                        Password = 10000
                    
                    }
                };

                app.UseIdentityServer(options);
            });
        }

        X509Certificate2 LoadCertificate()
        {
            //return new X509Certificate2(
            //    string.Format(@"{0}\Certificates\idsrvtokencert.pfx",
            //    AppDomain.CurrentDomain.BaseDirectory)
            //);

            return new X509Certificate2(string.Format(@"{0}\Certificates\idsrvtokencert.pfx",
                AppDomain.CurrentDomain.BaseDirectory), "", X509KeyStorageFlags.MachineKeySet);

        }
    }
}
