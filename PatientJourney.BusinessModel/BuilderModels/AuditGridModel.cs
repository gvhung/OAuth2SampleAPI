using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientJourney.BusinessModel.BuilderModels
{
    public class AuditGridModel
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String MiddleInitial { get; set; }
        public String FullName { get; set; }
        public String Email { get; set; }
        public String UPI { get; set; }
        public String User511 { get; set; }
        public String LogonClientDate { get; set; }
        public String LogonUTCDate { get; set; }
        public String LogonClientTimeZone { get; set; }
    }
}
