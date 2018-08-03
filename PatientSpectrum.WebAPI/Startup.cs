using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Http.Cors;
using PatientJourney.GlobalConstants;
using Serilog;
using PatientSpectrum.WebAPI.Exception_Handler;
using PatientSpectrum.WebAPI.Helper;

[assembly: OwinStartup(typeof(PatientSpectrum.WebAPI.Startup))]

namespace PatientSpectrum.WebAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Log.Logger = new LoggerConfiguration()
                           .ReadFrom.AppSettings()
                           .CreateLogger();
            
            app.UseIdentityServerBearerTokenAuthentication(
                new IdentityServer3.AccessTokenValidation.IdentityServerBearerTokenAuthenticationOptions
                {
                    Authority = OAuthPJConstants.OAuthServerPublicOrigin,
                    RequiredScopes = new string[] { OAuthPJConstants.PJAPIScope }

                });

            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                "default",
                "api/{controller}/{action}");

            config.EnableCors(new EnableCorsAttribute("http://localhost:28149", "*", "GET, POST, OPTIONS, PUT, DELETE", "PreflightMaxAge=600"));

            config.Filters.Add(new PatientSpectrumExceptionFilter());

            //log every request and response including headers. 
            config.MessageHandlers.Add(new LogRequestAndResponseHandler());

            app.UseWebApi(config);
        }
    }
}
