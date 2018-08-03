using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientJourney.BusinessModel.BuilderModels
{
    public class CountryAssociationModel
    {
        public String CountryID { get; set; }
        public String CountryName { get; set; }
        public Boolean IsActive { get; set; }
    }
}
