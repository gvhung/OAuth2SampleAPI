using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientJourney.BusinessModel;
using PatientJourney.DataAccess.Data;
using Microsoft.CSharp;
using Newtonsoft.Json;
using PatientJourney.BusinessModel.BusinessModel;

namespace PatientJourney.DataAccess
{
    public static class ChartListDSForPJ
    {
        public static ChartModel GetBarChartListDS(ChartInput input)
        {
            ChartModel response = new ChartModel();
            string countryName = "";
            string productName = "";
            int productID, countryID = 0;

            using (PJEntities entity = new PJEntities())
            {
                var patientJourneyData = from r in entity.Patient_Journey select r;
                var patientJourneystagesData = from r in entity.Patient_Journey_Stages.OrderBy(x => x.Stage_Display_Order) select r;
                var countryData = from r in entity.Country_Master select r;
                var brandData = from r in entity.Brand_Master select r;
                var stageMasterData = from r in entity.Stage_Master select r;
                var statusMasterData = from r in entity.Status_Master select r;

                List<TimeStatisticsProduct> timeStatsWithProd = new List<TimeStatisticsProduct>();

                for (int j = 0; j < input.lstProductId.Count; j++)
                {
                    List<TimeStatistics> listWithCountry = new List<TimeStatistics>();
                    TimeStatisticsProduct timeStatisticsProduct = new TimeStatisticsProduct();
                    productID = Convert.ToInt32(input.lstProductId[j]);
                    for (int i = 0; i < input.lstCountryId.Count; i++)
                    {
                        TimeStatistics timeStatistics = new BusinessModel.TimeStatistics();
                        countryID = Convert.ToInt32(input.lstCountryId[i]);

                        int patientJourneyId = 0;

                        patientJourneyId = patientJourneyData.Where(x => x.Country_Master_Id == countryID
                           && x.Brand_Master_Id == productID && x.Year == input.Year && x.Status_Master_Id == 3).
                           Select(x => x.Patient_Journey_Id).FirstOrDefault();

                        if (patientJourneyId == 0)
                        {
                            patientJourneyId = patientJourneyData.Where(x => x.Country_Master_Id == countryID
                           && x.Brand_Master_Id == productID && x.Year == input.Year && x.Status_Master_Id == 7).
                           Select(x => x.Patient_Journey_Id).FirstOrDefault();
                        }

                        response.BarchartList = (from kd in patientJourneystagesData
                                                 join c in stageMasterData on kd.Stage_Master_Id equals c.Stage_Master_Id
                                                 where kd.Patient_Journey_Id == patientJourneyId
                                                 select new BarChartData()
                                                 {
                                                     ID = kd.Patient_Journey_Stages_Id,
                                                     GroupName = c.Stage_Name,
                                                     StageName = c.Stage_Name,
                                                     Value = kd.Time_Statistics,
                                                     PopulationValue = (kd.Population_Statistics / 100),
                                                     StageDisplayOrder = kd.Stage_Display_Order
                                                 }).ToList();

                        countryName = countryData.FirstOrDefault(ci => ci.Country_Master_Id == countryID).Country_Name;

                        timeStatistics.country = countryName;
                        timeStatistics.countryId = countryID;
                        timeStatistics.BarchartList = response.BarchartList;
                        listWithCountry.Add(timeStatistics);
                    }
                    productName = brandData.FirstOrDefault(ci => ci.Brand_Master_Id == productID).Brand_Name;
                    timeStatisticsProduct.ProductName = productName;
                    timeStatisticsProduct.ProductId = productID;
                    timeStatisticsProduct.timeStatsList = listWithCountry;
                    timeStatsWithProd.Add(timeStatisticsProduct);
                }
                //country - colours mapping
                //only for 5 countries
                List<CountryColour> countryColourList = new List<CountryColour>();
                string[] coloursArry = new string[] { "#84BD00", "#6699C9", "#F49D45", "#996699", "#F2594B" };
                for (int i = 0; i < input.lstCountryId.Count; i++)
                {
                    CountryColour countryColour = new CountryColour();
                    countryColour.Colour = coloursArry[i];
                    int countryIDColour = Convert.ToInt32(input.lstCountryId[i]);
                    countryColour.CountryID = countryIDColour;
                    countryName = countryData.FirstOrDefault(ci => ci.Country_Master_Id == countryIDColour).Country_Name;
                    countryColour.CountryName = countryName;
                    countryColourList.Add(countryColour);
                }
                response.countryColourList = countryColourList;
                response.timeStatisticsProductData = timeStatsWithProd;
            }
            return response;
        }

        public static VJChartModel GetVJChartListDS(VJChartInput input)
        {
            VJChartModel response = new VJChartModel();
            string countryName = "";
            string productName = "";
            int productID, countryID = 0;

            using (PJEntities entity = new PJEntities())
            {
                var countryData = from r in entity.Country_Master select r;
                var productData = from r in entity.Brand_Master select r;
                var patientJourneyData = from r in entity.Patient_Journey select r;
                var patientJourneyStagesData = from r in entity.Patient_Journey_Stages.OrderBy(x => x.Stage_Display_Order) select r;
                var patientJourneyTransactionData = from r in entity.Patient_Journey_Transactions select r;

                List<PatientJourneyData> patientJourneyDataList = new List<PatientJourneyData>();
                List<SelectedProduct> FproductListWithData = new List<SelectedProduct>();

                for (int k = 0; k < input.lstProductId.Count; k++)
                {
                    SelectedProduct selectedProduct = new BusinessModel.SelectedProduct();
                    int productIDs = Convert.ToInt32(input.lstProductId[k]);

                    List<SelectedCountry> FcountryListWithData = new List<SelectedCountry>();

                    for (int i = 0; i < input.lstCountryId.Count; i++)
                    {
                        SelectedCountry selectedCountry = new BusinessModel.SelectedCountry();
                        int countryIds = Convert.ToInt32(input.lstCountryId[i]);

                        int patientJourneyId = 0;

                        patientJourneyId = patientJourneyData.Where(x => x.Brand_Master_Id == productIDs && x.Country_Master_Id == countryIds && x.Year == input.Year && x.Status_Master_Id == 3)
                            .Select(x => x.Patient_Journey_Id).FirstOrDefault();

                        if (patientJourneyId == 0)
                        {
                            patientJourneyId = patientJourneyData.Where(x => x.Brand_Master_Id == productIDs && x.Country_Master_Id == countryIds && x.Year == input.Year && x.Status_Master_Id == 7)
                                                        .Select(x => x.Patient_Journey_Id).FirstOrDefault();
                        }

                        var stagesId = patientJourneyStagesData.Where(x => x.Patient_Journey_Id == patientJourneyId).Select(x => x.Patient_Journey_Stages_Id).ToList();

                        List<PJTransactionList> PJTransactionListData = new List<PJTransactionList>();

                        for (int j = 0; j < stagesId.Count; j++)
                        {
                            int stagesID = Convert.ToInt32(stagesId[j]);
                            var transactionId = patientJourneyTransactionData.Where(x => x.Patient_Journey_Id == patientJourneyId &&
                               x.Patient_Journey_Stages_Id == stagesID).Select(x => x.Patient_Journey_Transactions_Id).ToList();

                            for (int n = 0; n < transactionId.Count; n++)
                            {
                                int TransactionId = Convert.ToInt32(transactionId[n]);

                                response.pjTransactionList = (from kd in patientJourneyTransactionData                                                              
                                                              where kd.Patient_Journey_Id == patientJourneyId
                                                              where kd.Patient_Journey_Stages_Id == stagesID
                                                              where kd.Patient_Journey_Transactions_Id == TransactionId
                                                              join kq in patientJourneyStagesData on kd.Patient_Journey_Stages_Id equals kq.Patient_Journey_Stages_Id
                                                              select new PJTransactionList()
                                                              {
                                                                  PatientJourneyId = patientJourneyId,
                                                                  PatientJourneyStagesId = stagesID,
                                                                  PatientJourneyTransactionId = TransactionId,
                                                                  Transaction_Title = kd.Transaction_Title,
                                                                  Stage_Title = kq.Stage_Title,                                                                  
                                                                  Patient_Rating = kd.Patient_Rating,
                                                                  HCP_Rating = kd.HCP_Rating,
                                                                  Payer_Rating = kd.Payer_Rating
                                                                  //,
                                                                  //Feasibility_Rating = kd.Feasibility_Rating,
                                                                  //Viability_Rating = kd.Viability_Rating
                                                              }).ToList();

                                PJTransactionListData.AddRange(response.pjTransactionList);
                            }
                        }

                        countryName = countryData.FirstOrDefault(ci => ci.Country_Master_Id == countryIds).Country_Name;

                        selectedCountry.CountryName = countryName;
                        selectedCountry.CountryID = countryIds;
                        selectedCountry.pjTransactionList = PJTransactionListData;
                        FcountryListWithData.Add(selectedCountry);
                    }

                    productName = productData.FirstOrDefault(ci => ci.Brand_Master_Id == productIDs).Brand_Name;
                    selectedProduct.ProductName = productName;
                    selectedProduct.ProductId = productIDs;
                    selectedProduct.selectedCountryList = FcountryListWithData;
                    FproductListWithData.Add(selectedProduct);
                }

                List<CountryColourVJ> countryColourLists = new List<CountryColourVJ>();
                string[] coloursArry = new string[] { "#84BD00", "#6699C9", "#F49D45", "#996699", "#F2594B" };
                for (int i = 0; i < input.lstCountryId.Count; i++)
                {
                    CountryColourVJ countryColour = new CountryColourVJ();
                    countryColour.Colours = coloursArry[i];
                    int countryIDColour = Convert.ToInt32(input.lstCountryId[i]);
                    countryColour.CountryIDs = countryIDColour;
                    countryName = countryData.FirstOrDefault(ci => ci.Country_Master_Id == countryIDColour).Country_Name;
                    countryColour.CountryNames = countryName;
                    countryColourLists.Add(countryColour);
                }


                response.countryColourLists = countryColourLists;
                response.SelectedProductList = FproductListWithData;
            }
            return response;
        }

      

        public static VJRadarModel GetVJRadarChartListDS(VJRadarInput input)
        {
            VJRadarModel response = new VJRadarModel();
            string countryName = "";
            string productName = "";
            int productID, countryID = 0;

            using (PJEntities entity = new PJEntities())
            {
                var countryData = from r in entity.Country_Master select r;
                var productData = from r in entity.Brand_Master select r;
                var patientJourneyData = from r in entity.Patient_Journey select r;
                var patientJourneyStagesData = from r in entity.Patient_Journey_Stages select r;
                var patientJourneyTransactionData = from r in entity.Patient_Journey_Transactions select r;

                List<PatientJourneyDataRadar> PatientJourneyDataRadarList = new List<PatientJourneyDataRadar>();
                List<SelectedProductRadar> FproductListWithData = new List<SelectedProductRadar>();

                for (int k = 0; k < input.lstProductId.Count; k++)
                {
                    SelectedProductRadar selectedProduct = new BusinessModel.SelectedProductRadar();
                    int productIDs = Convert.ToInt32(input.lstProductId[k]);

                    List<SelectedCountryRadar> FcountryListWithData = new List<SelectedCountryRadar>();

                    for (int i = 0; i < input.lstCountryId.Count; i++)
                    {
                        SelectedCountryRadar selectedCountry = new BusinessModel.SelectedCountryRadar();

                        int countryIds = Convert.ToInt32(input.lstCountryId[i]);

                        int patientJourneyId = patientJourneyData.Where(x => x.Brand_Master_Id == productIDs && x.Country_Master_Id == countryIds && x.Status_Master_Id == 3)
                          .Select(x => x.Patient_Journey_Id).FirstOrDefault();

                        var stagesId = patientJourneyStagesData.Where(x => x.Patient_Journey_Id == patientJourneyId).Select(x => x.Patient_Journey_Stages_Id).ToList();

                        List<BenchMarkDataListRadar> benchMarkListWithData = new List<BenchMarkDataListRadar>();

                        for (int j = 0; j < stagesId.Count; j++)
                        {
                            int stagesID = Convert.ToInt32(stagesId[j]);

                            var transactionId = patientJourneyTransactionData.Where(x => x.Patient_Journey_Id == patientJourneyId &&
                               x.Patient_Journey_Stages_Id == stagesID && x.Transaction_Title == input.TransactionName).Select(x => x.Patient_Journey_Transactions_Id).ToList();

                            for (int n = 0; n < transactionId.Count; n++)
                            {
                                int TransactionId = Convert.ToInt32(transactionId[n]);
                                response.benchMarkDataListRadar = (from kd in patientJourneyTransactionData
                                                                   where kd.Patient_Journey_Id == patientJourneyId
                                                                   where kd.Patient_Journey_Stages_Id == stagesID
                                                                   where kd.Patient_Journey_Transactions_Id == TransactionId
                                                                   select new BenchMarkDataListRadar()
                                                                   {
                                                                       PatientJourneyId = patientJourneyId,
                                                                       PatientJourneyStagesId = stagesID,
                                                                       PatientJourneyTransactionId = TransactionId,
                                                                       Transaction_Title = kd.Transaction_Title,
                                                                       HCP_Rating = kd.HCP_Rating,
                                                                       Patient_Rating = kd.Patient_Rating,
                                                                       Payer_Rating = kd.Payer_Rating,
                                                                       Feasibility_Rating = kd.Feasibility_Rating,
                                                                       Viability_Rating = kd.Viability_Rating
                                                                   })
                                                              .ToList();

                                benchMarkListWithData.AddRange(response.benchMarkDataListRadar);
                            }
                        }

                        countryName = countryData.FirstOrDefault(ci => ci.Country_Master_Id == countryIds).Country_Name;

                        selectedCountry.CountryName = countryName;
                        selectedCountry.CountryID = countryIds;
                        selectedCountry.benchMarkDataListRadar = benchMarkListWithData;
                        FcountryListWithData.Add(selectedCountry);
                    }

                    productName = productData.FirstOrDefault(ci => ci.Brand_Master_Id == productIDs).Brand_Name;
                    selectedProduct.ProductName = productName;
                    selectedProduct.ProductId = productIDs;
                    selectedProduct.selectedCountryListRadar = FcountryListWithData;
                    FproductListWithData.Add(selectedProduct);
                }

                response.ProductlistRadar = FproductListWithData;

                return response;
            }
        }
    }
}
