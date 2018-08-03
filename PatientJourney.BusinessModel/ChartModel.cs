using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientJourney.BusinessModel
{
    public class ChartModel
    {
        public List<Benchmarks> BenchMarksList { get; set; }
        public List<Stages> StagesList { get; set; }
        public List<BarChartData> BarchartList { get; set; }
        public List<LineChartData> LinechartList { get; set; }
        public List<TimeStatistics> timeStatisticsData { get; set; }
        public List<TimeStatisticsProduct> timeStatisticsProductData { get; set; }
        public List<CountryColour> countryColourList { get; set; }
    }

    public class CountryColour
    {
        public int CountryID { get; set; }
        public string Colour { get; set; }
        public string CountryName { get; set; }
    }

    public class CountryColourVJ
    {
        public int CountryIDs { get; set; }
        public string Colours { get; set; }
        public string CountryNames { get; set; }
    }

    public class VJRadarModel
    {
        public List<PatientJourneyDataRadar> PatientJourneyDataIDRadar { get; set; }
        public List<BenchMarkDataListRadar> benchMarkDataListRadar { get; set; }

        public List<SelectedProductRadar> ProductlistRadar { get; set; }
    }

    public class PatientJourneyDataRadar
    {
        public int PatientJourneyID { get; set; }
        public int? CountryId { get; set; }
        public int? ProductId { get; set; }
    }

    public class SelectedProductRadar
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public List<SelectedCountryRadar> selectedCountryListRadar { get; set; }
    }

    public class SelectedCountryRadar
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public List<BenchMarkDataListRadar> benchMarkDataListRadar { get; set; }
    }

    public class BenchMarkDataListRadar
    {
        public int? PatientJourneyId { get; set; }
        public int? PatientJourneyStagesId { get; set; }
        public int? PatientJourneyTransactionId { get; set; }
        public int? HCP_Rating { get; set; }
        public int? Payer_Rating { get; set; }
        public int? Patient_Rating { get; set; }
        public int? Feasibility_Rating { get; set; }
        public int? Viability_Rating { get; set; }
        public string Transaction_Title { get; set; }
    }

    public class VJChartModel
    {
        public List<Benchmarks> BenchMarksList { get; set; }
        public List<PatientJourneyData> PatientJourneyIdDataList { get; set; }
        public List<SelectedCountry> SelectedCountryList { get; set; }
        public List<PJTransactionList> pjTransactionList { get; set; }
        public List<SelectedProduct> SelectedProductList { get; set; }
        public List<SelectedBenchMark> SelectedbenchmarkList { get; set; }
        public List<CountryColourVJ> countryColourLists { get; set; }
    }

    public class SelectedProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public List<SelectedCountry> selectedCountryList { get; set; }
    }

    public class SelectedCountry
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public List<PJTransactionList> pjTransactionList { get; set; }
    }

    public class SelectedBenchMark
    {
        public int BenchmarkID { get; set; }
        public string BenchmarkName { get; set; }
        public List<BenchMarkDataList> benchMarkDataList { get; set; }
    }

    public class BenchMarkDataList
    {
        public int? StagesAndGroupId { get; set; }
        public string BenchmarkName { get; set; }
        public string StagesName { get; set; }
        public decimal? Rating { get; set; }
        public string LegendsName { get; set; }
    }

    public class PJTransactionList
    {
        public int? PatientJourneyId { get; set; }
        public int? PatientJourneyStagesId { get; set; }
        public int? PatientJourneyTransactionId { get; set; }
        public int? HCP_Rating { get; set; }
        public int? Payer_Rating { get; set; }
        public int? Patient_Rating { get; set; }
        public int? Feasibility_Rating { get; set; }
        public int? Viability_Rating { get; set; }
        public string Transaction_Title { get; set; }
        public string Stage_Title { get; set; }
    }

    public class Benchmarks
    {
        public int ID { get; set; }
        public string BenchmarkName { get; set; }
        public string GroupName { get; set; }
    }

    public class Stages
    {
        public int ID { get; set; }
        public string GroupName { get; set; }
        public string StageName { get; set; }
    }

    public class BarChartData
    {
        public int ID { get; set; }
        public string GroupName { get; set; }
        public string StageName { get; set; }
        public decimal? Value { get; set; }
        public decimal? PopulationValue { get; set; }
        public int? StageDisplayOrder { get; set; }
    }

    public class PatientJourneyData
    {
        public int PatientJourneyID { get; set; }
        public int? CountryId { get; set; }
        public int? ProductId { get; set; }
    }

    public class LineChartData
    {
        public int ID { get; set; }
        public int benchMarkID { get; set; }
        public int stagesAndGroupId { get; set; }
        public decimal Rating { get; set; }
        public string Insight { get; set; }
    }

    public class Journey_Pdf_TempList
    {
        public int Id { get; set; }
        public int Country_Id { get; set; }
        public int Brand_Id { get; set; }
        public int Year { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }

    public class ChartInput
    {
        public int SubTherapeuticId { get; set; }
        public int TherapeuticId { get; set; }
        public int IndicationId { get; set; }
        public string AreaId { get; set; }
        public string CountryId { get; set; }
        public string ProductId { get; set; }
        public int Year { get; set; }

        public List<string> lstAreaId { get; set; }
        public List<string> lstCountryId { get; set; }
        public List<string> lstProductId { get; set; }
    }

    public class MasterDataInput
    {
        public int SubTherapeuticId { get; set; }
        public int TherapeuticId { get; set; }
        public int IndicationId { get; set; }
        public string ArchetypeId { get; set; }
        public string AreaId { get; set; }
        public string CountryId { get; set; }
        public string ProductId { get; set; }
        public int Year { get; set; }
        public string SubTherapeuticName { get; set; }
        public string IndicationName { get; set; }
        public string TherapeuticName { get; set; }
        public string BrandName { get; set; }
        public string ArchetypeName { get; set; }
        public string AreaName { get; set; }
        public string CountryName { get; set; }
        public bool? SaveFavourite { get; set; }
        public bool? FirstLoad { get; set; }
        public bool? ResetSession { get; set; }

        public List<string> lstAreaId { get; set; }
        public List<string> lstCountryId { get; set; }
        public List<string> lstProductId { get; set; }

        public List<TherapeuticAreaList> TherapeuticList { get; set; }
        public List<SubTherapeuticAreaList> SubTherapeuticList { get; set; }
        public List<IndicationList> IndicationList { get; set; }
        public List<IndicationList2> ProductList { get; set; }
        public List<AreaList> AreaList { get; set; }
        public List<AreaList2> CountryList { get; set; }
        public List<ArchetypeList> ArchetypeList { get; set; }
        public List<YearList> YearList { get; set; }

        public List<CountryColourVJ> CountryNames { get; set; }
        public List<SelectedProduct> ProductNames { get; set; }
        

    }


    public class UserListResult
    {
        public List<UserList> UserList { get; set; }
    }

    public class UserList
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public decimal UPI { get; set; }
        public string AD_Logon { get; set; }
        public string Role { get; set; }
    }


    public class SearchResultList
    {
        public List<JourneySearchResult> SearchResult { get; set; }
    }

    public class JourneySearchResult
    {
        public int JourneyNumber { get; set; }
        public int JourneyId { get; set; }
        public string JourneyName { get; set; }
        public string JourneyDescription { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string StrCreatedDate { get; set; }
        public string SubmitComments { get; set; }
        public string ApproverComments { get; set; }
        public int CurrentUserRole { get; set; }
        public int isCurrentUserRecord { get; set; }
        public int Year { get; set; }
    }

    public class JourneyResultList
    {
        public List<JourneyResult> JourneyResult { get; set; }
    }

    public class JourneyResult
    {
        public string StageName { get; set; }
        public int Transactions { get; set; }
        public int PopulationStatistics { get; set; }
        public int TimeStatistics { get; set; }
    }
    
    public class VJChartInput
    {
        public int SubTherapeuticId { get; set; }
        public int TherapeuticId { get; set; }
        public int IndicationId { get; set; }
        public string AreaId { get; set; }
        public string CountryId { get; set; }
        public string ProductId { get; set; }
        public int Year { get; set; }
        public int BrandId { get; set; }

        public List<string> lstAreaId { get; set; }
        public List<string> lstCountryId { get; set; }
        public List<string> lstProductId { get; set; }
    }

    public class VJRadarInput
    {
        public string CountryId { get; set; }
        public string ProductId { get; set; }
        public int Year { get; set; }
        public int BrandId { get; set; }
        public string TransactionName { get; set; }

        public List<string> lstCountryId { get; set; }
        public List<string> lstProductId { get; set; }
    }

    public class TimeStatisticsProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public List<TimeStatistics> timeStatsList { get; set; }
    }

    public class TimeStatistics
    {
        public string country { get; set; }
        public int countryId { get; set; }
        public List<BarChartData> BarchartList { get; set; }
    }



}
