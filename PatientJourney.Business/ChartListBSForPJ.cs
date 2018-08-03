using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientJourney.BusinessModel;
using PatientJourney.DataAccess;
using PatientJourney.BusinessModel.BusinessModel;

namespace PatientJourney.Business
{
    public class ChartListBSForPJ
    {
        public static ChartModel GetChartListBS(ChartInput input)
        {
            ChartModel response = new ChartModel();

            input.lstAreaId = input.AreaId.Split(',').ToList();
            input.lstCountryId = input.CountryId.Split(',').ToList();
            input.lstProductId = input.ProductId.Split(',').ToList();

            response = ChartListDSForPJ.GetBarChartListDS(input);
            return response;
        }

        public static VJChartModel GetVJChartListBS(VJChartInput input)
        {
            VJChartModel response = new VJChartModel();

            input.lstAreaId = input.AreaId.Split(',').ToList();
            input.lstCountryId = input.CountryId.Split(',').ToList();
            input.lstProductId = input.ProductId.Split(',').ToList();

            response = ChartListDSForPJ.GetVJChartListDS(input);
            return response;
        }

        //public static KeyLeversBuilder GenerateKeyLeversBS(VJChartInput input)
        //{
        //    KeyLeversBuilder response = new KeyLeversBuilder();
        //    input.lstCountryId = input.CountryId.Split(',').ToList();
        //    response = ChartListDSForPJ.GenerateKeyLeversDS(input);
        //    return response;
        //}

        public static VJRadarModel GetVJRadarListBS(VJRadarInput input)
        {
            VJRadarModel response = new VJRadarModel();

            input.lstCountryId = input.CountryId.Split(',').ToList();
            input.lstProductId = input.ProductId.Split(',').ToList();

            response = ChartListDSForPJ.GetVJRadarChartListDS(input);
            return response;
        }
        
    }
}
