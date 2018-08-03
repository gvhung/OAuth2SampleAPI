using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientJourney.DataAccess.Data;
using PatientJourney.BusinessModel.BuilderModels;

namespace PatientJourney.DataAccess.DataAccess
{
    public class dbMasterData
    {
        public static List<Role_Master> GetallRoles()
        {
            using (PJEntities _entity = new PJEntities())
            {
                var result = _entity.Role_Master.ToList();
                return result;
            }
        }

        public static List<Country_Master> GetallCountry()
        {
            using (PJEntities _entity = new PJEntities())
            {
                var result = _entity.Country_Master.ToList();
                return result;
            }
        }

        public static List<User_Roles> GetallRolesForUser()
        {
            using (PJEntities _entity = new PJEntities())
            {
                var result = _entity.User_Roles.ToList();
                return result;
            }
        }

        public static List<User_Country_Association> GetallCountryForUsers()
        {
            using (PJEntities _entity = new PJEntities())
            {
                var result = _entity.User_Country_Association.ToList();
                return result;
            }
        }

        public static List<MasterDataModel> GetMastersForAddUser()
        {
            List<MasterDataModel> _finalList = new List<MasterDataModel>();
            MasterDataModel _master;

            using (PJEntities _entity = new PJEntities())
            {
                var countryData = _entity.Country_Master.ToList();
                var roleData = _entity.Role_Master.ToList();

                _master = new MasterDataModel();

                _master.CountryMasterList = (from a in countryData
                                             select new CountryMaster_List()
                                             {
                                                 CountryId = a.Country_Master_Id,
                                                 CountryName = a.Country_Name
                                             }).OrderBy(x => x.CountryName).ToList();

                _master.RoleMasterList = (from a in roleData
                                             select new RoleMaster_List()
                                             {
                                                 RoleId = a.Role_Master_Id,
                                                 RoleName = a.Role_Name
                                             }).OrderBy(x => x.RoleName).ToList();
                _finalList.Add(_master);
            }
            return _finalList;
        }

        public static List<User_Roles> GetRolesForUser(int userId)
        {
            using (PJEntities _entity = new PJEntities())
            {
                var result = _entity.User_Roles.Where(x => x.User_Id == userId).ToList();
                return result;
            }
        }

        public static List<User_Country_Association> GetCountryForUser(int userId)
        {
            using (PJEntities _entity = new PJEntities())
            {
                var result = _entity.User_Country_Association.Where(x => x.User_Id == userId).ToList();
                return result;
            }
        }
    }
}
