using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace PatientJourney.GlobalConstants
{
    public class AppConstants
    {
        public static String CreatedBy = "PatientSpectrum";

        /// <summary>
        /// Represent the key for the UPI
        /// </summary>
        public const string UPI = "upi";

        /// <summary>
        /// Represents the key for the first name
        /// </summary>
        public const string FirstName = "givenname";

        /// <summary>
        /// Represents the key for the last name
        /// </summary>
        public const string LastName = "sn";

        /// <summary>
        /// Represents the key for the email
        /// </summary>
        public const string Email = "mail";

        /// <summary>
        /// Represents the key for the job title
        /// </summary>
        public const string Title = "title";

        /// <summary>
        /// Represents the key for the office phone
        /// </summary>
        public const string OfficePhone = "telephonenumber";

        /// <summary>
        /// Represents the key for the division code
        /// </summary>
        public const string DivisionCode = "divisioncode";

        /// <summary>
        /// Represents the key for the job code
        /// </summary>
        public const string JobCode = "jobcode";

        public const string AD_GROUP_NAME = "AD_GROUP_NAME";

        /// <summary>
        /// Represent the key for the manager upi
        /// </summary>
        public const string ManagerUPI = "managerupi";

        /// <summary>
        /// Represents the key for the approving manager upi
        /// </summary>
        public const string ApprovingManagerUPI = "idmapprovingmanagerupi";

        /// <summary>
        /// Represents the key for the sales franchise
        /// </summary>
        public const string SalesFranchise = "salesfranchise";

        /// <summary>
        /// Represents the key for the sales force pmi
        /// </summary>
        public const string SalesPMI = "salespmi";

        /// <summary>
        /// Represents the key for the default ldap domain
        /// </summary>
        public const string DefaultLdapDomain = "DEFAULT_DOMAIN";

        public const string DefaultDomain = "DOMAIN";

        /// <summary>
        /// Represents the key for the ldap user
        /// </summary>
        public const string LdapUsername = "LDAP_USER";

        /// <summary>
        /// Represents the key for the ldap password
        /// </summary>
        public const string LdapPassword = "LDAP_PWD";

        /// <summary>
        /// Represents the key for the ldap path
        /// </summary>
        public const string LdapPath = "LDAP_PATH";

        /// <summary>
        /// Represents the key for the ldap upi search filter from 5-1-1
        /// </summary>
        public const string LdapUpiSearchFilter = "LDAP_UPI_SEARCH_FILTER";

        /// <summary>
        /// Represent the propery name for upi
        /// </summary>
        public const string LdapUpiPropertyName = "employeeNumber";

        /// <summary>
        /// Represent the property name for Sales Area
        /// </summary>
        public const string SalesArea = "salesarea";

        /// <summary>
        /// Represent the property name for Sales Froce
        /// </summary>
        public const string SalesForce = "salessalesforce";

        /// <summary>
        /// Represent the property name for ADLogon
        /// </summary>
        public const string ADLogon = "adlogon";

        /// <summary>
        /// Represent the property name for ADDomain
        /// </summary>
        public const string ADDomain = "addomain";

        public const string GivenDomain = "ABBVIENET";

        public const string GivenPath = "LDAPServerPath";

        public const string ColumnNames = "FullName,User_511,Logon_Client_Date,Logon_UTC_Date,Logon_Client_Time_Zone";

        public const string ContentType = "application/vnd.ms-excel"; 
    }

    public class VersionTitleConstants
    {
        public const string StageReorder = "Stages reordered";
        public const string StageAdded = "Stage added";
        public const string StageUpdated = "Stage updated";
        public const string StageRemoved = "Stage removed";
        public const string TransactionReorder = "Transactions reordered";
        public const string TransactionAdded = "Transaction added";
        public const string TransactionUpdated = "Transaction updated";
        public const string TransactionRemoved = "Transaction removed";
        public const string ClinIntAdded = "Clinical intervention added";
        public const string AssociatedAdded = "Associated cost added";
        public const string DesiredAdded = "Desired outcome added";
        public const string ClinIntUpdated = "Clinical intervention updated";
        public const string AssociatedUpdated = "Associated cost updated";
        public const string DesiredUpdated = "Desired outcome updated";
        public const string PatientUpdated = "Updated Patient";
        public const string HCPUpdated = "Updated HCP";
        public const string PayerUpdated = "Updated Payer";
        public const string FeasibilityUpdated = "Updated Feasibility";
        public const string ViabilityUpdated = "Updated Viability";
    }

    public class VersionCommentsConstants
    {
        public const string StageReorder = "Stages are reordered";
        public const string StageRemoved = "Stage is removed";
        public const string TransactionReorder = "Transactions are reordered";
        public const string TransactionAdded = "New transaction is added";
        public const string TransactionUpdated = "Transaction is updated";
        public const string TransactionRemoved = "Transaction is removed";
        public const string ClinIntAdded = "New clinical intervention added";
        public const string ClinIntUpdated = "Clinical intervention is updated";
        public const string AssociatedUpdated = "Associated cost is updated";
        public const string DesiredUpdated = "Desired outcome is updated";
        public const string PatientUpdated = "Patient is updated";
        public const string HCPUpdated = "HCP is updated";
        public const string PayerUpdated = "Payer is updated";
        public const string FeasibilityUpdated = "Feasibility is updated";
        public const string ViabilityUpdated = "Viability is updated";
    }

    public class EmailTemplate
    {
        public const string Email = "<!DOCTYPE html><html><head><meta http-equiv='X-UA-Compatible' content='IE=edge' charset='utf-8' /></head>" +
                                    "<body style='margin: 0px;'>" +
                                    "<table width='100%' style='background-color:#00afd8;padding:10px 18px;' cellspacing='0' cellpadding='0'><tr>" +
                                    "<td><img src='cid:(PSLogo)'></img></td>" +
                                    "<td style='color: white;font-size: 13px;margin-top: 7px;' align='right'>" +
                                    "<a href='#' style='padding-right: 7px;color: white;text-decoration: none;'>PATIENT JOURNEY</a>|" +
                                    "<a href='#' style='padding-left: 7px;color: white;text-decoration: none;'>HOW TO USE PATIENT JOURNEY</a></td></tr></table>" +
                                    "<img src='cid:(PSHeader)' width='1349' height='216' style='height: 216px;width:1349px;border:0;'></img>" +
                                    //src='cid:(PSHeader)'
                                    "<table style='background-color:#eaeae8;padding: 9px 21px;font-size: 19px;margin-top:-4px;' width='100%'><tr><td>(Title)</td></tr></table>" +
                                    "<table style='background-color:#ecf0f1;padding: 10px 20px;width: 97%;margin:20px 20px 12px 20px;' width='97%'><tr>" +
                                    "<td style='width: 12%;'>Patient Journey Name</td>" +
                                    "<td style='width: 2%;'>:</td>" +
                                    "<td style='width: 31%;font-weight:bold;'>(JourneyName)</td>" +
                                    "<td style='width: 9%;'>Created By</td>" +
                                    "<td style='width: 2%;'>:</td>" +
                                    "<td style='font-weight:bold;'>(CreatedBy)</td></tr>" +
                                    "<tr><td>Brand</td><td>:</td><td style='font-weight:bold;'>(BrandName)</td>" +
                                    "<td>Created Date</td><td>:</td><td style='font-weight:bold;'>(CreatedDate)</td></tr>" +
                                    "<tr><td>Country</td><td>:</td><td style='font-weight:bold;'>(CountryName)</td><td>Reviewed By</td>" +
                                    "<td>:</td><td style='font-weight:bold;'>(ReviewedBy)</td></tr></table>" +
                                    "<p style='font-weight:bold;margin: 0px 0px 0px 20px;'>Comments</p>" +
                                    "<div style='padding:10px 20px;border:1px solid #d9d9d9;margin:3px 20px;height: 100px;'>(Comment)</div>" +
                                    "<table style='width: 100%;background-color: #0c9b44;bottom: 0;padding: 20px 20px;' cellspacing='0' cellpadding='0'><tr>" +
                                    "<td align='left'><table cellspacing='0' cellpadding='0' style='color: #ffffff;font-size: 12px;'> <tr>" +
                                    "<td style='padding-right:15px'>Contact Us</td>" +
                                    "<td style='padding-right:15px'>Privacy Policy</td>" +
                                    "<td style='padding-right:15px'>Terms of Use</td>" +
                                    "<td style='padding-right:15px'>&copy;2017 Abbvie Inc.</td></tr></table></td>" +
                                    "<td style='margin: calc((60px - 25px)/2) 0;' align='right'><a href=''><img src='cid:(PSFooter)'></a></td></tr></table>" +
                                    "</body></html>";

        public const string FromAddress = "PatientSpectrum-noreply@abbvie.com";
        public const string SubmitTitle = "Patient Journey submitted for approval";
        public const string ApproveTitle = "Patient Journey Approved";
        public const string SentBackTitle = "Patient Journey sent back for revision";
        public const string RejectTitle = "Patient Journey Rejected";
        public const string ArchiveTitle = "Patient Journey Archived";
        public const string Subject = "Patient Spectrum";

    }

    public class OAuthPJConstants
    {
        public const string OAuthServerURI = "https://PJOAuthServer/identity";
        public const string OAuthServerPublicOrigin = "https://pc374999.cts.com"; //identity server url-https
        public const string OAuthServer = OAuthServerPublicOrigin + "/identity";
        public const string OAuthServerTokenEndPoint = OAuthServerPublicOrigin + "/connect/token";
        public const string OAuthServerAuthorizationEndPoint = OAuthServerPublicOrigin + "/connect/authorize";
        public const string OAuthServerAudience = "https://PJOAuthServer/identity/resources";

      //  public  string PatientResourceAPIURI = ConfigurationManager.AppSettings["PatientResourceAPIURI"]; 
        public const string PatientResourceAPIURI = "http://localhost/";//web API http-url
       // public const string PatientResourceAPIURI = "https://localhost:44373/";//web API http url to be changed back to this https after testing
        public const string AdminResourceAPIURI = "http://localhost:18181/"; 
        
        public const string PJAPIScope = "pj_api_scope";
        public const string PJAdminAPIScope = "pj_admin_api_scope";
        public const string OfflineAccessScope = "offline_access";
        public const string ReadonlyRole = "ReadOnly";
        public const string EditorRole = "Editor";
        public const string AdminRole = "Admin";
        public const string ViewerRole = "Viewer";
        public const string Role = "userrole";
        public const string ScopeDelimiter = " ";
        public const string ResourceScopes = PJAPIScope + ScopeDelimiter + PJAdminAPIScope + ScopeDelimiter + OfflineAccessScope;
        public const int AccessTokenLifetime = 3600; //in seconds

        public const string MVCClientId = "securemvcclient";
        public const string MVCClientSecret = "secret code";

        public const string JavascriptClientId = "angularapp";
        public const string JavascriptClientSecret = "javascript secret code";
        
        public const string CustomClientId = "customclient";
        public const string CustomClientSecret = "custom secret code";

        public const string ROFCClientId = "refreshclient";
        public const string ROFCClientSecret = "refresh secret code";

        /* Error Messages */
        public const string RolesMissingError = "roles missing";
        public const string ForbiddenAccessErrorResponse = "Forbidden Access. You have insuffcient privilege access!";
        public enum PJRoles
        {
            Editor = 1,
            Admin = 2,
            Viewer = 3
        }

        public const string UnhandledErrorMessage = "Unhandled Error.";
        public const string InvalidRequestMessage = "Patient Spectrum API is unable to process the request due to invalid request.";
        public const string InvalidRequestReasonPharse = "Invalid request exception occured at {0}";
        public const string NullReferenceExceptionMessage = "Patient Spectrum API is unable to process the request due to null values";
        public const string NullReferenceReasonPharse = "Null Refernce exception occured at {0}";
        public const string InvalidCastMessage = "Patient Spectrum API is unable to process the request due to invalid casting";
        public const string InvalidCastReasonPharse = "Invalid Casting exception occured at {0}";
        public const string UnknownExceptionMessage = "Patient Spectrum API is unable to process the request due to unknown exception.";
        public const string UnknownExceptionReasonPharse = "Unknown exception occured at {0}";
    }
}
