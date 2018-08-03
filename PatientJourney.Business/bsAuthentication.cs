using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientJourney.BusinessModel.BuilderModels;
using PatientJourney.BusinessModel.Builders;
using PatientJourney.DataAccess.DataAccess;
using PatientJourney.DataAccess.Data;
using System.Data.Entity;
using PatientJourney.GlobalConstants;

namespace PatientJourney.Business
{
    public class bsAuthentication
    {
        private dbAuthentication _dbAuthentication = new dbAuthentication();

        public UserDetailsBuilder GetUserDetailswithRoles(String UserName)
        {
            UserDetailsBuilder _UserDetails = new UserDetailsBuilder();
            var userdetails = _dbAuthentication.GetUserforADlogonID(UserName);
            if (userdetails == null)
            {
                List<LDAPUserModel> _ldapUserModel = new List<LDAPUserModel>();
                _ldapUserModel = bsUserAdministration.GetUserInfo(UserName.ToUpper(), null);

                UserModel _user = new UserModel();
                _user.User511 = UserName;
                _user.FirstName = _ldapUserModel[0].FirstName;
                _user.LastName = _ldapUserModel[0].LastName;
                _user.Email = _ldapUserModel[0].Email;
                _user.UPI = _ldapUserModel[0].UPI;
                _user.IsActive = true;
                _user.CreatedDate = DateTime.Now.ToString();
                _user.CreatedBy = "ALAGAKX";
                _user.RoleIds = "3";
                _user.CountryIds = "9";
                var insertUser = bsUserAdministration.InsertNewUser(_user);
                if (insertUser == 1)
                {
                    _UserDetails = GetUserDetailswithRoles(UserName);
                }
                return _UserDetails;
            }
            else
            {
                UserModel _user = new UserModel();
                _user.UserID = userdetails.User_Id.ToString();
                _user.FirstName = userdetails.First_Name;
                _user.LastName = userdetails.Last_Name;
                _user.MiddleInitial = userdetails.Middle_Initial;
                _user.Email = userdetails.Email_Id;
                _user.UPI = userdetails.UPI == null ? String.Empty : userdetails.UPI.ToString();
                _user.User511 = userdetails.User_511;
                _user.IsActive = userdetails.Is_Active;
                _user.CreatedBy = userdetails.Created_By;
                _user.CreatedDate = userdetails.Created_Date == null ? String.Empty : userdetails.Created_Date.ToString();
                _user.ModifiedBy = userdetails.Modified_By;
                _user.ModifiedDate = userdetails.Modified_Date == null ? String.Empty : userdetails.Modified_Date.ToString();
                _UserDetails.Userdetails = _user;

                var userroles = _dbAuthentication.GetRolesforUsers(userdetails.User_Id);
                List<RolesModel> _lstUserroles = new List<RolesModel>();
                for (int i = 0; i < userroles.Count; i++)
                {
                    RolesModel _role = new RolesModel();
                    var roledetails = _dbAuthentication.GetRoleDetails(userroles[i].Role_Master_Id);
                    if (roledetails != null)
                    {
                        _role.RoleID = roledetails.Role_Master_Id.ToString();
                        _role.RoleName = roledetails.Role_Name;
                        _role.IsActive = roledetails.Is_Active;
                        _lstUserroles.Add(_role);
                    }
                }
                _UserDetails.UserRoles = _lstUserroles;
                return _UserDetails;
            }
        }


        public UserDetailsBuilder GetUserNameForDisplay(String UserName)
        {
            UserDetailsBuilder _UserDetails = new UserDetailsBuilder();
            var userdetails = _dbAuthentication.GetUserforADlogonID(UserName);
            if (userdetails == null)
                return null;
            else
            {
                UserModel _user = new UserModel();
                _user.FirstName = userdetails.First_Name;
                _user.LastName = userdetails.Last_Name;
                _UserDetails.Userdetails = _user;
                return _UserDetails;
            }
        }

        public LogOnAuditBuilder LogOnUserAudit(String UserName, String ClientTime, String ClientTimeZone, String UtcTime)
        {
            LogOnAuditBuilder _LogonAuditBuilder = new LogOnAuditBuilder();
            var userdetails = _dbAuthentication.GetUserforADlogonID(UserName);

            using (PJEntities db = new PJEntities())
            {
                User_Logon_Audit userLogonAudit = new User_Logon_Audit();
                userLogonAudit.User_511 = userdetails.User_511;
                userLogonAudit.UPI = userdetails.UPI == null ? String.Empty : userdetails.UPI.ToString();
                userLogonAudit.First_Name = userdetails.First_Name;
                userLogonAudit.Middle_Initial = userdetails.Middle_Initial;
                userLogonAudit.Last_Name = userdetails.Last_Name;
                userLogonAudit.Email_Id = userdetails.Email_Id;
                userLogonAudit.Logon_Client_Date = DateTimeOffset.Parse(ClientTime.Substring(0, Math.Min(ClientTime.Length, 25)));
                userLogonAudit.Logon_UTC_Date = DateTimeOffset.Parse(UtcTime.Substring(0, Math.Min(UtcTime.Length, 25)));
                userLogonAudit.Logon_Client_TimeZone = ClientTimeZone;
                userLogonAudit.Created_By = "RANGARX6";
                userLogonAudit.Created_Date = DateTimeOffset.Now;
                userLogonAudit.Modified_By = "RANGARX6";
                userLogonAudit.Modified_Date = DateTimeOffset.Now;
                db.User_Logon_Audit.Add(userLogonAudit);
                db.SaveChanges();
            }

            return _LogonAuditBuilder;
        }

        public LogOnAuditBuilder LogOnUserAuditForMs(String UserName, String ClientTime, String ClientTimeZone, String UtcTime)
        {
            LogOnAuditBuilder _LogonAuditBuilder = new LogOnAuditBuilder();
            var userdetails = _dbAuthentication.GetUserforADlogonID(UserName);

            using (PJEntities db = new PJEntities())
            {
                User_Logon_Audit_MS userLogonAudit = new User_Logon_Audit_MS();
                userLogonAudit.User_511 = userdetails.User_511;
                userLogonAudit.UPI = userdetails.UPI == null ? String.Empty : userdetails.UPI.ToString();
                userLogonAudit.First_Name = userdetails.First_Name;
                userLogonAudit.Middle_Initial = userdetails.Middle_Initial;
                userLogonAudit.Last_Name = userdetails.Last_Name;
                userLogonAudit.Email_Id = userdetails.Email_Id;
                userLogonAudit.Logon_Client_Date = DateTimeOffset.Parse(ClientTime.Substring(0, Math.Min(ClientTime.Length, 25)));
                userLogonAudit.Logon_UTC_Date = DateTimeOffset.Parse(UtcTime.Substring(0, Math.Min(UtcTime.Length, 25)));
                userLogonAudit.Logon_Client_TimeZone = ClientTimeZone;
                userLogonAudit.Created_By = "RANGARX6";
                userLogonAudit.Created_Date = DateTimeOffset.Now;
                userLogonAudit.Modified_By = "RANGARX6";
                userLogonAudit.Modified_Date = DateTimeOffset.Now;
                db.User_Logon_Audit_MS.Add(userLogonAudit);
                db.SaveChanges();
            }
            return _LogonAuditBuilder;
        }

        public bool GetSearchCriteriaForUser(String UserName)
        {
            bool doesExistAlready;
            using (PJEntities entity = new PJEntities())
            {
                var userData = from r in entity.Users select r;
                var searchData = from r in entity.Favourite_Search select r;
                int userId = userData.Where(x => x.User_511 == UserName.ToUpper()).Select(x => x.User_Id).FirstOrDefault();
                doesExistAlready = searchData.Any(o => o.User_Id == userId);
            }
            return doesExistAlready;
        }
    }
}
