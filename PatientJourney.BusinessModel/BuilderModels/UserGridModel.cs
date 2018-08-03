using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientJourney.BusinessModel.BuilderModels
{
    public class UserGridModel
    {
        public int UserID { get; set; }
        public String RoleID { get; set; }
        public int CountryID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String MiddleInitial { get; set; }
        public String FullName { get; set; }
        public String RoleName { get; set; }
        public String Email { get; set; }
        public decimal UPI { get; set; }
        public String User511 { get; set; }
        public Boolean IsActive { get; set; }
    }
}
