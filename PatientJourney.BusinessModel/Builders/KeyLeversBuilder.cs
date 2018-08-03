using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientJourney.BusinessModel.BusinessModel
{
    public class KeyLeversBuilder
    {
        public List<KeyLeversModel> lstkeyLeversData { get; set; }
        public Int32? CountryID { get; set; }
        public Int32? ProductID { get; set; }
        public String CountryName { get; set; }
        public String ProductName { get; set; }
    }

    public class TabularViewBuilder
    {
        public List<TabularViewModel> lsttabularViewData { get; set; }
        public Int32? CountryID { get; set; }
        public Int32? ProductID { get; set; }
        public String CountryName { get; set; }
        public String ProductName { get; set; }
        public String Key { get; set; }
    }

    public class TopCountries
    {
        public Int32? CountryID { get; set; }
        public String CountryName { get; set; }
        public String TransactionName { get; set; }
        public Decimal? DesirabilityPatient { get; set; }
        public Decimal? DesirabilityHCP { get; set; }
        public Decimal? DesirabilityPayor { get; set; }
        public Decimal? Feasiblity { get; set; }
        public Decimal? Viablity { get; set; }
        public Decimal? FinalRating { get; set; }
        public Decimal? FinalRatingPercentage { get; set; }
        public int? Rank { get; set; }

    }

    public class TherapeuticAreaList
    {
        public int TherapeuticId { get; set; }
        public string TherapeuticName { get; set; }
    }


}
