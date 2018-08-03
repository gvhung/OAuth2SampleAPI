using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientSpectrum.WebAPI.WebAPIModels
{
    public class GetInputDataModel
    {
         public string TherapeuticId_Input { get; set; }
        public string SubTherapeuticId_Input { get; set; }
        public string IndicationId_Input { get; set; }
        public string ProductId_Input { get; set; }
        public string AreaId_Input { get; set; }
        public string CountryId_Input { get; set; }
        public string ArchetypeId_Input { get; set; }
        public string Year_Input { get; set; }
        public bool? SaveFavourite { get; set; }
        public string SubTherapeuticName { get; set; }
        public string IndicationName { get; set; }
        public string TherapeuticName { get; set; }
    }
}