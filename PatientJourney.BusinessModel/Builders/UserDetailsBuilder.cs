using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientJourney.BusinessModel.BuilderModels;

namespace PatientJourney.BusinessModel.Builders
{
    public class UserDetailsBuilder
    {
        public UserDetailsBuilder()
        {
            Userdetails = new UserModel();
            UserRoles = new List<RolesModel>();
            UserCountry = new List<CountryMaster_List>();
        }
        public UserModel Userdetails { get; set; }
        public List<RolesModel> UserRoles { get; set; }
        public List<CountryMaster_List> UserCountry { get; set; }
    }
}
