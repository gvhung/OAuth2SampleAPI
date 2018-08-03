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
    
    public partial class Favourite_Search
    {
        public Favourite_Search()
        {
            this.Favourite_Search_Archetype = new HashSet<Favourite_Search_Archetype>();
            this.Favourite_Search_Area = new HashSet<Favourite_Search_Area>();
            this.Favourite_Search_Brand = new HashSet<Favourite_Search_Brand>();
            this.Favourite_Search_Country = new HashSet<Favourite_Search_Country>();
        }
    
        public int Favourite_Search_Id { get; set; }
        public Nullable<int> User_Id { get; set; }
        public bool Is_Active { get; set; }
        public Nullable<int> Therapeutic_Area_Master_Id { get; set; }
        public Nullable<int> SubTherapeutic_Area_Master_Id { get; set; }
        public Nullable<int> Indication_Master_Id { get; set; }
        public int Year { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual ICollection<Favourite_Search_Archetype> Favourite_Search_Archetype { get; set; }
        public virtual ICollection<Favourite_Search_Area> Favourite_Search_Area { get; set; }
        public virtual ICollection<Favourite_Search_Brand> Favourite_Search_Brand { get; set; }
        public virtual ICollection<Favourite_Search_Country> Favourite_Search_Country { get; set; }
        public virtual Indication_Master Indication_Master { get; set; }
        public virtual SubTherapeutic_Area_Master SubTherapeutic_Area_Master { get; set; }
        public virtual Therapeutic_Area_Master Therapeutic_Area_Master { get; set; }
        public virtual User User { get; set; }
    }
}
