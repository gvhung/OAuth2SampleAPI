using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientJourney.BusinessModel
{
   public class InputValidationModel
    {
        public bool Isvalid { get; set; }
        public string ErrorString { get; set; }
        public bool IsEmpty { get; set; }
    }
}
