using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientJourney.BusinessModel.BusinessModel
{
    public class KeyLeversModel
    {
        public String TransactionName { get; set; }
        public String StageName { get; set; }
        public String DisplayOrder { get; set; }
        public Decimal? DesirabilityPatient { get; set; }
        public Decimal? DesirabilityHCP { get; set; }
        public Decimal? DesirabilityPayor { get; set; }
        public Decimal? Feasiblity { get; set; }
        public Decimal? Viablity { get; set; }
        public Decimal? AggregatedRating { get; set; }
    }

    public class TabularViewModel
    {
        public string Key { get; set; }
        public List<TabularViewModelFinal> lsttabularViewDataFinal { get; set; }
    }

    public class TabularViewModelFinal
    {
        public String TransactionName { get; set; }
        public String StageName { get; set; }
        public String DisplayOrder { get; set; }
        public Decimal? DesirabilityPatient { get; set; }
        public Decimal? DesirabilityHCP { get; set; }
        public Decimal? DesirabilityPayor { get; set; }
        public Decimal? Feasiblity { get; set; }
        public Decimal? Viablity { get; set; }
    }
}
