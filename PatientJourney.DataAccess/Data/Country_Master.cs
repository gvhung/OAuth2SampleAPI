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
    
    public partial class Country_Master
    {
        public Country_Master()
        {
            this.Favourite_Search_Country = new HashSet<Favourite_Search_Country>();
            this.User_Country_Association = new HashSet<User_Country_Association>();
            this.Journey_Pdf = new HashSet<Journey_Pdf>();
            this.Patient_Journey_Temp = new HashSet<Patient_Journey_Temp>();
            this.Patient_Journey = new HashSet<Patient_Journey>();
        }
    
        public int Country_Master_Id { get; set; }
        public string Country_Name { get; set; }
        public Nullable<int> Area_Master_Id { get; set; }
        public bool Is_Active { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public Nullable<int> Archetype_Master_Id { get; set; }
    
        public virtual Archetype_Master Archetype_Master { get; set; }
        public virtual Area_Master Area_Master { get; set; }
        public virtual Area_Master Area_Master1 { get; set; }
        public virtual ICollection<Favourite_Search_Country> Favourite_Search_Country { get; set; }
        public virtual ICollection<User_Country_Association> User_Country_Association { get; set; }
        public virtual ICollection<Journey_Pdf> Journey_Pdf { get; set; }
        public virtual ICollection<Patient_Journey_Temp> Patient_Journey_Temp { get; set; }
        public virtual ICollection<Patient_Journey> Patient_Journey { get; set; }
    }
}
