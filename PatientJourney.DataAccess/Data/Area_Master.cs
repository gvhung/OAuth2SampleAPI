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
    
    public partial class Area_Master
    {
        public Area_Master()
        {
            this.Country_Master = new HashSet<Country_Master>();
            this.Country_Master1 = new HashSet<Country_Master>();
            this.Favourite_Search_Area = new HashSet<Favourite_Search_Area>();
        }
    
        public int Area_Master_Id { get; set; }
        public string Area_Name { get; set; }
        public bool Is_Active { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public Nullable<int> Archetype_Master_Id { get; set; }
    
        public virtual Archetype_Master Archetype_Master { get; set; }
        public virtual ICollection<Country_Master> Country_Master { get; set; }
        public virtual ICollection<Country_Master> Country_Master1 { get; set; }
        public virtual ICollection<Favourite_Search_Area> Favourite_Search_Area { get; set; }
    }
}
