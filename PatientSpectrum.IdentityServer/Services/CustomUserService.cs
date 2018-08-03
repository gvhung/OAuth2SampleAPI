using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;
using PatientJourney.Business;
using PatientJourney.CrytoHelper;
using PatientJourney.GlobalConstants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.DirectoryServices;
using PatientJourney.BusinessModel.Builders;
using System.Linq;

namespace PatientSpectrum.IdentityServer.Services
{
    class CustomUserService : UserServiceBase
    {
        private bsAuthentication _bsAuthenticationPJ = new bsAuthentication();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            var excryptedUsername = context.UserName;
            var username = excryptedUsername;
            string decryptString = string.Empty;
            string encryptString = string.Empty;

            bool isValidUser_PJ;
            string RoleIds = string.Empty;
            string RoleNames = string.Empty;
            string CountryIds = string.Empty;
            string LogOnAuditPJ = string.Empty;
            try
            {
                decryptString = X509CyptoAlgorithm.Decrypt(Convert.FromBase64String(username));

                //get sample for PostMan input purpose
                var encryptedUserName = X509CyptoAlgorithm.Encrypt("NARAYSX2"); 

                username = decryptString;
            }
            catch (Exception)
            {
                context.AuthenticateResult = new AuthenticateResult("Invalid Username");
                return Task.FromResult(0);
            }

            var password = context.Password;
            try
            {
                encryptString = X509CyptoAlgorithm.Encrypt(password);
                password = encryptString;
            }
            catch (Exception)
            {
                context.AuthenticateResult = new AuthenticateResult("Invalid Username");
                return Task.FromResult(0);
            }

            var message = context.SignInMessage;

            if (message != null)
            {
                var useremail = message.Tenant;
                if (useremail == null)
                {
                    useremail = password;
                }

                UserDetailsBuilder PJUser;
                try
                {
                    var userNameChecked = CommonHelper.UserName(username);
                    string domain = ConfigurationManager.AppSettings["DOMAIN"];
                    bool isValidUser_MS = GetUserInfo(userNameChecked, domain);
                    PJUser = _bsAuthenticationPJ.GetUserDetailswithRoles(userNameChecked);
                    if (PJUser == null)
                        isValidUser_PJ = false;
                    else
                    {
                        var fname = PJUser.Userdetails.FirstName;
                        string roles = null;
                        string names = null;
                        for (int i = 0; i < PJUser.UserRoles.Count; i++)
                        {
                            if (i == PJUser.UserRoles.Count - 1)
                            {
                                roles = roles + PJUser.UserRoles[i].RoleID;
                                names = names + PJUser.UserRoles[i].RoleName;
                            }
                            else
                            {
                                roles = roles + PJUser.UserRoles[i].RoleID + ",";
                                names = names + PJUser.UserRoles[i].RoleName + " ";
                            }
                        }
                        var userdetails = PJUser.Userdetails;
                        RoleIds = roles;
                        RoleNames = names.TrimEnd();
                        if (RoleIds == null || RoleIds == "")
                        {
                            List<int> NumRoleids = bsUserAdministration.GetRolesForUser(PJUser.Userdetails.UserID.ToString());
                            RoleIds = string.Join(",", NumRoleids.Select(n => n.ToString()).ToArray());
                            RoleNames = RoleNames + ((NumRoleids.IndexOf((int)OAuthPJConstants.PJRoles.Admin) != -1)? OAuthPJConstants.AdminRole + " " : String.Empty);
                            RoleNames = RoleNames + ((NumRoleids.IndexOf((int)OAuthPJConstants.PJRoles.Editor) != -1) ? OAuthPJConstants.EditorRole + " " : String.Empty);
                            RoleNames = RoleNames + ((NumRoleids.IndexOf((int)OAuthPJConstants.PJRoles.Viewer) != -1) ? OAuthPJConstants.ViewerRole : String.Empty);
                            RoleNames = RoleNames.TrimEnd();
                        }

                        List<Int32> ids = bsUserAdministration.GetCountryForUser(PJUser.Userdetails.UserID.ToString());

                        if (ids.Count > 0)
                        {
                            CountryIds = string.Join(",", ids.Select(n => n.ToString()).ToArray());
                        }
                        else
                        {
                            CountryIds = "9";
                        }
                        //FirstLoad = true;
                        if ((PJUser.Userdetails.IsActive) && (PJUser.UserRoles.Count > 0))
                        {
                            isValidUser_PJ = true;
                            LogOnAuditPJ = "TRUE";
                        }
                        else
                        {
                            isValidUser_PJ = false;
                        }


                    }
                }
                catch (Exception ex)
                {
                    context.AuthenticateResult = new AuthenticateResult("Authentication Failed. Reason : " + ex.Message);
                    return Task.FromResult(0);
                }
                //use the 511 Id to go to database and fetch role and other user claims

                var claims = new List<Claim>
                    {
                        new Claim("user511id", username),
                        new Claim("userrole", RoleNames),
                        new Claim("userroleid", RoleIds),
                        new Claim("usercountry", CountryIds),
                        new Claim("isvaliduser", isValidUser_PJ.ToString()),
                        new Claim("greetingname", PJUser.Userdetails.FirstName + " " + PJUser.Userdetails.LastName),
                        new Claim("useremail", PJUser.Userdetails.Email),
                        new Claim("encrypted511Id", password),
                        new Claim("name", username),
                    };

                var result = new AuthenticateResult(username, username,
                    claims: claims,
                    authenticationMethod: "custom");
                
                context.AuthenticateResult = new AuthenticateResult(username, username, claims);
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="five_1_1"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public bool GetUserInfo(string five_1_1, string domain)
        {
            if (string.IsNullOrWhiteSpace(five_1_1)) throw new ArgumentNullException("five_1_1");
            bool isValidUser;

            int upi = 0;

            upi = FetchUPIForLogonAccount(five_1_1, domain);
            string secADGroup = ConfigurationManager.AppSettings["AD_GROUP_NAME"];
            isValidUser = IsAdminExistsOECGroup(upi, secADGroup, domain);
            return isValidUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="five_1_1"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        private int FetchUPIForLogonAccount(string five_1_1, string domain)
        {
            int upi = 0;
            string stringUpi = string.Empty;

            //// get the upi based on the availability of the domain name
            //string stringUpi = string.IsNullOrWhiteSpace(domain) ?
            //    wsClient.getUPIFromAccount(five_1_1) : wsClient.getUPIFromAccountAndDomain(five_1_1, domain);

            DirectoryEntry objDE;

            if (!String.IsNullOrEmpty(domain))
            {
                // Create a new DirectoryEntry with the given path.
                //, null, null, AuthenticationTypes.Secure
                objDE = new DirectoryEntry(GetLdapDC(domain), LdapUser, LdapPassword, AuthenticationTypes.Secure);
                DirectorySearcher DirSearch = new DirectorySearcher(objDE);
                DirSearch.ReferralChasing = ReferralChasingOption.All;
                DirSearch.SearchScope = SearchScope.Subtree;
                DirSearch.PropertiesToLoad.Add("employeeNumber");

                DirSearch.Filter = string.Format(LdapUpiSearchFilter, five_1_1);
                SearchResult result = null;
                SearchResultCollection results = DirSearch.FindAll();

                if (results != null)
                {
                    try
                    {
                        result = results[0];
                        stringUpi = result.Properties["employeeNumber"][0].ToString();
                    }
                    catch
                    {
                        stringUpi = string.Empty;
                    }
                }
            }

            if (!int.TryParse(stringUpi, out upi))
            {
                throw new Exception(
                    string.Format("Could not find the UPI from the UPI Directory for the given 5-1-1 {0} and domain {1}", five_1_1, domain));
            }

            return upi;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="upi"></param>
        /// <param name="secADGroup"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        private bool IsAdminExistsOECGroup(int upi, string secADGroup, string domain)
        {
            bool isAdmin = false;
            string LDAPAbbvieServerPath = ConfigurationManager.AppSettings["LDAPAbbvieServerPath"];
            try
            {
                DirectoryEntry de = new DirectoryEntry(LDAPAbbvieServerPath, LdapUser, LdapPassword, AuthenticationTypes.Secure);
                DirectorySearcher objDirectorySearch = new DirectorySearcher(de);
                objDirectorySearch.Filter = "(&(objectClass=group) (cn=" + secADGroup + "))";
                SearchResult objSearchResults = objDirectorySearch.FindOne();

                DirectoryEntry path = new DirectoryEntry(objSearchResults.Path, LdapUser, LdapPassword);
                DirectorySearcher searcher = new DirectorySearcher(path);

                searcher.SearchScope = SearchScope.Subtree;

                SearchResult r = searcher.FindOne();

                ResultPropertyCollection resultPropColl = r.Properties;

                string givenupi = upi.ToString().ToLower();

                foreach (Object memberColl in resultPropColl["member"])
                {
                    DirectoryEntry gpMemberEntry = new DirectoryEntry(LDAPAbbvieServerPath + "/" + memberColl, LdapUser, LdapPassword);

                    PropertyCollection userProps = gpMemberEntry.Properties;
                    string upid = Convert.ToString(gpMemberEntry.Properties["employeenumber"].Value).ToLower();

                    if (upid.Contains(givenupi))
                    {
                        object obVal = userProps["samAccountName"].Value;
                        if (null != obVal)
                        {
                            isAdmin = true;
                            break;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return isAdmin;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        private string GetLdapDC(string domain)
        {
            string trimmedDomain = string.IsNullOrWhiteSpace(domain) ? String.Empty : domain.Trim().ToUpper();

            string dcName = ConfigurationManager.AppSettings[trimmedDomain];
            if (string.IsNullOrWhiteSpace(dcName))
            {
                dcName = ConfigurationManager.AppSettings["DEFAULT_DOMAIN"];
            }

            return string.Concat(LdapPath, dcName);
        }

        /// <summary>
        /// Get the ldap user name used to connect 
        /// to ldap directory
        /// </summary>
        private string LdapUser
        {
            get
            {
                string username = ConfigurationManager.AppSettings["LDAP_USER"];
                return string.IsNullOrWhiteSpace(username) ? null : username;
            }
        }

        /// <summary>
        /// Get the ldap upi search filter
        /// </summary>
        private string LdapUpiSearchFilter
        {
            get
            {
                return ConfigurationManager.AppSettings["LDAP_UPI_SEARCH_FILTER"];
            }
        }

        /// <summary>
        /// Get the ldap password used to connect
        /// to the ldap directory
        /// </summary>
        private string LdapPassword
        {
            get
            {
                string pwd = ConfigurationManager.AppSettings["LDAP_PWD"];
                return string.IsNullOrWhiteSpace(pwd) ? null : pwd;
            }
        }

        /// <summary>
        /// Get the path to the Ldap directory
        /// </summary>
        private string LdapPath
        {
            get
            {
                return ConfigurationManager.AppSettings["LDAP_PATH"];
            }
        }

    }
}