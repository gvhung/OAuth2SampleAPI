using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientJourney.BusinessModel
{
    public class GetBrandAndAreaListPJ
    {
        public List<TherapeuticAreaList> TherapeuticList { get; set; }
        public List<AreaList> AreaList { get; set; }
        public List<AreaList2> AreaList2 { get; set; }
        public List<YearList> YearList { get; set; }
        public List<IndicationList> IndicationList { get; set; }
        public List<ProductList> ProductList { get; set; }
        public List<CountryList> CountryList { get; set; }
        public List<SubTherapeuticAreaList> SubTherapeuticList { get; set; }
        public string UserNameForDisplay { get; set; }
        public List<ArchetypeList> ArchetypeLists { get; set; }
        public bool? SearchPresent { get; set; }
    }

    public class ArchetypeList
    {
        public int? ArchetypeId { get; set; }
        public string ArchetypeName { get; set; }
    }

    public class GetSubTherapeuticAreaListPJ
    {
        public List<SubTherapeuticAreaList> SubTherapeuticList { get; set; }
    }

    public class GetIndicationListPJ
    {
        public List<IndicationList> IndicationList { get; set; }
    }

    public class GetProductListPJ
    {
        public List<ProductList> ProductList { get; set; }
    }

    public class GetCountryListPJ
    {
        public List<CountryList> CountryList { get; set; }
    }


    //public class GetMasterForAddUsersPJ
    //{
    //    public List<CountryMaster_List> CountryMasterList { get; set; }
    //    public List<RoleMaster_List> RoleMasterList { get; set; }
    //}

    //public class CountryMaster_List
    //{
    //    public int CountryId { get; set; }
    //    public string CountryName { get; set; }
    //}

    //public class RoleMaster_List
    //{
    //    public int RoleId { get; set; }
    //    public string RoleName { get; set; }
    //}

    

    public class GetYears
    {
        public List<YearList> GetYear()
        {
            List<YearList> sourceIds = new List<YearList>();
            sourceIds.Add(new YearList("2017", "2017", "09"));
            sourceIds.Add(new YearList("2018", "2018", "10"));
            return sourceIds;
        }
    }

    public class YearList
    {
        public string id { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public YearList(string _id, string _Year, string _Month)
        {
            id = _id;
            Year = _Year;
            Month = _Month;
        }
    }

    public class IndicationList
    {
        public int IndicationId { get; set; }
        public string IndicationName { get; set; }
    }

    public class TherapeuticAreaList
    {
        public int TherapeuticId { get; set; }
        public string TherapeuticName { get; set; }
    }

    public class BrandList
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
    }
    public class SubTherapeuticAreaList
    {
        public int SubTherapeuticId { get; set; }
        public string SubTherapeuticName { get; set; }
    }

    public class GetAreaFromArchitypeListPJ
    {
        public List<CountryList> CountryList { get; set; }
        public List<ArchetypeList> ArchetypeList { get; set; }
    }

    public class CountryList
    {
        public int CountryId { get; set; }
        public int? AreaId { get; set; }
        public string CountryName { get; set; }
        public string AreaName { get; set; }
    }

    public class ProductList
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? IndicationId { get; set; }
        public string IndicationName { get; set; }
    }

    public class IndicationList2
    {
        public int IndicationId { get; set; }
        public string IndicationName { get; set; }
        public List<ProductList> ProductList { get; set; }
    }

    public class AreaList
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
    }

    public class GetArchitypeListPJ
    {
        public List<AreaList> AreaList { get; set; }
        public List<AreaList2> AreaCountryList { get; set; }
    }

    public class AreaList2
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public List<CountryList> CountryList { get; set; }
    }

    public class SEARCH_CRITERIA
    {
        public int ID { get; set; }
        public Nullable<int> USER_ID { get; set; }
        public int THERAPEUTIC_ID { get; set; }
        public int SUB_THERAPEUTIC_ID { get; set; }
        public int INDICATION_ID { get; set; }
        public string ARCHETYPE_ID { get; set; }
        public string BRAND_ID { get; set; }
        public string AREA_ID { get; set; }
        public string COUNTRY_ID { get; set; }
        public int YEAR { get; set; }
        public string DESCRIPTIONS { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public Nullable<System.DateTime> MODIFIED_DATE { get; set; }
        public bool? FirstLoad { get; set; }
        public bool? ResetSession { get; set; }
    }


    public class PatientAdminMasterData
    {
        public List<TherapeuticAreaList> TherapeuticList { get; set; }
        public List<YearMasterData> YearList { get; set; }
        public List<ArchetypeList> ArchetypeList { get; set; }
        public List<AreaList> AreaList { get; set; }
    }

    public class StageList
    {
        public int StageMasterId { get; set; }
        public string StageName { get; set; }
    }

    public class TransactionMasterData
    {
        public int TransactionMasterId { get; set; }
        public string TransactionName { get; set; }
    }

    public class LocationMasterData
    {
        public int LocationMasterId { get; set; }
        public string LocationName { get; set; }
    }

    public class ClinicalMasterData
    {
        public int ClinicalMasterId { get; set; }
        public string ClinicalName { get; set; }
    }

    public class YearMasterData
    {
        public int YearId { get; set; }
        public int YearName { get; set; }
    }

    public class SubClinicalMasterData
    {
        public int SubClinicalMasterId { get; set; }
        public string SubClinicalName { get; set; }
    }
}
