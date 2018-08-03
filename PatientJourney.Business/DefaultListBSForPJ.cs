using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientJourney.BusinessModel;
using PatientJourney.DataAccess;
using PatientJourney.DataAccess.Data;

namespace PatientJourney.Business
{
    public static class DefaultListBSForPJ
    {
        public static GetBrandAndAreaListPJ GetBrandAndAreaListBSForPJ()
        {
            GetBrandAndAreaListPJ listresponse = new GetBrandAndAreaListPJ();
            listresponse = DefaultListDSForPJ.GetBrandAndAreaListDSForPJ();

            return listresponse;
        }

        public static PatientAdminMasterData GetTherapeuticandArchetypeData(string CountryIds)
        {
            int[] countryid = CountryIds.Split(',').Select(int.Parse).ToArray();
            PatientAdminMasterData listresponse = new PatientAdminMasterData();
            listresponse = DefaultListDSForPJ.GetTherapeuticandArchetypeData(countryid);

            return listresponse;
        }

        public static GetSubTherapeuticAreaListPJ GetSubTherapeuticListBSForPJ(int TherapeuticId)
        {
            GetSubTherapeuticAreaListPJ listresponse = new GetSubTherapeuticAreaListPJ();
            listresponse = DefaultListDSForPJ.GetSubTherapeuticListDSForPJ(TherapeuticId);

            return listresponse;
        }


        public static GetIndicationListPJ GetIndicationListBSForPJ(int SubTherapeuticId, int TherapeuticId)
        {
            GetIndicationListPJ listresponse = new GetIndicationListPJ();
            listresponse = DefaultListDSForPJ.GetIndicationListDSForPJ(SubTherapeuticId, TherapeuticId);

            return listresponse;
        }

        public static List<IndicationList2> GetProductListBSForPJ(int IndicationId, int SubTherapeuticId, int TherapeuticId)
        {
            var listresponse = DefaultListDSForPJ.GetProductListDSForPJ(IndicationId, SubTherapeuticId, TherapeuticId);
            return listresponse;
        }

        public static List<AreaList2> GetCountryListBSForPJ(String AreaId)
        {
            List<String> lstAreaId = new List<String>();

            lstAreaId = AreaId.Split(',').ToList();
            var result = DefaultListDSForPJ.GetCountryListDSForPJ(lstAreaId);
            return result;
        }

        public static List<CountryList> GetCountryData(String AreaId, String CountryIds)
        {
            int[] countryid = CountryIds.Split(',').Select(int.Parse).ToArray();
            List<CountryList> listresponse = new List<CountryList>();
            listresponse = DefaultListDSForPJ.GetCountryData(Convert.ToInt32(AreaId), countryid);

            return listresponse;
        }

        public static GetArchitypeListPJ GetAreaandCountryDataPJ(String ArchitypeId)
        {
            List<String> lstArchitypeId = new List<String>();

            lstArchitypeId = ArchitypeId.Split(',').ToList();
            var result = DefaultListDSForPJ.GetAreaandCountryDataPJ(lstArchitypeId);

            return result;
        }

        public static GetAreaFromArchitypeListPJ GetAreaFromCountryListBSForPJ(String CountryId)
        {
            List<String> lstCountryId = new List<String>();

            lstCountryId = CountryId.Split(',').ToList();
            var result = DefaultListDSForPJ.GetAreaFromCountryListDSForPJ(lstCountryId);
            return result;
        }
        
        public static SEARCH_CRITERIA GetSearchCriteria(String UserName)
        {
            var result = DefaultListDSForPJ.GetSearchCriteria(UserName);

            return result;
        }

        public static string GetTherapeuticName(int TherapeuticId)
        {
            var listresponse = DefaultListDSForPJ.GetTherapeuticName(TherapeuticId);
            return listresponse;
        }

        public static string GetSubTherapeuticName(int SubTherapeuticId)
        {
            var listresponse = DefaultListDSForPJ.GetSubTherapeuticName(SubTherapeuticId);
            return listresponse;
        }

        public static string GetIndicationName(int IndicationId)
        {
            var listresponse = DefaultListDSForPJ.GetIndicationName(IndicationId);
            return listresponse;
        }

        public static string GetBrandName(string BrandId)
        {
            List<String> lstBrandId = new List<String>();
            lstBrandId = BrandId.Split(',').ToList();

            var listresponse = DefaultListDSForPJ.GetProductNames(lstBrandId);

            string result = string.Join(",", listresponse.Select(r => r.ProductName));

            return result;
        }

        public static string GetArchetypeName(string ArchetypeId)
        {
            List<String> lstArchetypeId = new List<String>();
            lstArchetypeId = ArchetypeId.Split(',').ToList();

            var listresponse = DefaultListDSForPJ.GetArchetypeName(lstArchetypeId);
            string result = string.Join(",", listresponse.Select(r => r.ArchetypeName));

            return result;
        }

        public static string GetAreaName(string AreaId)
        {
            List<String> lstAreaId = new List<String>();
            lstAreaId = AreaId.Split(',').ToList();

            var listresponse = DefaultListDSForPJ.GetAreaName(lstAreaId);

            string result = string.Join(",", listresponse.Select(r => r.AreaName));

            return result;
        }

        public static string GetCountryName(string CountryId)
        {
            List<String> lstCountryId = new List<String>();
            lstCountryId = CountryId.Split(',').ToList();

            var listresponse = DefaultListDSForPJ.GetCountryNames(lstCountryId);

            string result = string.Join(",", listresponse.Select(r => r.CountryNames));

            return result;
        }

        public static string SaveSearchCriteria(string TherapeuticId, string SubTherapeuticId, string IndicationId, string ArchetypeId, string AreaId, string CountryId, string ProductId, string Year, string UserName)
        {

            List<String> lstCountryId = new List<String>();
            lstCountryId = CountryId.Split(',').ToList();

            List<String> lstProductId = new List<String>();
            lstProductId = ProductId.Split(',').ToList();

            List<String> lstAreaId = new List<String>();
            lstAreaId = AreaId.Split(',').ToList();

            List<String> lstArchetypeId = new List<String>();
            lstArchetypeId = ArchetypeId.Split(',').ToList();

            var listresponse = DefaultListDSForPJ.SaveSearchCriteria(TherapeuticId, SubTherapeuticId, IndicationId, lstArchetypeId, lstCountryId, lstProductId, lstAreaId, Year, UserName);
            return listresponse;
        }

        //public static UserListResult GetUserList()
        //{
        //    UserListResult result = new UserListResult();
        //    result = DefaultListDSForPJ.GetUserList();

        //    return result;
        //}

        //public static string AddNewUser(string FirstName, string MiddleName, string LastName, string EmailId, string UPI,
        //    string ADLogon, string userTypeID, string RoleID, string countryID)
        //{
        //    List<String> lstCountryId = new List<String>();
        //    lstCountryId = countryID.Split(',').ToList();

        //    var listresponse = DefaultListDSForPJ.AddNewUser(FirstName, MiddleName, LastName, EmailId, UPI, ADLogon, userTypeID, RoleID, lstCountryId);

        //    return listresponse;
        //}

        public static List<StageList> GetStageData(string JourneyId, int StatusId)
        {
            List<StageList> listStage = new List<StageList>();
            var listresponse = DefaultListDSForPJ.GetStageData(Convert.ToInt32(JourneyId), StatusId);

            for (int i = 0; i < listresponse.Count; i++)
            {
                if (!listresponse[i].Stage_Name.ToLower().Equals("other"))
                {
                    StageList stage = new StageList();
                    stage.StageMasterId = listresponse[i].Stage_Master_Id;
                    stage.StageName = listresponse[i].Stage_Name;
                    listStage.Add(stage);
                }
            }

            var last = listresponse.Where(x => x.Stage_Name.ToLower().Equals("other")).FirstOrDefault();
            StageList stage1 = new StageList();
            stage1.StageMasterId = last.Stage_Master_Id;
            stage1.StageName = last.Stage_Name;
            listStage.Add(stage1);

            return listStage;
        }

        public static List<AreaList> GetAreaListBSForPJ(int ArchetypeId)
        {
            List<AreaList> listresponse = new List<AreaList>();
            listresponse = DefaultListDSForPJ.GetAreaListDSForPJ(ArchetypeId);

            return listresponse;
        }

        public static List<TransactionMasterData> GetTransactionMasterData(string StageId, string TransactionId, string IndicationId, int StatusId)
        {
            List<TransactionMasterData> listTransaction = new List<TransactionMasterData>();
            TransactionMasterData transaction = null;
            List<Transaction_Master> listresponse = new List<Transaction_Master>();

            listresponse = DefaultListDSForPJ.GetTransactionMasterData(Convert.ToInt32(StageId), Convert.ToInt32(TransactionId), Convert.ToInt32(IndicationId), StatusId);
            for (int i = 0; i < listresponse.Count; i++)
            {
                transaction = new TransactionMasterData();
                transaction.TransactionMasterId = listresponse[i].Transaction_Master_Id;
                transaction.TransactionName = listresponse[i].Transaction_Name;
                listTransaction.Add(transaction);
            }
            return listTransaction;
        }

        public static List<LocationMasterData> GetLocationMasterData()
        {
            List<LocationMasterData> listLocation = new List<LocationMasterData>();
            LocationMasterData location = null;
            List<Transaction_Location_Master> listresponse = new List<Transaction_Location_Master>();

            listresponse = DefaultListDSForPJ.GetLocationMasterData();
            for (int i = 0; i < listresponse.Count; i++)
            {
                location = new LocationMasterData();
                location.LocationMasterId = listresponse[i].Transaction_Location_Master_Id;
                location.LocationName = listresponse[i].Location_Name;
                listLocation.Add(location);
            }
            return listLocation;
        }

        public static List<CountryColourVJ> GetCountryNames(String CountryId)
        {
            List<String> lstCountryId = new List<String>();

            lstCountryId = CountryId.Split(',').ToList();
            var result = DefaultListDSForPJ.GetCountryNames(lstCountryId);
            return result;
        }

        public static List<SelectedProduct> GetProductNames(String ProductID)
        {
            List<String> lstproductId = new List<String>();

            lstproductId = ProductID.Split(',').ToList();
            var result = DefaultListDSForPJ.GetProductNames(lstproductId);
            return result;
        }

        public static List<CountryList> GetAllCountries(string CountryIds)
        {
            int[] countryid = CountryIds.Split(',').Select(int.Parse).ToArray();
            List<CountryList> lstCountry = new List<CountryList>();
            CountryList country = null;
            var result = DefaultListDSForPJ.GetAllCountries(countryid);
            for(int i=0;i<result.Count;i++)
            {
                country = new CountryList();
                country.CountryId = result[i].Country_Master_Id;
                country.CountryName = result[i].Country_Name;
                lstCountry.Add(country);
            }
            return lstCountry;
        }

        public static List<ClinicalMasterData> GetClinicalMasterData()
        {
            List<ClinicalMasterData> lstClinical = new List<ClinicalMasterData>();
            ClinicalMasterData clinical = null;
            var result = DefaultListDSForPJ.GetClinicalMasterData();
            for(int i=0;i<result.Count;i++)
            {
                clinical = new ClinicalMasterData();
                clinical.ClinicalMasterId = result[i].Clinical_Intervention_Master_Id;
                clinical.ClinicalName = result[i].Title;
                lstClinical.Add(clinical);
            }
            return lstClinical;
        }

        public static List<SubClinicalMasterData> GetSubClinicalMasterData()
        {
            List<SubClinicalMasterData> lstSubClinical = new List<SubClinicalMasterData>();
            SubClinicalMasterData subclinical = null;
            var result = DefaultListDSForPJ.GetSubClinicalMasterData();
            for(int i=0;i<result.Count;i++)
            {
                subclinical = new SubClinicalMasterData();
                subclinical.SubClinicalMasterId = result[i].SubClinical_Intervention_Master_Id;
                subclinical.SubClinicalName = result[i].Title;
                lstSubClinical.Add(subclinical);
            }
            return lstSubClinical;
        }

        public static List<YearMasterData> GetYearMasterData()
        {
            List<YearMasterData> lstYear = new List<YearMasterData>();
            YearMasterData year = null;
            var result = DefaultListDSForPJ.GetYearMasterData();
            for (int i = 0; i < result.Count; i++)
            {
                year = new YearMasterData();
                year.YearId = result[i].Year_Master_Id;
                year.YearName = result[i].Year_Name;
                lstYear.Add(year);
            }
            return lstYear;
        }
    }
}