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
    
    public partial class Favourite_Search_Country
    {
        public int Favourite_Search_Country_Id { get; set; }
        public Nullable<int> User_Id { get; set; }
        public Nullable<int> Favourite_Search_Id { get; set; }
        public Nullable<int> Country_Master_Id { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual Country_Master Country_Master { get; set; }
        public virtual Favourite_Search Favourite_Search { get; set; }
        public virtual User User { get; set; }
    }
}
