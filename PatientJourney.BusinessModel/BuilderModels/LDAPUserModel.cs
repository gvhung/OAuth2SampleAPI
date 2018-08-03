using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientJourney.BusinessModel.BuilderModels
{
    public class LDAPUserModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Territory { get; set; }
        public string Phone { get; set; }
        public string UPI { get; set; }

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }
    }
}
