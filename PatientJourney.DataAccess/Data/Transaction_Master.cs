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
    
    public partial class Transaction_Master
    {
        public Transaction_Master()
        {
            this.Patient_Journey_Transactions = new HashSet<Patient_Journey_Transactions>();
            this.Patient_Journey_Transactions_Temp = new HashSet<Patient_Journey_Transactions_Temp>();
        }
    
        public int Transaction_Master_Id { get; set; }
        public string Transaction_Name { get; set; }
        public string Transaction_Description { get; set; }
        public Nullable<int> Image_Master_Id { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public Nullable<int> Indication_Master_Id { get; set; }
    
        public virtual Image_Master Image_Master { get; set; }
        public virtual Indication_Master Indication_Master { get; set; }
        public virtual ICollection<Patient_Journey_Transactions> Patient_Journey_Transactions { get; set; }
        public virtual ICollection<Patient_Journey_Transactions_Temp> Patient_Journey_Transactions_Temp { get; set; }
    }
}