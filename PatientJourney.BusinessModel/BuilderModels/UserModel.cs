using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientJourney.BusinessModel.BuilderModels
{
    public class UserModel
    {
        public String UserID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String MiddleInitial { get; set; }
        public String Email { get; set; }
        public String UPI { get; set; }
        public String User511 { get; set; }
        public Boolean IsActive { get; set; }
        public String CreatedDate { get; set; }
        public String CreatedBy { get; set; }
        public String ModifiedDate { get; set; }
        public String ModifiedBy { get; set; }
        public String RoleIds { get; set; }
        public String CountryIds { get; set; }
        public String FullName { get; set; }
    }
}
