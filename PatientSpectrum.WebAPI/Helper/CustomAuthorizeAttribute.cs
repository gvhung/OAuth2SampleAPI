using PatientJourney.GlobalConstants;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace PatientSpectrum.WebAPI.Helper
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        string[] _roles;
       
        public string[] UserRoles
        {
            get { return _roles; }
        }

        public CustomAuthorizeAttribute(params string[] roles)
        {
            if (roles == null)
            {
                throw new ArgumentNullException("roles missing");
            }

            _roles = roles;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var principal = actionContext.ControllerContext.RequestContext.Principal as ClaimsPrincipal;

            if (principal == null)
            {
                return false;
            }

            var claims = principal.Claims.ToClaimsDictionary();

            if (claims.ContainsKey(OAuthPJConstants.Role))
            {
                string UserRole = claims[OAuthPJConstants.Role].ToString();
                bool roleaccess=RoleAuthorization(UserRole);
                return roleaccess;
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, OAuthPJConstants.ForbiddenAccessErrorResponse);
            response.Headers.Add("WWW-Authenticate", "Bearer error=\"insufficient_privileges\"");

            actionContext.Response = response;
        }

        private bool RoleAuthorization(string UserRole)
        {
            string UserRoleAccess = UserRoles.ToSpaceSeparatedString().ToLower();//Roles which can access particular Web API methods
            string Role = UserRole.ToLower();// user roles
            string[] UserRoleAccessSplitted = UserRoleAccess.Split(' ');
            for (int i = 0; i < UserRoleAccessSplitted.Length; i++)
            {
                if (Role.Contains(UserRoleAccessSplitted[i]))// comparing user role with roles accessible to corresponding Web API methods
                {
                        return true;
                  
                }
            }

            return false;
        }
    }
}