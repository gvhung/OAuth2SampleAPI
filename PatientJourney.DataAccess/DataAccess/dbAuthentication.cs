using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientJourney.DataAccess.Data;

namespace PatientJourney.DataAccess.DataAccess
{
    public class dbAuthentication
    {
        public User GetUserforADlogonID(String UserName)
        {
            using(PJEntities _entity = new PJEntities())
            {
                var result = _entity.Users.Where(x => x.User_511.ToLower() == UserName.ToLower()).FirstOrDefault();
                return result;
            }
        }
        public List<User_Roles> GetRolesforUsers(Int32? UserId)
        {
            using (PJEntities _entity = new PJEntities())
            {
                var result = _entity.User_Roles.Where(x => x.User_Id == UserId).ToList();
                return result;
            }
        }
        public Role_Master GetRoleDetails(Int32? RoleId)
        {
            using (PJEntities _entity = new PJEntities())
            {
                var result = _entity.Role_Master.Where(x => x.Role_Master_Id == RoleId).FirstOrDefault();
                return result;
            }
        }
    }
}
