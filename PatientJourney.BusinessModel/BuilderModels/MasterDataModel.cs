using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientJourney.BusinessModel.BuilderModels
{
    public class MasterDataModel
    {
        public List<CountryMaster_List> CountryMasterList { get; set; }
        public List<RoleMaster_List> RoleMasterList { get; set; }      
    }

    public class CountryMaster_List
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }

    public class RoleMaster_List
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
