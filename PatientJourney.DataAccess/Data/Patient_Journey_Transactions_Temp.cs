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
    
    public partial class Patient_Journey_Transactions_Temp
    {
        public Patient_Journey_Transactions_Temp()
        {
            this.Patient_Journey_Strategic_Moment_Temp = new HashSet<Patient_Journey_Strategic_Moment_Temp>();
            this.Patient_Journey_Strategic_Moment_Temp1 = new HashSet<Patient_Journey_Strategic_Moment_Temp>();
            this.Patient_Journey_Trans_Clin_Interventions_Temp = new HashSet<Patient_Journey_Trans_Clin_Interventions_Temp>();
            this.Patient_Journey_Transactions_AssociatedCosts_Temp = new HashSet<Patient_Journey_Transactions_AssociatedCosts_Temp>();
            this.Patient_Journey_Transactions_DesiredOutcomes_Temp = new HashSet<Patient_Journey_Transactions_DesiredOutcomes_Temp>();
            this.Patient_Journey_HCP_Insights_Temp = new HashSet<Patient_Journey_HCP_Insights_Temp>();
            this.Patient_Journey_Patient_Insights_Temp = new HashSet<Patient_Journey_Patient_Insights_Temp>();
            this.Patient_Journey_Payor_Insights_Temp = new HashSet<Patient_Journey_Payor_Insights_Temp>();
        }
    
        public int Patient_Journey_Transactions_Temp_Id { get; set; }
        public string Transaction_Title { get; set; }
        public string Description { get; set; }
        public int Patient_Journey_Temp_Id { get; set; }
        public int Patient_Journey_Stages_Temp_Id { get; set; }
        public int Transaction_Master_Id { get; set; }
        public int Transaction_Location_Master_Id { get; set; }
        public int Transaction_Display_Order { get; set; }
        public string Transaction_Location_Title { get; set; }
        public int HCP_Rating { get; set; }
        public int Payer_Rating { get; set; }
        public int Patient_Rating { get; set; }
        public int Feasibility_Rating { get; set; }
        public int Viability_Rating { get; set; }
        public string HCP_Description { get; set; }
        public string Payer_Description { get; set; }
        public string Patient_Description { get; set; }
        public string Feasibility_Description { get; set; }
        public string Viability_Description { get; set; }
        public string Created_By { get; set; }
        public System.DateTime Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public string Patient_Evidence { get; set; }
        public string HCP_Evidence { get; set; }
        public string Payer_Evidence { get; set; }
        public string Feasibility_Evidence { get; set; }
        public string Viability_Evidence { get; set; }
        public Nullable<int> Patient_Journey_Transactions_Id { get; set; }
    
        public virtual Patient_Journey_Stages_Temp Patient_Journey_Stages_Temp { get; set; }
        public virtual ICollection<Patient_Journey_Strategic_Moment_Temp> Patient_Journey_Strategic_Moment_Temp { get; set; }
        public virtual ICollection<Patient_Journey_Strategic_Moment_Temp> Patient_Journey_Strategic_Moment_Temp1 { get; set; }
        public virtual Patient_Journey_Temp Patient_Journey_Temp { get; set; }
        public virtual ICollection<Patient_Journey_Trans_Clin_Interventions_Temp> Patient_Journey_Trans_Clin_Interventions_Temp { get; set; }
        public virtual ICollection<Patient_Journey_Transactions_AssociatedCosts_Temp> Patient_Journey_Transactions_AssociatedCosts_Temp { get; set; }
        public virtual ICollection<Patient_Journey_Transactions_DesiredOutcomes_Temp> Patient_Journey_Transactions_DesiredOutcomes_Temp { get; set; }
        public virtual Transaction_Master Transaction_Master { get; set; }
        public virtual Transaction_Location_Master Transaction_Location_Master { get; set; }
        public virtual ICollection<Patient_Journey_HCP_Insights_Temp> Patient_Journey_HCP_Insights_Temp { get; set; }
        public virtual ICollection<Patient_Journey_Patient_Insights_Temp> Patient_Journey_Patient_Insights_Temp { get; set; }
        public virtual ICollection<Patient_Journey_Payor_Insights_Temp> Patient_Journey_Payor_Insights_Temp { get; set; }
    }
}
