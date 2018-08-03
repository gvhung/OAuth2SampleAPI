using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientJourney.BusinessModel.BusinessModel;
using PatientJourney.DataAccess.DataAccess;

namespace PatientJourney.Business
{
    public class bsVisualJourney
    {
        private dbVisualJourney _dbVisualJourney = new dbVisualJourney();

        public List<KeyLeversBuilder> GetKeyLeversforCountriesandBrands(String CountryIds, String BrandId, String YearId)
        {
            List<String> lstCounryId = CountryIds.Split(',').ToList();
            var patientJourneyMaster = _dbVisualJourney.GetPatientJourney();
            var patientJourneyStages = _dbVisualJourney.GetPatientJourneyStages();
            var patientJourneyTransactions = _dbVisualJourney.GetPatientJourneyTransactions();
            var stagesMasterData = _dbVisualJourney.GetStagesMasterData();
            List<KeyLeversBuilder> _lstKeyLeversBuilder = new List<KeyLeversBuilder>();

            for (int a = 0; a < lstCounryId.Count; a++)
            {
                KeyLeversBuilder _keyleverBuilder = new KeyLeversBuilder();
                _keyleverBuilder.CountryID = Convert.ToInt32(lstCounryId[a]);
                _keyleverBuilder.ProductID = Convert.ToInt32(BrandId);
                _keyleverBuilder.CountryName = _dbVisualJourney.GetCountryDetails().FirstOrDefault(ci => ci.Country_Master_Id == Convert.ToInt32(lstCounryId[a])).Country_Name;
                _keyleverBuilder.ProductName = _dbVisualJourney.GetProductDetails().FirstOrDefault(ci => ci.Brand_Master_Id == Convert.ToInt32(BrandId)).Brand_Name;

                int patientJourneyId = patientJourneyMaster.Where(x => x.Brand_Master_Id == Convert.ToInt32(BrandId) && x.Country_Master_Id == Convert.ToInt32(lstCounryId[a]) && x.Year == Convert.ToInt32(YearId)
                    && x.Status_Master_Id == 3)
                           .Select(x => x.Patient_Journey_Id).FirstOrDefault();

                if (patientJourneyId == 0)
                {
                    patientJourneyId = patientJourneyMaster.Where(x => x.Brand_Master_Id == Convert.ToInt32(BrandId) && x.Country_Master_Id == Convert.ToInt32(lstCounryId[a]) && x.Year == Convert.ToInt32(YearId)
                    && x.Status_Master_Id == 7)
                           .Select(x => x.Patient_Journey_Id).FirstOrDefault();
                }

                var stagesId = patientJourneyStages.Where(x => x.Patient_Journey_Id == patientJourneyId).Select(x => x.Patient_Journey_Stages_Id).ToList();

                List<KeyLeversModel> _lstKeyLeverModel = new List<KeyLeversModel>();
                for (int b = 0; b < stagesId.Count; b++)
                {                   
                    
                    int stagesID = Convert.ToInt32(stagesId[b]);
                    string stageName = patientJourneyStages.Where(x => x.Patient_Journey_Stages_Id == stagesID).Select(x => x.Stage_Title).FirstOrDefault().ToString();

                    var transactionId = patientJourneyTransactions.Where(x => x.Patient_Journey_Id == patientJourneyId &&
                                                  x.Patient_Journey_Stages_Id == stagesID).Select(x => x.Patient_Journey_Transactions_Id).ToList();

                    for (int c = 0; c < transactionId.Count; c++)
                    {
                        KeyLeversModel _keylever = new KeyLeversModel();
                        int TransactionId = Convert.ToInt32(transactionId[c]);

                        var _filteredbenchMarkRating = patientJourneyTransactions.Where(x => x.Patient_Journey_Transactions_Id == TransactionId).ToList();

                        if (_filteredbenchMarkRating != null)
                        {
                            _keylever.StageName = stageName;
                            _keylever.TransactionName = _filteredbenchMarkRating.Select(x => x.Transaction_Title).FirstOrDefault().ToString();
                            _keylever.DesirabilityPatient = _filteredbenchMarkRating.Select(x => x.Patient_Rating).FirstOrDefault();
                            _keylever.DesirabilityHCP = _filteredbenchMarkRating.Select(x => x.HCP_Rating).FirstOrDefault();
                            _keylever.DesirabilityPayor = _filteredbenchMarkRating.Select(x => x.Payer_Rating).FirstOrDefault();
                            _keylever.Feasiblity = _filteredbenchMarkRating.Select(x => x.Feasibility_Rating).FirstOrDefault();
                            _keylever.Viablity = _filteredbenchMarkRating.Select(x => x.Viability_Rating).FirstOrDefault();
                            _keylever.AggregatedRating = (Math.Max(Convert.ToDecimal(_keylever.DesirabilityHCP), Math.Max(Convert.ToDecimal(_keylever.DesirabilityPayor), Convert.ToDecimal(_keylever.DesirabilityPatient))));
                            _lstKeyLeverModel.Add(_keylever);
                        }
                    }
                }
                _keyleverBuilder.lstkeyLeversData = _lstKeyLeverModel.OrderByDescending(x => x.AggregatedRating).ToList();
                _lstKeyLeversBuilder.Add(_keyleverBuilder);
            }
            return _lstKeyLeversBuilder;
        }

        public List<TabularViewBuilder> GetTabularviewForVJ(String CountryIds, String BrandId, String YearId)
        {
            List<String> lstCounryId = CountryIds.Split(',').ToList();

            var patientJourneyMaster = _dbVisualJourney.GetPatientJourney();
            var patientJourneyStages = _dbVisualJourney.GetPatientJourneyStages();
            var patientJourneyTransactions = _dbVisualJourney.GetPatientJourneyTransactions();

            List<TabularViewBuilder> _lstTabularViewBuilder = new List<TabularViewBuilder>();

            for (int a = 0; a < lstCounryId.Count; a++)
            {
                TabularViewBuilder _tabularViewBuilder = new TabularViewBuilder();

                _tabularViewBuilder.CountryID = Convert.ToInt32(lstCounryId[a]);
                _tabularViewBuilder.ProductID = Convert.ToInt32(BrandId);
                _tabularViewBuilder.CountryName = _dbVisualJourney.GetCountryDetails().FirstOrDefault(ci => ci.Country_Master_Id == Convert.ToInt32(lstCounryId[a])).Country_Name;
                _tabularViewBuilder.ProductName = _dbVisualJourney.GetProductDetails().FirstOrDefault(ci => ci.Brand_Master_Id == Convert.ToInt32(BrandId)).Brand_Name;

                int patientJourneyId = patientJourneyMaster.Where(x => x.Brand_Master_Id == Convert.ToInt32(BrandId) && x.Country_Master_Id == Convert.ToInt32(lstCounryId[a]) && x.Year == Convert.ToInt32(YearId)
                    && x.Status_Master_Id == 3)
                           .Select(x => x.Patient_Journey_Id).FirstOrDefault();

                if (patientJourneyId == 0)
                {
                    patientJourneyId = patientJourneyMaster.Where(x => x.Brand_Master_Id == Convert.ToInt32(BrandId) && x.Country_Master_Id == Convert.ToInt32(lstCounryId[a]) && x.Year == Convert.ToInt32(YearId)
                    && x.Status_Master_Id == 7)
                           .Select(x => x.Patient_Journey_Id).FirstOrDefault();
                }

                var stagesId = patientJourneyStages.Where(x => x.Patient_Journey_Id == patientJourneyId).Select(x => x.Patient_Journey_Stages_Id).ToList();

                List<TabularViewModel> _lstTabularViewModel = new List<TabularViewModel>();

                for (int i = 0; i < stagesId.Count; i++)
                {
                    TabularViewModel _tabularViewVJ = new TabularViewModel();
                    int stagesID = Convert.ToInt32(stagesId[i]);

                    _tabularViewVJ.Key = patientJourneyStages.Where(x => x.Patient_Journey_Stages_Id == stagesID).Select(x => x.Stage_Title).FirstOrDefault();

                    var transactionId = patientJourneyTransactions.Where(x => x.Patient_Journey_Id == patientJourneyId &&
                                                  x.Patient_Journey_Stages_Id == stagesID).Select(x => x.Patient_Journey_Transactions_Id).ToList();

                    List<TabularViewModelFinal> _lstTabularViewModelFinal = new List<TabularViewModelFinal>();

                    for (int b = 0; b < transactionId.Count; b++)
                    {
                        TabularViewModelFinal _tabulaViewModelFinal = new TabularViewModelFinal();
                        int TransactionId = Convert.ToInt32(transactionId[b]);

                        var _filteredbenchMarkRating = patientJourneyTransactions.Where(x => x.Patient_Journey_Transactions_Id == TransactionId).ToList();

                        if (_filteredbenchMarkRating != null)
                        {
                            _tabulaViewModelFinal.TransactionName = _filteredbenchMarkRating.Select(x => x.Transaction_Title).FirstOrDefault().ToString();
                            _tabulaViewModelFinal.DesirabilityPatient = _filteredbenchMarkRating.Select(x => x.Patient_Rating).FirstOrDefault();
                            _tabulaViewModelFinal.DesirabilityHCP = _filteredbenchMarkRating.Select(x => x.HCP_Rating).FirstOrDefault();
                            _tabulaViewModelFinal.DesirabilityPayor = _filteredbenchMarkRating.Select(x => x.Payer_Rating).FirstOrDefault();
                            _tabulaViewModelFinal.Feasiblity = _filteredbenchMarkRating.Select(x => x.Feasibility_Rating).FirstOrDefault();
                            _tabulaViewModelFinal.Viablity = _filteredbenchMarkRating.Select(x => x.Viability_Rating).FirstOrDefault();
                            _lstTabularViewModelFinal.Add(_tabulaViewModelFinal);
                        }
                    }
                    _tabularViewVJ.lsttabularViewDataFinal = _lstTabularViewModelFinal.ToList();
                    _lstTabularViewModel.Add(_tabularViewVJ);
                }

                _tabularViewBuilder.lsttabularViewData = _lstTabularViewModel;
                _lstTabularViewBuilder.Add(_tabularViewBuilder);
            }
            return _lstTabularViewBuilder;
        }

        public List<TopCountries> GetTopCountriesForKeyLevers(String TransactionName, int ProductID, string StageName, int JourneyId, int Year)
        {
            List<TopCountries> _lstFinalTopCountriesBuilder = new List<TopCountries>();
            List<TopCountries> _lstTopCountriesBuilder = new List<TopCountries>();

            var patientJourneyMaster = _dbVisualJourney.GetPatientJourney();
            var patientJourneyStages = _dbVisualJourney.GetPatientJourneyStages();
            var patientJourneyTransactions = _dbVisualJourney.GetPatientJourneyTransactions();
            var countryList = _dbVisualJourney.GetCountryDetails();

            var statusId = _dbVisualJourney.GetJourneyStatus(JourneyId);

            var details = (from journey in patientJourneyMaster
                           join stages in patientJourneyStages
                           on journey.Patient_Journey_Id equals stages.Patient_Journey_Id
                           join transaction in patientJourneyTransactions
                           on stages.Patient_Journey_Stages_Id equals transaction.Patient_Journey_Stages_Id
                           where transaction.Transaction_Title == TransactionName
                           where journey.Status_Master_Id == statusId
                           where journey.Brand_Master_Id == ProductID
                           where journey.Year == Year
                           select new { TransactionId = transaction.Patient_Journey_Transactions_Id, CountryId = journey.Country_Master_Id }).ToList();


            for (int b = 0; b < details.Count; b++)
            {
                TopCountries _keylever = new TopCountries();
                var _filteredbenchMarkRating = patientJourneyTransactions.Where(x => x.Patient_Journey_Transactions_Id == details[b].TransactionId).ToList();

                if (_filteredbenchMarkRating != null)
                {
                    _keylever.DesirabilityPatient = _filteredbenchMarkRating.Select(x => x.Patient_Rating).FirstOrDefault();
                    _keylever.DesirabilityHCP = _filteredbenchMarkRating.Select(x => x.HCP_Rating).FirstOrDefault();
                    _keylever.DesirabilityPayor = _filteredbenchMarkRating.Select(x => x.Payer_Rating).FirstOrDefault();
                    _keylever.FinalRating = Convert.ToDecimal(_keylever.DesirabilityHCP) + Convert.ToDecimal(_keylever.DesirabilityPayor) + Convert.ToDecimal(_keylever.DesirabilityPatient);
                    _keylever.FinalRatingPercentage = (Math.Max(Convert.ToDecimal(_keylever.DesirabilityHCP), Math.Max(Convert.ToDecimal(_keylever.DesirabilityPayor), Convert.ToDecimal(_keylever.DesirabilityPatient))));
                }
                _keylever.CountryID = details[b].CountryId;
                _keylever.CountryName = countryList.Where(x => x.Country_Master_Id == details[b].CountryId).FirstOrDefault().Country_Name;
                _keylever.TransactionName = TransactionName;
                _lstTopCountriesBuilder.Add(_keylever);
            }

            var discList = _lstTopCountriesBuilder.GroupBy(x => x.CountryName, (k, g) => g.Aggregate((a, x) => (x.FinalRating > a.FinalRating) ? x : a));
            var rankList = discList.OrderByDescending(x => x.FinalRating).GroupBy(x => x.CountryName).SelectMany((g, i) => g.Select(e => new { Col1 = e, Rank = i + 1 })).ToList();
            var descList = discList.OrderByDescending(x => x.FinalRating).ToList();
            for (int indx = 0; indx < rankList.Count; indx++)
            {
                descList[indx].Rank = rankList[indx].Rank;
            }
            _lstFinalTopCountriesBuilder = descList.Take(5).ToList();

            return _lstFinalTopCountriesBuilder;
        }
    }
}
