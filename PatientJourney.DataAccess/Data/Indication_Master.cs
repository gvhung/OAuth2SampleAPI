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
    
    public partial class Indication_Master
    {
        public Indication_Master()
        {
            this.Brand_Master = new HashSet<Brand_Master>();
            this.Favourite_Search = new HashSet<Favourite_Search>();
            this.Transaction_Master = new HashSet<Transaction_Master>();
        }
    
        public int Indication_Master_Id { get; set; }
        public string Indication_Name { get; set; }
        public Nullable<int> Therapeutic_Area_Master_Id { get; set; }
        public Nullable<int> SubTherapeutic_Area_Master_Id { get; set; }
        public bool Is_Active { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
        public virtual ICollection<Brand_Master> Brand_Master { get; set; }
        public virtual ICollection<Favourite_Search> Favourite_Search { get; set; }
        public virtual SubTherapeutic_Area_Master SubTherapeutic_Area_Master { get; set; }
        public virtual Therapeutic_Area_Master Therapeutic_Area_Master { get; set; }
        public virtual ICollection<Transaction_Master> Transaction_Master { get; set; }
    }
}
