using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientJourney.DataAccess.Data;
using PatientJourney.BusinessModel.BuilderModels;
using PatientJourney.BusinessModel.Builders;

namespace PatientJourney.DataAccess.DataAccess
{
    public class dbUserAdministration
    {
        public static Int32? InsertNewUser(User userdetails)
        {
            using (PJEntities _entity = new PJEntities())
            {
                //USER _user = new USER();
                //_user.FIRST_NAME = userdetail.FirstName;
                //_user.LAST_NAME = userdetail.LastName;
                //_user.MIDDLE_INITIAL = userdetail.MiddleInitial;
                //_user.IS_ACTIVE = userdetail.IsActive;
                //_user.USER_511 = userdetail.User511;
                //_user.UPI = Convert.ToDecimal(userdetail.UPI);
                //_user.EMAILID = userdetail.Email;
                //_user.CREATED_BY = userdetail.CreatedBy;
                //_user.CREATED_DATE = Convert.ToDateTime(userdetail.CreatedDate);
                //_user.MODIFIED_BY = userdetail.ModifiedBy;
                //_user.MODIFIED_DATE = Conveuserdetail.ModifiedDate;
                try
                {
                    _entity.Users.Add(userdetails);
                    _entity.SaveChanges();
                    return userdetails.User_Id;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public static bool InsertUserRolesforUserID(User_Roles _userRole)
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    _entity.User_Roles.Add(_userRole);
                    _entity.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static Int32? UpdateUserDetails(User _user)
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    var _selectedUser = _entity.Users.Where(x => x.User_Id == _user.User_Id).FirstOrDefault();
                    _selectedUser.First_Name = _user.First_Name;
                    _selectedUser.Last_Name = _user.Last_Name;
                    _selectedUser.Middle_Initial = _user.Middle_Initial;
                    _selectedUser.Email_Id = _user.Email_Id;
                    _selectedUser.User_511 = _user.User_511;
                    _selectedUser.UPI = _user.UPI;
                    _selectedUser.Is_Active = _user.Is_Active;
                    _selectedUser.Modified_Date = _user.Modified_Date;
                    _selectedUser.Modified_By = _user.Modified_By;
                    _entity.SaveChanges();
                    return _selectedUser.User_Id;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public static bool InsertUserCountryAssociation(User_Country_Association _userCountry)
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    _entity.User_Country_Association.Add(_userCountry);
                    _entity.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static List<User_Country_Association> GetUserCountryAssociation(Int32? User_Id)
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    var _userCountrylist = _entity.User_Country_Association.Where(x => x.User_Id == User_Id).ToList();
                    return _userCountrylist;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static List<User_Roles> GetUserRolesAssociation(Int32? User_Id)
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    var _userRoleslist = _entity.User_Roles.Where(x => x.User_Id == User_Id).ToList();
                    return _userRoleslist;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static bool RemoveUserRolesAssocation(Int32? User_Id, Int32? Role_Id)
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    var _userRole = _entity.User_Roles.Where(x => x.User_Id == User_Id && x.Role_Master_Id == Role_Id).FirstOrDefault();
                    _entity.User_Roles.Remove(_userRole);
                    _entity.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool RemoveUserCountryAssocation(Int32? User_Id, Int32? Country_Id)
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    var _userCountry = _entity.User_Country_Association.Where(x => x.User_Id == User_Id && x.Country_Master_Id == Country_Id).FirstOrDefault();
                    if (_userCountry != null)
                    {
                        _entity.User_Country_Association.Remove(_userCountry);
                        _entity.SaveChanges();
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool DeleteUserRolesAssocation(Int32? User_Id)
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    var _userRole = _entity.User_Roles.Where(x => x.User_Id == User_Id);

                    foreach (var userRole in _userRole)
                    {
                        _entity.User_Roles.Remove(userRole);
                    }
                    _entity.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool DeleteUserCountryAssocation(Int32? User_Id)
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    var _userCountry = _entity.User_Country_Association.Where(x => x.User_Id == User_Id);
                    foreach (var userCountry in _userCountry)
                    {
                        _entity.User_Country_Association.Remove(userCountry);
                    }
                    _entity.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool DeleteFavArchetype(Int32? User_Id)
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    var _userArchetype = _entity.Favourite_Search_Archetype.Where(x => x.User_Id == User_Id);
                    foreach (var userArchetype in _userArchetype)
                    {
                        _entity.Favourite_Search_Archetype.Remove(userArchetype);
                    }
                    _entity.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }


        public static bool DeleteFavArea(Int32? User_Id)
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    var _userArea = _entity.Favourite_Search_Area.Where(x => x.User_Id == User_Id);
                    foreach (var userArea in _userArea)
                    {
                        _entity.Favourite_Search_Area.Remove(userArea);
                    }
                    _entity.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool DeleteFavBrand(Int32? User_Id)
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    var _userBrand = _entity.Favourite_Search_Brand.Where(x => x.User_Id == User_Id);
                    foreach (var userBrand in _userBrand)
                    {
                        _entity.Favourite_Search_Brand.Remove(userBrand);
                    }
                    _entity.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool DeleteFavCountry(Int32? User_Id)
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    var _userCountry = _entity.Favourite_Search_Country.Where(x => x.User_Id == User_Id);
                    foreach (var userCountry in _userCountry)
                    {
                        _entity.Favourite_Search_Country.Remove(userCountry);
                    }
                    _entity.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool DeleteFavSearch(Int32? User_Id)
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    var _userSearch = _entity.Favourite_Search.Where(x => x.User_Id == User_Id);
                    foreach (var userSearch in _userSearch)
                    {
                        _entity.Favourite_Search.Remove(userSearch);
                    }
                    _entity.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }


        public static bool RemoveUser(Int32? User_Id)
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    var _users = _entity.Users.Where(x => x.User_Id == User_Id).FirstOrDefault();
                    _entity.Users.Remove(_users);
                    _entity.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static List<UserGridModel> GetUsersforSearchCriteria(String SearchText, bool isActiveUserincluded, bool isInActiveUserincluded)
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    List<UserGridModel> _userFilter = new List<UserGridModel>();
                    var userlist = _entity.Users.Select(x => new UserGridModel
                    {
                        UserID = x.User_Id,
                        FirstName = (x.First_Name == null) ? string.Empty : x.First_Name.ToLower(),
                        LastName = (x.Last_Name == null) ? string.Empty : x.Last_Name.ToLower(),
                        MiddleInitial = (x.Middle_Initial == null) ? string.Empty : x.Middle_Initial.ToLower(),
                        UPI = x.UPI,
                        User511 = (x.User_511 == null) ? string.Empty : x.User_511.ToLower(),
                        Email = (x.Email_Id == null) ? string.Empty : x.Email_Id.ToLower(),
                        IsActive = x.Is_Active,
                    }).ToList();

                    SearchText = SearchText.ToLower();

                    if (isActiveUserincluded && isInActiveUserincluded)
                    {
                        _userFilter = (from h in userlist
                                       where h.FirstName.Contains(SearchText)
                                       || h.LastName.Contains(SearchText)
                                       || h.MiddleInitial.Contains(SearchText)
                                       || h.UPI.ToString().Contains(SearchText)
                                       || h.User511.Contains(SearchText)
                                       || h.Email.Contains(SearchText)
                                       select h).ToList();
                        return _userFilter;
                    }
                    else if (isActiveUserincluded && !isInActiveUserincluded)
                    {
                        _userFilter = (from h in userlist
                                       where (h.FirstName.Contains(SearchText)
                                       || h.LastName.Contains(SearchText)
                                       || h.MiddleInitial.Contains(SearchText)
                                       || h.UPI.ToString().Contains(SearchText)
                                       || h.User511.Contains(SearchText)
                                       || h.Email.Contains(SearchText))
                                       && h.IsActive == true
                                       select h).ToList();
                        return _userFilter;
                    }
                    else if (!isActiveUserincluded && isInActiveUserincluded)
                    {
                        var searchresults = (from h in userlist
                                             where (h.FirstName.Contains(SearchText)
                                             || h.LastName.Contains(SearchText)
                                             || h.MiddleInitial.Contains(SearchText)
                                             || h.UPI.ToString().Contains(SearchText)
                                             || h.User511.Contains(SearchText)
                                             || h.Email.Contains(SearchText))
                                             && h.IsActive == false
                                             select h).ToList();
                        return searchresults;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        public static List<UserGridModel> GetUserGrid(bool isActiveUserincluded, bool isInActiveUserincluded)
        {
            var _allUsers = dbUserAdministration.GetAllUsers();
            var _allRoles = dbMasterData.GetallRoles();
            var _allRolesForUser = dbMasterData.GetallRolesForUser();
            var _allUsersActive = dbUserAdministration.GetAllActiveUsers();
            var _allUsersInActive = dbUserAdministration.GetAllInActiveUsers();

            try
            {
                List<UserGridModel> _finalList = new List<UserGridModel>();
                UserGridModel _user;

                if (isActiveUserincluded && isInActiveUserincluded)
                {
                    for (int i = 0; i < _allUsers.Count; i++)
                    {
                        _user = new UserGridModel();
                        int userId = _allUsers[i].User_Id;
                        _user.UserID = _allUsers[i].User_Id;
                        _user.FirstName = _allUsers[i].First_Name;
                        _user.LastName = _allUsers[i].Last_Name;
                        _user.MiddleInitial = _allUsers[i].Middle_Initial;
                        _user.UPI = _allUsers[i].UPI;
                        _user.FullName = _allUsers[i].First_Name + " " + _allUsers[i].Last_Name;
                        _user.Email = _allUsers[i].Email_Id;
                        _user.User511 = _allUsers[i].User_511;
                        _user.IsActive = _allUsers[i].Is_Active;

                        var roles = _allRolesForUser.Where(x => x.User_Id == _user.UserID).ToList();
                        _user.RoleID = string.Join(",", roles.Select(n => n.Role_Master_Id.ToString()).ToArray());

                        var rolesMasterList = from rolesMaster in _allRoles
                                              join userRoles in _allRolesForUser on rolesMaster.Role_Master_Id equals userRoles.Role_Master_Id
                                              where userRoles.User_Id == _user.UserID
                                              select new { rolesMaster.Role_Name, rolesMaster.Is_Active };

                        _user.RoleName = string.Join(",", rolesMasterList.Select(n => n.Role_Name.ToString()).ToArray());


                        _finalList.Add(_user);
                    }
                    return _finalList;
                }

                else if (isActiveUserincluded && !isInActiveUserincluded)
                {
                    for (int i = 0; i < _allUsersActive.Count; i++)
                    {
                        _user = new UserGridModel();
                        int userId = _allUsersActive[i].User_Id;
                        _user.UserID = _allUsersActive[i].User_Id;
                        _user.FirstName = _allUsersActive[i].First_Name;
                        _user.LastName = _allUsersActive[i].Last_Name;
                        _user.MiddleInitial = _allUsers[i].Middle_Initial;
                        _user.UPI = _allUsersActive[i].UPI;
                        _user.FullName = _allUsersActive[i].First_Name + " " + _allUsersActive[i].Last_Name;
                        _user.Email = _allUsersActive[i].Email_Id;
                        _user.User511 = _allUsersActive[i].User_511;
                        _user.IsActive = _allUsersActive[i].Is_Active;

                        var roles = _allRolesForUser.Where(x => x.User_Id == _user.UserID).ToList();
                        _user.RoleID = string.Join(",", roles.Select(n => n.Role_Master_Id.ToString()).ToArray());

                        var rolesMasterList = from rolesMaster in _allRoles
                                              join userRoles in _allRolesForUser on rolesMaster.Role_Master_Id equals userRoles.Role_Master_Id
                                              where userRoles.User_Id == _user.UserID
                                              select new { rolesMaster.Role_Name, rolesMaster.Is_Active };

                        _user.RoleName = string.Join(",", rolesMasterList.Select(n => n.Role_Name.ToString()).ToArray());

                        _finalList.Add(_user);
                    }
                    return _finalList;
                }

                else if (!isActiveUserincluded && isInActiveUserincluded)
                {
                    for (int i = 0; i < _allUsersInActive.Count; i++)
                    {
                        _user = new UserGridModel();
                        int userId = _allUsersInActive[i].User_Id;

                        _user.UserID = _allUsersInActive[i].User_Id;
                        _user.FirstName = _allUsersInActive[i].First_Name;
                        _user.LastName = _allUsersInActive[i].Last_Name;
                        _user.MiddleInitial = _allUsers[i].Middle_Initial;
                        _user.UPI = _allUsersInActive[i].UPI;
                        _user.FullName = _allUsersInActive[i].First_Name + " " + _allUsersInActive[i].Last_Name;
                        _user.Email = _allUsersInActive[i].Email_Id;
                        _user.User511 = _allUsersInActive[i].User_511;
                        _user.IsActive = _allUsersInActive[i].Is_Active;

                        var roles = _allRolesForUser.Where(x => x.User_Id == _user.UserID).ToList();
                        _user.RoleID = string.Join(",", roles.Select(n => n.Role_Master_Id.ToString()).ToArray());

                        var rolesMasterList = from rolesMaster in _allRoles
                                              join userRoles in _allRolesForUser on rolesMaster.Role_Master_Id equals userRoles.Role_Master_Id
                                              where userRoles.User_Id == _user.UserID
                                              select new { rolesMaster.Role_Name, rolesMaster.Is_Active };

                        _user.RoleName = string.Join(",", rolesMasterList.Select(n => n.Role_Name.ToString()).ToArray());


                        _finalList.Add(_user);
                    }
                    return _finalList;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public static List<User> GetAllUsers()
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    var allUserslist = _entity.Users.ToList();
                    return allUserslist;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static List<User> GetAllActiveUsers()
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    var allActiveUserslist = _entity.Users.Where(x => x.Is_Active == true).ToList();
                    return allActiveUserslist;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static List<User> GetAllInActiveUsers()
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    var allInActiveUserslist = _entity.Users.Where(x => x.Is_Active == false).ToList();
                    return allInActiveUserslist;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static bool CheckifUserExists(String ADLogon)
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    var result = _entity.Users.Where(x => x.User_511.ToLower() == ADLogon.ToLower()).Any();
                    return result;
                }
                catch
                {
                    return true;
                }
            }
        }

        public static User GetUserforUserId(Int32? UserId)
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    var result = _entity.Users.Where(x => x.User_Id == UserId).FirstOrDefault();
                    return result;
                }
                catch
                {
                    return null;
                }
            }

        }

    }
}
