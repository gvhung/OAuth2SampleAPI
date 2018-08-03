using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientJourney.DataAccess.Data;
using PatientJourney.DataAccess.DataAccess;
using PatientJourney.BusinessModel.BuilderModels;
using PatientJourney.BusinessModel.Builders;
using PatientJourney.GlobalConstants;
using PatientJourney.BusinessModel;
using System.DirectoryServices;
using System.Configuration;

namespace PatientJourney.Business
{
    public class bsUserAdministration
    {
        //Method to Insert User with Roles and Country Associations.
        public static Int32? InsertNewUser(UserModel User)
        {
            var ifuserexists = dbUserAdministration.CheckifUserExists(User.User511);

            List<LDAPUserModel> _ldapUserModel = new List<LDAPUserModel>();
            if (User.FirstName == null || User.LastName == null || User.Email == null || User.UPI == null)
            {
                _ldapUserModel = bsUserAdministration.GetUserInfo(User.User511.ToUpper(), User.UPI);
            }
            if (!ifuserexists)
            {
                User _userEntity = new User();
                _userEntity.User_511 = User.User511;
                _userEntity.First_Name = User.FirstName != null ? User.FirstName : _ldapUserModel[0].FirstName;
                _userEntity.Last_Name = User.LastName != null ? User.LastName : _ldapUserModel[0].LastName;
                _userEntity.Middle_Initial = User.MiddleInitial;
                _userEntity.Email_Id = User.Email != null ? User.Email : _ldapUserModel[0].Email;
                _userEntity.UPI = Convert.ToDecimal(User.UPI) != null ? Convert.ToDecimal(User.UPI) : Convert.ToDecimal(_ldapUserModel[0].UPI);
                _userEntity.Is_Active = User.IsActive;
                _userEntity.Created_Date = DateTime.Now;
                _userEntity.Created_By = User.User511 == null ? AppConstants.CreatedBy : User.User511;
                var insertedUserID = dbUserAdministration.InsertNewUser(_userEntity);

                if (User.RoleIds != null)
                {
                    List<String> lstRolesId = User.RoleIds.Trim().Split(',').ToList();
                    for (int i = 0; i < lstRolesId.Count; i++)
                    {
                        User_Roles _userRoleEntity = new User_Roles();
                        _userRoleEntity.User_Id = Convert.ToInt32(insertedUserID);
                        _userRoleEntity.Role_Master_Id = Convert.ToInt32(lstRolesId[i]);
                        _userRoleEntity.Is_Active = true;
                        _userRoleEntity.Created_Date = DateTime.Now;
                        _userRoleEntity.Created_By = User.User511 == null ? AppConstants.CreatedBy : User.User511;
                        var status = dbUserAdministration.InsertUserRolesforUserID(_userRoleEntity);
                    }
                }

                if (User.CountryIds != null)
                {
                    List<String> lstCountryId = User.CountryIds.Trim().Split(',').ToList();
                    for (int i = 0; i < lstCountryId.Count; i++)
                    {
                        User_Country_Association _userCountryEntity = new User_Country_Association();
                        _userCountryEntity.User_Id = Convert.ToInt32(insertedUserID);
                        _userCountryEntity.Country_Master_Id = Convert.ToInt32(lstCountryId[i]);
                        _userCountryEntity.Created_Date = DateTime.Now;
                        _userCountryEntity.Created_By = User.User511 == null ? AppConstants.CreatedBy : User.User511;
                        var status = dbUserAdministration.InsertUserCountryAssociation(_userCountryEntity);
                    }
                }
                return 1;
            }
            else
            {
                return -1;
            }

        }

        //Method to Update User with Roles and Country Associations.
        public static Int32? UpdateExistingUser(UserModel User)
        {
            var userdetailsEntity = dbUserAdministration.GetUserforUserId(Convert.ToInt32(User.UserID));
            if (userdetailsEntity != null)
            {
                userdetailsEntity.First_Name = User.FirstName;
                userdetailsEntity.Last_Name = User.LastName;
                userdetailsEntity.Middle_Initial = User.MiddleInitial;
                userdetailsEntity.Email_Id = User.Email;
                userdetailsEntity.UPI = String.IsNullOrEmpty(User.UPI) ? 0 : Convert.ToDecimal(User.UPI);
                userdetailsEntity.Is_Active = User.IsActive;
                userdetailsEntity.Modified_Date = DateTime.Now;
                userdetailsEntity.Modified_By = SessionHelper.LoggedinUser == null ? AppConstants.CreatedBy : SessionHelper.LoggedinUser.User511;
                var updatedUserID = dbUserAdministration.UpdateUserDetails(userdetailsEntity);

                var existingRolesforUser = dbUserAdministration.GetUserRolesAssociation(Convert.ToInt32(User.UserID));
                List<String> lstRolesId = User.RoleIds.Trim().Split(',').ToList();
                for (int i = 0; i < existingRolesforUser.Count; i++)
                {
                    if (!lstRolesId.Contains(existingRolesforUser[i].Role_Master_Id.ToString()))
                    {
                        var isRoleRemoved = dbUserAdministration.RemoveUserRolesAssocation(Convert.ToInt32(User.UserID), existingRolesforUser[i].Role_Master_Id);
                    }
                }
                for (int i = 0; i < lstRolesId.Count; i++)
                {
                    var isroleExists = existingRolesforUser.Where(x => x.Role_Master_Id == Convert.ToInt32(lstRolesId[i])).Any();
                    if (!isroleExists)
                    {
                        User_Roles _userRoleEntity = new User_Roles();
                        _userRoleEntity.User_Id = Convert.ToInt32(updatedUserID);
                        _userRoleEntity.Role_Master_Id = Convert.ToInt32(lstRolesId[i]);
                        _userRoleEntity.Is_Active = true;
                        _userRoleEntity.Created_Date = DateTime.Now;
                        _userRoleEntity.Created_By = SessionHelper.LoggedinUser == null ? AppConstants.CreatedBy : SessionHelper.LoggedinUser.User511;
                        var status = dbUserAdministration.InsertUserRolesforUserID(_userRoleEntity);
                    }
                }

                var existingCountriesforUser = dbUserAdministration.GetUserCountryAssociation(Convert.ToInt32(User.UserID));
                if (User.CountryIds != null)
                {
                    List<String> lstCountryId = User.CountryIds.Trim().Split(',').ToList();
                    for (int i = 0; i < existingCountriesforUser.Count; i++)
                    {
                        if (!lstCountryId.Contains(existingCountriesforUser[i].Country_Master_Id.ToString()))
                        {
                            var isCountryAssociationRemoved = dbUserAdministration.RemoveUserCountryAssocation(Convert.ToInt32(User.UserID), existingCountriesforUser[i].Country_Master_Id);
                        }
                    }
                    for (int i = 0; i < lstCountryId.Count; i++)
                    {
                        var isCountryExists = existingCountriesforUser.Where(x => x.Country_Master_Id == Convert.ToInt32(lstCountryId[i])).Any();
                        if (!isCountryExists)
                        {
                            User_Country_Association _userCountryEntity = new User_Country_Association();
                            _userCountryEntity.User_Id = Convert.ToInt32(updatedUserID);
                            _userCountryEntity.Country_Master_Id = Convert.ToInt32(lstCountryId[i]);
                            _userCountryEntity.Created_Date = DateTime.Now;
                            _userCountryEntity.Created_By = SessionHelper.LoggedinUser == null ? AppConstants.CreatedBy : SessionHelper.LoggedinUser.User511;
                            var status = dbUserAdministration.InsertUserCountryAssociation(_userCountryEntity);
                        }
                    }
                }
                return updatedUserID;
            }
            else
            {
                return -1;
            }
        }

        //Method to Update User with Roles and Country Associations.
        public static Int32? DeleteUser(string UserId, string RoleId)
        {
            int User_Id = Convert.ToInt32(UserId);

            bool deleteUserRole = dbUserAdministration.DeleteUserRolesAssocation(User_Id);
            bool deleteUserCountryAssociation = dbUserAdministration.DeleteUserCountryAssocation(User_Id);
            bool deleteFavArchetype = dbUserAdministration.DeleteFavArchetype(User_Id);
            bool deleteArea = dbUserAdministration.DeleteFavArea(User_Id);
            bool deleteBrand = dbUserAdministration.DeleteFavBrand(User_Id);
            bool deleteCountry = dbUserAdministration.DeleteFavCountry(User_Id);
            bool deleteSearch = dbUserAdministration.DeleteFavSearch(User_Id);

            if (deleteUserRole && deleteUserCountryAssociation)
            {
                var deleteUser = dbUserAdministration.RemoveUser(User_Id);
                return 1;
            }
            else
            {
                return -1;
            }
        }


        // Method to Get all Users details with Roles and Country Association.
        public static List<UserDetailsBuilder> GetAllUsers()
        {
            var _allUsers = dbUserAdministration.GetAllUsers();
            var _allRoles = dbMasterData.GetallRoles();
            var _allCountries = dbMasterData.GetallCountry();
            var _allRolesForUser = dbMasterData.GetallRolesForUser();
            var _allCountryForUser = dbMasterData.GetallCountryForUsers();

            List<UserDetailsBuilder> _finalList = new List<UserDetailsBuilder>();
            UserDetailsBuilder _user;
            for (int i = 0; i < _allUsers.Count; i++)
            {
                _user = new UserDetailsBuilder();

                int userId = _allUsers[i].User_Id;
                _user.Userdetails.UserID = _allUsers[i].User_Id.ToString();
                _user.Userdetails.FirstName = _allUsers[i].First_Name;
                _user.Userdetails.LastName = _allUsers[i].Last_Name;
                _user.Userdetails.UPI = _allUsers[i].UPI.ToString();
                _user.Userdetails.FullName = _allUsers[i].First_Name + _allUsers[i].Last_Name;
                _user.Userdetails.Email = _allUsers[i].Email_Id;
                _user.Userdetails.User511 = _allUsers[i].User_511;

                var rolelist = (from h in _allRolesForUser
                                join r in _allRoles on h.Role_Master_Id equals r.Role_Master_Id
                                where h.User_Id == userId
                                select new RolesModel()
                                   {
                                       RoleID = h.User_Roles_Id.ToString(),
                                       RoleName = r.Role_Name
                                   }).OrderBy(x => x.RoleName).ToList();

                _user.UserRoles = rolelist;

                var userCountryList = (from h in _allCountryForUser
                                       join r in _allUsers on h.User_Id equals r.User_Id
                                       join v in _allCountries on h.Country_Master_Id equals v.Country_Master_Id
                                       where h.User_Id == userId
                                       select new CountryMaster_List()
                                       {
                                           CountryId = (int)h.Country_Master_Id,
                                           CountryName = v.Country_Name
                                       }).OrderBy(x => x.CountryName).ToList();

                _user.UserCountry = userCountryList;

                _finalList.Add(_user);
            }
            return _finalList;
        }

        // Method to bind the user details in Grid
        public static List<UserGridModel> GetAllUsersForGrid(bool isActiveUserincluded, bool isInActiveUserincluded)
        {
            var _finalList = dbUserAdministration.GetUserGrid(isActiveUserincluded, isInActiveUserincluded);
            return _finalList;
        }

        // Method to bind the Search user details in Grid
        public static List<UserGridModel> GetSearchResultsForGrid(String SearchText, bool isActiveUserincluded, bool isInActiveUserincluded)
        {
            var _allUsers = dbUserAdministration.GetAllUsers();
            var _allRoles = dbMasterData.GetallRoles();
            var _allRolesForUser = dbMasterData.GetallRolesForUser();

            var searchResults = dbUserAdministration.GetUsersforSearchCriteria(SearchText, isActiveUserincluded, isInActiveUserincluded);

            List<UserGridModel> _finalList = new List<UserGridModel>();
            UserGridModel _user;

            if (searchResults.Count > 0)
            {
                for (int i = 0; i < searchResults.Count; i++)
                {
                    _user = new UserGridModel();
                    int userId = Convert.ToInt32(searchResults[i].UserID);
                    _user.UserID = searchResults[i].UserID;
                    _user.FirstName = searchResults[i].FirstName;
                    _user.LastName = searchResults[i].LastName;
                    _user.MiddleInitial = searchResults[i].MiddleInitial;
                    _user.UPI = searchResults[i].UPI;
                    _user.FullName = searchResults[i].FirstName + " " + searchResults[i].LastName;
                    _user.Email = searchResults[i].Email;
                    _user.User511 = searchResults[i].User511;
                    _user.IsActive = searchResults[i].IsActive;

                    var roles = _allRolesForUser.Where(x => x.User_Id == _user.UserID).ToList();
                    _user.RoleID = string.Join(",", roles.Select(n => n.Role_Master_Id.ToString()).ToArray());

                    var rolesMasterList = from rolesMaster in _allRoles
                                          join userRoles in _allRolesForUser on rolesMaster.Role_Master_Id equals userRoles.Role_Master_Id
                                          where userRoles.User_Id == _user.UserID
                                          select new { rolesMaster.Role_Name, rolesMaster.Is_Active };

                    _user.RoleName = string.Join(",", rolesMasterList.Select(n => n.Role_Name.ToString()).ToArray());

                    _finalList.Add(_user);
                }

            }

            return _finalList;
        }

        public static List<MasterDataModel> GetMastersForAddUser()
        {
            var _finalList = dbMasterData.GetMastersForAddUser();
            return _finalList;
        }


        public static List<Int32> GetRolesForUser(string UserId)
        {
            int userId = Convert.ToInt32(UserId);
            var _finalList = dbMasterData.GetRolesForUser(userId);

            List<Int32> _list =  new List<Int32>();

            for (int i = 0; i < _finalList.Count; i++)
            {
                int roleId = _finalList[i].Role_Master_Id;
                _list.Add(roleId);
            }
            
            return _list;
        }


        public static List<Int32> GetCountryForUser(string UserId)
        {
            int userId = Convert.ToInt32(UserId);
            var _finalList = dbMasterData.GetCountryForUser(userId);

            List<Int32> _list = new List<Int32>();

            for (int i = 0; i < _finalList.Count; i++)
            {
                int countryId = (int)_finalList[i].Country_Master_Id;
                _list.Add(countryId);
            }
            return _list;
        }



        public static List<LDAPUserModel> GetUserInfo(string UserName, string UPI)
        {
            string domain = AppConstants.GivenDomain;
            int upi = 0;
            if (UPI == null || UPI == "")
            {
                upi = bsUserAdministration.FetchUPIForLogonAccount(UserName, domain);
                UPI = upi.ToString();
            }
            List<LDAPUserModel> ldapDetails = bsUserAdministration.GetUserDetailByUPI(UPI);
            return ldapDetails;
        }

        static private int FetchUPIForLogonAccount(string five_1_1, string domain)
        {
            int upi = 0;
            string stringUpi = string.Empty;

            DirectoryEntry objDE;

            if (!String.IsNullOrEmpty(domain))
            {
                objDE = new DirectoryEntry(GetLdapDC(domain), LdapUser, LdapPassword, AuthenticationTypes.Secure);
                DirectorySearcher DirSearch = new DirectorySearcher(objDE);
                DirSearch.ReferralChasing = ReferralChasingOption.All;
                DirSearch.SearchScope = SearchScope.Subtree;
                DirSearch.PropertiesToLoad.Add(AppConstants.LdapUpiPropertyName);

                DirSearch.Filter = string.Format(LdapUpiSearchFilter, five_1_1);
                SearchResult result = null;
                SearchResultCollection results = DirSearch.FindAll();

                if (results != null)
                {
                    try
                    {
                        result = results[0];
                        stringUpi = result.Properties[AppConstants.LdapUpiPropertyName][0].ToString();
                    }
                    catch
                    {
                        stringUpi = string.Empty;
                    }
                }
            }

            if (!int.TryParse(stringUpi, out upi))
            {
                upi = 0;
            }

            return upi;
        }

        public static List<LDAPUserModel> GetUserDetailByUPI(string sUPI)
        {
            List<LDAPUserModel> _userdetails = new List<LDAPUserModel>();
            LDAPUserModel UserDetail = new LDAPUserModel();
            DirectoryEntry objGroupEntry = null;
            string strSearchFilter = string.Empty;
            try
            {
                DirectoryEntry de = new DirectoryEntry(ConfigurationManager.AppSettings[AppConstants.GivenPath], LdapUser, LdapPassword);
                de.AuthenticationType = AuthenticationTypes.Secure;
                DirectorySearcher search = new DirectorySearcher(de);

                if (!string.IsNullOrEmpty(sUPI))
                {
                    //Appending upi to strSearchFilter
                    strSearchFilter = strSearchFilter + "(employeenumber=" + sUPI + ")";
                }

                if (!string.IsNullOrWhiteSpace(strSearchFilter))
                {

                    search.Filter = strSearchFilter;

                    search.PropertiesToLoad.Add(AppConstants.FirstName.ToString());
                    search.PropertiesToLoad.Add(AppConstants.LdapUpiPropertyName.ToString());
                    search.PropertiesToLoad.Add(AppConstants.Title.ToString());
                    search.PropertiesToLoad.Add(AppConstants.Email.ToString());
                    search.PropertiesToLoad.Add(AppConstants.LastName.ToString());

                    SearchResult result = search.FindOne();

                    if (result != null)
                    {
                        objGroupEntry = result.GetDirectoryEntry();
                        if (objGroupEntry.Properties["employeenumber"].Value != null)
                        {
                            UserDetail.FirstName = Convert.ToString(objGroupEntry.Properties["givenname"].Value).Trim();
                            UserDetail.LastName = Convert.ToString(objGroupEntry.Properties["sn"].Value).Trim();
                            UserDetail.Email = Convert.ToString(objGroupEntry.Properties["mail"].Value);
                            UserDetail.UPI = Convert.ToString(objGroupEntry.Properties["employeenumber"].Value);
                        }
                    }

                }
                _userdetails.Add(UserDetail);
                return _userdetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        static private string GetLdapDC(string domain)
        {
            string trimmedDomain = string.IsNullOrWhiteSpace(domain) ? String.Empty : domain.Trim().ToUpper();

            string dcName = ConfigurationManager.AppSettings[trimmedDomain];
            if (string.IsNullOrWhiteSpace(dcName))
            {
                dcName = ConfigurationManager.AppSettings[AppConstants.DefaultLdapDomain];
            }

            return string.Concat(LdapPath, dcName);
        }

        /// <summary>
        /// Get the ldap user name used to connect 
        /// to ldap directory
        /// </summary>
        static private string LdapUser
        {
            get
            {
                string username = ConfigurationManager.AppSettings[AppConstants.LdapUsername];
                return string.IsNullOrWhiteSpace(username) ? null : username;
            }
        }

        /// <summary>
        /// Get the ldap upi search filter
        /// </summary>
        static private string LdapUpiSearchFilter
        {
            get
            {
                return ConfigurationManager.AppSettings[AppConstants.LdapUpiSearchFilter];
            }
        }

        /// <summary>
        /// Get the ldap password used to connect
        /// to the ldap directory
        /// </summary>
        static private string LdapPassword
        {
            get
            {
                string pwd = ConfigurationManager.AppSettings[AppConstants.LdapPassword];
                return string.IsNullOrWhiteSpace(pwd) ? null : pwd;
            }
        }

        /// <summary>
        /// Get the path to the Ldap directory
        /// </summary>
        static private string LdapPath
        {
            get
            {
                return ConfigurationManager.AppSettings[AppConstants.LdapPath];
            }
        }


    }
}
