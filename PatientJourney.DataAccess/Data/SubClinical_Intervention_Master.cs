//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PatientJourney.DataAccess.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class SubClinical_Intervention_Master
    {
        public SubClinical_Intervention_Master()
        {
            this.Patient_Journey_Trans_SubClin_Interventions = new HashSet<Patient_Journey_Trans_SubClin_Interventions>();
            this.Patient_Journey_Trans_SubClin_Interventions_Temp = new HashSet<Patient_Journey_Trans_SubClin_Interventions_Temp>();
        }
    
        public int SubClinical_Intervention_Master_Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Image_Master_Id { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual Image_Master Image_Master { get; set; }
        public virtual ICollection<Patient_Journey_Trans_SubClin_Interventions> Patient_Journey_Trans_SubClin_Interventions { get; set; }
        public virtual ICollection<Patient_Journey_Trans_SubClin_Interventions_Temp> Patient_Journey_Trans_SubClin_Interventions_Temp { get; set; }
    }
}
