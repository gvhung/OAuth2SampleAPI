using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientJourney.DataAccess.Data;
using PatientJourney.DataAccess;
using PatientJourney.BusinessModel;
using System.Data;


namespace PatientJourney.DataAccess
{
    public static class DefaultListDSForPJ
    {
        public static GetBrandAndAreaListPJ GetBrandAndAreaListDSForPJ()
        {
            GetBrandAndAreaListPJ listresponse = new GetBrandAndAreaListPJ();

            using (PJEntities entity = new PJEntities())
            {
                var therapeuticAreaData = from r in entity.Therapeutic_Area_Master select r;
                var areaData = from r in entity.Area_Master select r;
                var countryData = entity.Country_Master.ToList();
                var archetypeData = entity.Archetype_Master.ToList();

                listresponse.TherapeuticList = (from a in therapeuticAreaData
                                                where a.Is_Active == true
                                                select new TherapeuticAreaList()
                                                {
                                                    TherapeuticId = a.Therapeutic_Area_Master_Id,
                                                    TherapeuticName = a.Therapeutic_Area_Name
                                                }).OrderBy(x => x.TherapeuticName).ToList();


                listresponse.ArchetypeLists = (from a in archetypeData
                                               where a.Is_Active == true
                                               select new ArchetypeList()
                                               {
                                                   ArchetypeId = a.Archetype_Master_Id,
                                                   ArchetypeName = a.Archetype_Name
                                               }).OrderBy(x => x.ArchetypeName).ToList();

                listresponse.AreaList = (from a in areaData
                                         where a.Is_Active == true
                                         select new AreaList()
                                         {
                                             AreaId = a.Area_Master_Id,
                                             AreaName = a.Area_Name
                                         }).OrderBy(x => x.AreaName).ToList();

                List<AreaList2> areaListFinal = new List<AreaList2>();
                for (int i = 0; i < listresponse.AreaList.Count; i++)
                {
                    AreaList2 _area = new AreaList2();
                    _area.AreaId = Convert.ToInt32(listresponse.AreaList[i].AreaId);
                    _area.AreaName = areaData.Where(x => x.Area_Master_Id == _area.AreaId).FirstOrDefault().Area_Name.ToString();
                    var countrylist = (from h in countryData
                                       where h.Area_Master_Id == _area.AreaId
                                       where h.Is_Active == true
                                       select new CountryList()
                                       {
                                           CountryId = h.Country_Master_Id,
                                           CountryName = h.Country_Name,
                                           AreaId = h.Area_Master_Id
                                       }).OrderBy(x => x.CountryName).ToList();
                    _area.CountryList = countrylist;
                    areaListFinal.Add(_area);
                }
                listresponse.AreaList2 = areaListFinal;
            }
            return listresponse;
        }

        public static PatientAdminMasterData GetTherapeuticandArchetypeData(int[] countryid)
        {
            PatientAdminMasterData listresponse = new PatientAdminMasterData();
            try
            {
                using (PJEntities entity = new PJEntities())
                {

                    listresponse.TherapeuticList = (from a in entity.Therapeutic_Area_Master
                                                    where a.Is_Active == true
                                                    select new TherapeuticAreaList()
                                                    {
                                                        TherapeuticId = a.Therapeutic_Area_Master_Id,
                                                        TherapeuticName = a.Therapeutic_Area_Name
                                                    }).OrderBy(x => x.TherapeuticName).ToList();
                    //ArchetypeList archetype = null;
                    //listresponse.ArchetypeList = new List<ArchetypeList>();
                    //for (int i = 0; i < countryid.Length; i++)
                    //{
                    //    archetype = new ArchetypeList();
                    //    int cuurentcountry = countryid[i];
                    //    archetype = (from arche in entity.Archetype_Master
                    //                 join area in entity.Area_Master
                    //                 on arche.Archetype_Master_Id equals area.Archetype_Master_Id
                    //                 join country in entity.Country_Master
                    //                 on area.Archetype_Master_Id equals country.Archetype_Master_Id
                    //                 where country.Country_Master_Id == cuurentcountry
                    //                 select new ArchetypeList()
                    //                 {
                    //                     ArchetypeId = arche.Archetype_Master_Id,
                    //                     ArchetypeName = arche.Archetype_Name
                    //                 }).FirstOrDefault();
                    //    if (archetype != null)
                    //    {
                    //        listresponse.ArchetypeList.Add(archetype);
                    //    }
                    //}
                    //listresponse.ArchetypeList.OrderBy(x => x.ArchetypeName);

                    AreaList areaList = null;
                    listresponse.AreaList = new List<AreaList>();
                    for (int i = 0; i < countryid.Length; i++)
                    {
                        areaList = new AreaList();
                        int cuurentcountry = countryid[i];
                        areaList = (from areaMaster in entity.Area_Master
                                    join country in entity.Country_Master
                                    on areaMaster.Area_Master_Id equals country.Area_Master_Id
                                    where country.Country_Master_Id == cuurentcountry
                                    select new AreaList()
                                     {
                                         AreaId = areaMaster.Area_Master_Id,
                                         AreaName = areaMaster.Area_Name
                                     }).FirstOrDefault();
                        if (areaList != null)
                        {
                            if (!listresponse.AreaList.Exists(x => x.AreaId == areaList.AreaId))
                            {
                                listresponse.AreaList.Add(areaList);
                            }
                        }
                    }
                    listresponse.AreaList = listresponse.AreaList.OrderBy(x => x.AreaName).ToList();

                    listresponse.YearList = (from a in entity.Year_Master
                                             where a.Active == true
                                             select new YearMasterData()
                                             {
                                                 YearId = a.Year_Master_Id,
                                                 YearName = a.Year_Name
                                             }).OrderBy(x => x.YearName).ToList();
                }
            }
            catch (Exception)
            {
            }
            return listresponse;
        }

        public static GetSubTherapeuticAreaListPJ GetSubTherapeuticListDSForPJ(int TherapeuticId)
        {
            GetSubTherapeuticAreaListPJ listresponse = new GetSubTherapeuticAreaListPJ();

            using (PJEntities entity = new PJEntities())
            {
                var subTherapeuticData = from r in entity.SubTherapeutic_Area_Master select r;

                listresponse.SubTherapeuticList = (from a in subTherapeuticData
                                                   where a.Therapeutic_Area_Master_Id == TherapeuticId
                                                   where a.Is_Active == true
                                                   select new SubTherapeuticAreaList()
                                                   {
                                                       SubTherapeuticId = a.SubTherapeutic_Area_Master_Id,
                                                       SubTherapeuticName = a.SubTherapeutic_Area_Name
                                                   }).OrderBy(x => x.SubTherapeuticName).ToList();
            }
            return listresponse;
        }

        public static GetIndicationListPJ GetIndicationListDSForPJ(int SubTherapeuticId, int TherapeuticId)
        {
            GetIndicationListPJ listresponse = new GetIndicationListPJ();

            using (PJEntities entity = new PJEntities())
            {
                var indicationData = from r in entity.Indication_Master select r;

                listresponse.IndicationList = (from a in indicationData
                                               where a.SubTherapeutic_Area_Master_Id == SubTherapeuticId
                                               where a.Therapeutic_Area_Master_Id == TherapeuticId
                                               where a.Is_Active == true
                                               select new IndicationList()
                                               {
                                                   IndicationId = a.Indication_Master_Id,
                                                   IndicationName = a.Indication_Name
                                               }).OrderBy(x => x.IndicationName).ToList();
            }
            return listresponse;
        }

        public static List<IndicationList2> GetProductListDSForPJ(int IndicationId, int SubTherapeuticId, int TherapeuticId)
        {
            List<IndicationList2> listresponse = new List<IndicationList2>();

            using (PJEntities entity = new PJEntities())
            {
                var productData = from r in entity.Brand_Master select r;
                var indicationData = from r in entity.Indication_Master select r;

                IndicationList2 _indication = new IndicationList2();
                _indication.IndicationId = Convert.ToInt32(IndicationId);
                _indication.IndicationName = indicationData.Where(x => x.Indication_Master_Id == _indication.IndicationId).FirstOrDefault().Indication_Name.ToString();
                var productlist = (from h in productData
                                   where h.Indication_Master_Id == _indication.IndicationId
                                   where h.Therapeutic_Area_Master_Id == TherapeuticId
                                   where h.SubTherapeutic_Area_Master_Id == SubTherapeuticId
                                   where h.Is_Active == true
                                   select new ProductList()
                                   {
                                       ProductId = h.Brand_Master_Id,
                                       ProductName = h.Brand_Name,
                                       IndicationId = h.Indication_Master_Id
                                   }).OrderBy(x => x.ProductName).ToList();
                _indication.ProductList = productlist;
                listresponse.Add(_indication);
            }
            return listresponse;
        }

        public static List<AreaList2> GetCountryListDSForPJ(List<String> areaID)
        {
            List<AreaList2> lstresponse = new List<AreaList2>();

            using (PJEntities entity = new PJEntities())
            {
                var countryData = entity.Country_Master.ToList();
                var areaData = entity.Area_Master.ToList();

                for (int i = 0; i < areaID.Count; i++)
                {
                    AreaList2 _area = new AreaList2();
                    _area.AreaId = Convert.ToInt32(areaID[i]);
                    _area.AreaName = areaData.Where(x => x.Area_Master_Id == _area.AreaId).FirstOrDefault().Area_Name.ToString();
                    var countrylist = (from h in countryData
                                       where h.Area_Master_Id == _area.AreaId
                                       where h.Is_Active == true
                                       select new CountryList()
                                       {
                                           CountryId = h.Country_Master_Id,
                                           CountryName = h.Country_Name,
                                           AreaId = h.Area_Master_Id
                                       }).OrderBy(x => x.CountryName).ToList();
                    _area.CountryList = countrylist;
                    lstresponse.Add(_area);
                }
            }
            return lstresponse;
        }

        public static List<CountryList> GetCountryData(int areaId, int[] countryid)
        {
            List<CountryList> listresponse = new List<CountryList>();
            try
            {
                CountryList country = null;
                using (PJEntities entity = new PJEntities())
                {
                    for (int i = 0; i < countryid.Length; i++)
                    {
                        country = new CountryList();
                        int cuurentcountry = countryid[i];
                        country = (from areaMaster in entity.Area_Master
                                   join countryMaster in entity.Country_Master
                                   on areaMaster.Area_Master_Id equals countryMaster.Area_Master_Id
                                   where areaMaster.Area_Master_Id == areaId && countryMaster.Country_Master_Id == cuurentcountry
                                   select new CountryList()
                                   {
                                       CountryId = countryMaster.Country_Master_Id,
                                       CountryName = countryMaster.Country_Name
                                   }).FirstOrDefault();
                        if (country != null)
                        {
                            listresponse.Add(country);
                        }
                    }
                }
                listresponse = listresponse.OrderBy(x => x.CountryName).ToList();
            }
            catch (Exception)
            {
            }
            return listresponse;
        }

        public static GetArchitypeListPJ GetAreaandCountryDataPJ(List<String> architypeID)
        {
            GetArchitypeListPJ lstresponse = new GetArchitypeListPJ();

            using (PJEntities entity = new PJEntities())
            {
                var countryData = entity.Country_Master.ToList();
                var areaData = entity.Area_Master.ToList();

                for (int i = 0; i < architypeID.Count; i++)
                {
                    int archetypeID = Convert.ToInt32(architypeID[i]);
                    AreaList _area = new AreaList();
                    lstresponse.AreaList = (from a in areaData
                                            where a.Archetype_Master_Id == archetypeID
                                            where a.Is_Active == true
                                            select new AreaList()
                                            {
                                                AreaId = a.Area_Master_Id,
                                                AreaName = a.Area_Name
                                            }).OrderBy(x => x.AreaName).ToList();
                }

                List<AreaList2> areaListFinal = new List<AreaList2>();
                for (int j = 0; j < lstresponse.AreaList.Count; j++)
                {
                    int archetypeID = Convert.ToInt32(architypeID[j]);

                    for (int i = 0; i < lstresponse.AreaList.Count; i++)
                    {
                        AreaList2 _area = new AreaList2();
                        _area.AreaId = Convert.ToInt32(lstresponse.AreaList[i].AreaId);
                        _area.AreaName = areaData.Where(x => x.Area_Master_Id == _area.AreaId).FirstOrDefault().Area_Name.ToString();
                        var countrylist = (from h in countryData
                                           where h.Area_Master_Id == _area.AreaId
                                           where h.Archetype_Master_Id == archetypeID
                                           where h.Is_Active == true
                                           select new CountryList()
                                           {
                                               CountryId = h.Country_Master_Id,
                                               CountryName = h.Country_Name,
                                               AreaId = h.Area_Master_Id,
                                               AreaName = _area.AreaName
                                           }).OrderBy(x => x.CountryName).ToList();
                        _area.CountryList = countrylist;
                        areaListFinal.Add(_area);
                    }
                }
                lstresponse.AreaCountryList = areaListFinal;
            }

            return lstresponse;
        }

        public static GetAreaFromArchitypeListPJ GetAreaFromCountryListDSForPJ(List<String> lstCountryId)
        {
            GetAreaFromArchitypeListPJ lstresponse = new GetAreaFromArchitypeListPJ();

            using (PJEntities entity = new PJEntities())
            {
                var countryData = entity.Country_Master.ToList();
                var areaData = entity.Area_Master.ToList();
                var achetypeData = entity.Archetype_Master.ToList();

                for (int i = 0; i < lstCountryId.Count; i++)
                {
                    CountryList _area = new CountryList();
                    _area.CountryId = Convert.ToInt32(lstCountryId[i]);
                    _area.AreaId = countryData.Where(x => x.Country_Master_Id == _area.CountryId).Select(x => x.Area_Master_Id).FirstOrDefault();

                    lstresponse.CountryList = (from a in areaData
                                               where a.Area_Master_Id == _area.AreaId
                                               where a.Is_Active == true
                                               select new CountryList()
                                               {
                                                   AreaId = _area.AreaId,
                                               }).OrderBy(x => x.AreaId).Distinct().ToList();
                }

                for (int i = 0; i < lstresponse.CountryList.Count; i++)
                {
                    int areaID = Convert.ToInt32(lstresponse.CountryList[i].AreaId);
                    ArchetypeList _archetype = new ArchetypeList();
                    _archetype.ArchetypeId = areaData.Where(x => x.Area_Master_Id == areaID).Select(x => x.Archetype_Master_Id).FirstOrDefault();

                    lstresponse.ArchetypeList = (from a in achetypeData
                                                 where a.Archetype_Master_Id == _archetype.ArchetypeId
                                                 where a.Is_Active == true
                                                 select new ArchetypeList()
                                                 {
                                                     ArchetypeId = _archetype.ArchetypeId,
                                                 }).OrderBy(x => x.ArchetypeId).Distinct().ToList();
                }
            }
            return lstresponse;
        }

        public static SEARCH_CRITERIA GetSearchCriteria(string UserName)
        {
            SEARCH_CRITERIA result = new SEARCH_CRITERIA();
            using (PJEntities entity = new PJEntities())
            {
                var userData = from r in entity.Users select r;
                var searchData = from r in entity.Favourite_Search select r;
                int userId = userData.Where(x => x.User_511 == UserName.ToUpper()).Select(x => x.User_Id).FirstOrDefault();
                var searchResults = searchData.Where(x => x.User_Id == userId).FirstOrDefault();
                int searchId = searchResults.Favourite_Search_Id;

                List<int?> searchResultsBrand = entity.Favourite_Search_Brand.Where(x => x.Favourite_Search_Id == searchId).Select(x => x.Brand_Master_Id).ToList();
                List<int?> searchResultsArea = entity.Favourite_Search_Area.Where(x => x.Favourite_Search_Id == searchId).Select(x => x.Area_Master_Id).ToList();
                List<int?> searchResultsCountry = entity.Favourite_Search_Country.Where(x => x.Favourite_Search_Id == searchId).Select(x => x.Country_Master_Id).ToList();
                List<int?> searchResultsArchetype = entity.Favourite_Search_Archetype.Where(x => x.Favourite_Search_Id == searchId).Select(x => x.Archetype_Master_Id).ToList();



                result.THERAPEUTIC_ID = (int)searchResults.Therapeutic_Area_Master_Id;
                result.SUB_THERAPEUTIC_ID = (int)searchResults.SubTherapeutic_Area_Master_Id;
                result.INDICATION_ID = (int)searchResults.Indication_Master_Id;
                result.ARCHETYPE_ID = string.Join<int?>(",", searchResultsArchetype);
                result.BRAND_ID = string.Join<int?>(",", searchResultsBrand);
                result.AREA_ID = string.Join<int?>(",", searchResultsArea);
                result.COUNTRY_ID = string.Join<int?>(",", searchResultsCountry);
                result.YEAR = (int)searchResults.Year;

                return result;
            }
        }

        public static String GetTherapeuticName(int TherapeuticId)
        {
            String therapeuticName = null;
            using (PJEntities entity = new PJEntities())
            {
                therapeuticName = entity.Therapeutic_Area_Master.Where(x => x.Therapeutic_Area_Master_Id == TherapeuticId).Select(x => x.Therapeutic_Area_Name).FirstOrDefault().ToString();
            }
            return therapeuticName;
        }

        public static String GetSubTherapeuticName(int SubTherapeuticId)
        {
            String SubtherapeuticName = null;
            using (PJEntities entity = new PJEntities())
            {
                SubtherapeuticName = entity.SubTherapeutic_Area_Master.Where(x => x.SubTherapeutic_Area_Master_Id == SubTherapeuticId).Select(x => x.SubTherapeutic_Area_Name).FirstOrDefault().ToString();
            }
            return SubtherapeuticName;
        }

        public static String GetIndicationName(int IndicationId)
        {
            String IndicationName = null;
            using (PJEntities entity = new PJEntities())
            {
                IndicationName = entity.Indication_Master.Where(x => x.Indication_Master_Id == IndicationId).Select(x => x.Indication_Name).FirstOrDefault().ToString();
            }
            return IndicationName;
        }

        public static String SaveSearchCriteria(string TherapeuticId, string SubTherapeuticId, string IndicationId, List<string> lstArchetypeId, List<string> lstCountryId, List<string> lstProductId, List<string> lstAreaId, string Year, string UserName)
        {
            Favourite_Search _searchCriteria;
            Favourite_Search_Brand result1;
            Favourite_Search_Area result2;
            Favourite_Search_Country result3;
            Favourite_Search_Archetype result4;
            int productID, countryID, areaID, archeTypeID = 0;

            int therapeuticID = Convert.ToInt32(TherapeuticId);
            int subTherapeuticID = Convert.ToInt32(SubTherapeuticId);
            int indicationID = Convert.ToInt32(IndicationId);
            int year = Convert.ToInt32(Year);


            using (PJEntities entity = new PJEntities())
            {
                var userData = from r in entity.Users select r;
                var searchData = from r in entity.Favourite_Search select r;
                var searchDataBrand = from r in entity.Favourite_Search_Brand select r;
                var searchDataArea = from r in entity.Favourite_Search_Area select r;
                var searchDataCountry = from r in entity.Favourite_Search_Country select r;
                var searchDataArchetype = from r in entity.Favourite_Search_Archetype select r;
                int userId = userData.Where(x => x.User_511 == UserName.ToUpper()).Select(x => x.User_Id).FirstOrDefault();
                bool doesExistAlready = searchData.Any(o => o.User_Id == userId);

                if (doesExistAlready == true)
                {
                    var key = searchData.Where(o => o.User_Id == userId).FirstOrDefault().Favourite_Search_Id;

                    var brandData = searchDataBrand.Where(x => x.Favourite_Search_Id == key).ToList();
                    var areaData = searchDataArea.Where(x => x.Favourite_Search_Id == key).ToList();
                    var countryData = searchDataCountry.Where(x => x.Favourite_Search_Id == key).ToList();
                    var archetypeData = searchDataArchetype.Where(x => x.Favourite_Search_Id == key).ToList();
                    var favData = searchData.Where(x => x.Favourite_Search_Id == key).FirstOrDefault();

                    for (int i = 0; i < brandData.Count(); i++)
                    {
                        entity.Favourite_Search_Brand.Remove(brandData[i]);
                    }
                    for (int i = 0; i < areaData.Count(); i++)
                    {
                        entity.Favourite_Search_Area.Remove(areaData[i]);
                    }
                    for (int i = 0; i < countryData.Count(); i++)
                    {
                        entity.Favourite_Search_Country.Remove(countryData[i]);
                    }
                    for (int i = 0; i < archetypeData.Count(); i++)
                    {
                        entity.Favourite_Search_Archetype.Remove(archetypeData[i]);
                    }
                    entity.Favourite_Search.Remove(favData);
                    entity.SaveChanges();
                }

                _searchCriteria = new Favourite_Search();
                _searchCriteria.User_Id = userId;
                _searchCriteria.Is_Active = true;
                _searchCriteria.Therapeutic_Area_Master_Id = therapeuticID;
                _searchCriteria.SubTherapeutic_Area_Master_Id = subTherapeuticID;
                _searchCriteria.Indication_Master_Id = indicationID;
                _searchCriteria.Year = year;
                _searchCriteria.Created_By = UserName;
                _searchCriteria.Created_Date = DateTime.Now;
                _searchCriteria.Modified_By = UserName;
                _searchCriteria.Modified_Date = DateTime.Now;
                entity.Favourite_Search.Add(_searchCriteria);
                entity.SaveChanges();

                entity.Entry(_searchCriteria).GetDatabaseValues();

                int Fav_Search_Id = _searchCriteria.Favourite_Search_Id;

                for (int i = 0; i < lstProductId.Count; i++)
                {
                    result1 = new Favourite_Search_Brand();
                    productID = Convert.ToInt32(lstProductId[i]);

                    result1.User_Id = userId;
                    result1.Favourite_Search_Id = Fav_Search_Id;
                    result1.Brand_Master_Id = productID;
                    result1.Created_By = UserName;
                    result1.Created_Date = DateTime.Now;
                    result1.Modified_By = UserName;
                    result1.Modified_Date = DateTime.Now;
                    entity.Favourite_Search_Brand.Add(result1);
                }


                for (int i = 0; i < lstAreaId.Count; i++)
                {
                    result2 = new Favourite_Search_Area();
                    areaID = Convert.ToInt32(lstAreaId[i]);

                    result2.User_Id = userId;
                    result2.Favourite_Search_Id = Fav_Search_Id;
                    result2.Area_Master_Id = areaID;
                    result2.Created_By = UserName;
                    result2.Created_Date = DateTime.Now;
                    result2.Modified_By = UserName;
                    result2.Modified_Date = DateTime.Now;
                    entity.Favourite_Search_Area.Add(result2);
                }

                for (int i = 0; i < lstCountryId.Count; i++)
                {
                    result3 = new Favourite_Search_Country();
                    countryID = Convert.ToInt32(lstCountryId[i]);

                    result3.User_Id = userId;
                    result3.Favourite_Search_Id = Fav_Search_Id;
                    result3.Country_Master_Id = countryID;
                    result3.Created_By = UserName;
                    result3.Created_Date = DateTime.Now;
                    result3.Modified_By = UserName;
                    result3.Modified_Date = DateTime.Now;
                    entity.Favourite_Search_Country.Add(result3);
                }

                for (int i = 0; i < lstArchetypeId.Count; i++)
                {
                    result4 = new Favourite_Search_Archetype();
                    archeTypeID = Convert.ToInt32(lstArchetypeId[i]);

                    result4.User_Id = userId;
                    result4.Favourite_Search_Id = Fav_Search_Id;
                    result4.Archetype_Master_Id = archeTypeID;
                    result4.Created_By = UserName;
                    result4.Created_Date = DateTime.Now;
                    result4.Modified_By = UserName;
                    result4.Modified_Date = DateTime.Now;
                    entity.Favourite_Search_Archetype.Add(result4);
                }
                entity.SaveChanges();
            }
            return "TRUE";
        }

        //public static UserListResult GetUserList()
        //{
        //    UserListResult result = new UserListResult();

        //    using (PJEntities entity = new PJEntities())
        //    {
        //        var userMasterData = from r in entity.PJ_USERS select r;
        //        var roleMasterData = from r in entity.PJ_USER_ROLES_MASTER select r;

        //        result.UserList = (from j in userMasterData
        //                           select new UserList()
        //                           {
        //                               FullName = j.FIRST_NAME + j.LAST_NAME,
        //                               Email = j.EMAILID,
        //                               UPI = j.UPI,
        //                               AD_Logon = j.USER_511,
        //                               Role = "ADMIN"
        //                           }).ToList();
        //    }
        //    return result;
        //}

        //public static String AddNewUser(string FirstName, string MiddleName, string LastName, string EmailId, string UPI,
        //    string ADLogon, string userTypeID, string RoleID, List<string> lstCountryId)
        //{
        //    int countryID = 0;
        //    using (PJEntities entity = new PJEntities())
        //    {
        //        var userData = from r in entity.PJ_USERS select r;

        //        PJ_USERS _userDetails = new PJ_USERS();
        //        _userDetails.USER_511 = ADLogon;
        //        _userDetails.UPI = Convert.ToDecimal(UPI);
        //        _userDetails.FIRST_NAME = FirstName;
        //        _userDetails.MIDDLE_INITIAL = MiddleName;
        //        _userDetails.LAST_NAME = LastName;
        //        _userDetails.EMAILID = EmailId;
        //        _userDetails.IS_ACTIVE = true;
        //        _userDetails.CREATED_BY = "RANGARX6";
        //        _userDetails.CREATED_DATE = DateTime.Now;
        //        _userDetails.MODIFIED_BY = "RANGARX6";
        //        _userDetails.MODIFIED_DATE = DateTime.Now;
        //        entity.PJ_USERS.Add(_userDetails);
        //        entity.SaveChanges();
        //        entity.Entry(_userDetails).GetDatabaseValues();

        //        int User_Id = _userDetails.ID;

        //        for (int i = 0; i < lstCountryId.Count; i++)
        //        {
        //            countryID = Convert.ToInt32(lstCountryId[i]);

        //            PJ_USER_ADDON _userDetailsAdd = new PJ_USER_ADDON();
        //            _userDetailsAdd.COUNTRY_ID = countryID;
        //            _userDetailsAdd.ROLE_TYPE_ID = Convert.ToInt32(RoleID);
        //            _userDetailsAdd.USER_ID = User_Id;
        //            _userDetailsAdd.DESCRIPTIONS = null;
        //            _userDetailsAdd.CREATED_BY = "RANGARX6";
        //            _userDetailsAdd.CREATED_DATE = DateTime.Now;
        //            _userDetailsAdd.MODIFIED_BY = "RANGARX6";
        //            _userDetailsAdd.MODIFIED_DATE = DateTime.Now;
        //            entity.PJ_USER_ADDON.Add(_userDetailsAdd);
        //            entity.SaveChanges();
        //        }
        //    }
        //    return "TRUE";
        //}

        public static List<Stage_Master> GetStageData(int JourneyId, int StatusId)
        {
            List<Stage_Master> listresponse = new List<Stage_Master>();

            using (PJEntities entity = new PJEntities())
            {
                if (StatusId == 7 || StatusId == 3)
                {
                    var stagelist = entity.Patient_Journey_Stages_Temp.Where(x => x.Patient_Journey_Temp_Id == JourneyId).ToList();
                    var fulllist = entity.Stage_Master.ToList();
                    listresponse = fulllist.Where(p => !stagelist.Any(x => x.Stage_Master_Id == p.Stage_Master_Id)).ToList();
                    if (!listresponse.Any(x => x.Stage_Name.ToLower() == "other"))
                    {
                        var othertrans = entity.Stage_Master.Where(x => x.Stage_Name.ToLower() == "other").FirstOrDefault();
                        listresponse.Add(othertrans);
                    }
                    listresponse = listresponse.OrderBy(x => x.Stage_Name).ToList();
                }
                else
                {
                    var stagelist = entity.Patient_Journey_Stages.Where(x => x.Patient_Journey_Id == JourneyId).ToList();
                    var fulllist = entity.Stage_Master.ToList();
                    listresponse = fulllist.Where(p => !stagelist.Any(x => x.Stage_Master_Id == p.Stage_Master_Id)).ToList();
                    if (!listresponse.Any(x => x.Stage_Name.ToLower() == "other"))
                    {
                        var othertrans = entity.Stage_Master.Where(x => x.Stage_Name.ToLower() == "other").FirstOrDefault();
                        listresponse.Add(othertrans);
                    }
                    listresponse = listresponse.OrderBy(x => x.Stage_Name).ToList();
                }
            }
            return listresponse;
        }

        public static List<AreaList> GetAreaListDSForPJ(int ArchetypeId)
        {
            List<AreaList> listresponse = new List<AreaList>();

            using (PJEntities entity = new PJEntities())
            {
                var areaData = from r in entity.Area_Master select r;

                listresponse = (from a in areaData
                                where a.Archetype_Master_Id == ArchetypeId
                                where a.Is_Active == true
                                select new AreaList()
                                {
                                    AreaId = a.Area_Master_Id,
                                    AreaName = a.Area_Name
                                }).OrderBy(x => x.AreaName).ToList();
            }
            return listresponse;
        }

        public static List<Transaction_Master> GetTransactionMasterData(int StageId, int TransactionId, int IndicationId, int StatusId)
        {

            List<Transaction_Master> listresponse = new List<Transaction_Master>();
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    List<Transaction_Master> fulllist = new List<Transaction_Master>();
                    fulllist = entity.Transaction_Master.Where(x => x.Indication_Master_Id == IndicationId).ToList();
                    if (fulllist.Count <= 0)
                    {
                        fulllist = entity.Transaction_Master.GroupBy(x => x.Transaction_Name).Select(x => x.FirstOrDefault()).ToList();
                                  
                    }
                    if (StatusId == 7 || StatusId == 3)
                    {
                        var transactionlist = entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Stages_Temp_Id == StageId).ToList();
                        var currenttransaction = entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == TransactionId).FirstOrDefault();
                        listresponse = fulllist.Where(p => !transactionlist.Any(x => x.Transaction_Master_Id == p.Transaction_Master_Id)).ToList();
                        if (currenttransaction != null)
                        {
                            var currtrans = entity.Transaction_Master.Where(x => x.Transaction_Master_Id == currenttransaction.Transaction_Master_Id).FirstOrDefault();
                            listresponse.Add(currtrans);
                        }
                    }
                    else
                    {
                        var transactionlist = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Stages_Id == StageId).ToList();
                        var currenttransaction = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Transactions_Id == TransactionId).FirstOrDefault();
                        listresponse = fulllist.Where(p => !transactionlist.Any(x => x.Transaction_Master_Id == p.Transaction_Master_Id)).ToList();
                        if (currenttransaction != null)
                        {
                            var currtrans = entity.Transaction_Master.Where(x => x.Transaction_Master_Id == currenttransaction.Transaction_Master_Id).FirstOrDefault();
                            listresponse.Add(currtrans);
                        }
                    }
                    if (!listresponse.Any(x => x.Transaction_Name.ToLower() == "other"))
                    {
                        var othertrans = entity.Transaction_Master.Where(x => x.Transaction_Name.ToLower() == "other").FirstOrDefault();
                        listresponse.Add(othertrans);
                    }
                    listresponse = listresponse.OrderBy(x => x.Transaction_Name).ToList();


                }
            }
            catch (Exception)
            {
            }
            return listresponse;
        }

        public static List<Transaction_Location_Master> GetLocationMasterData()
        {
            List<Transaction_Location_Master> listresponse = new List<Transaction_Location_Master>();

            using (PJEntities entity = new PJEntities())
            {
                listresponse = entity.Transaction_Location_Master.OrderBy(x => x.Location_Name).ToList();
            }
            return listresponse;
        }


        public static List<CountryColourVJ> GetCountryNames(List<String> lstCountryId)
        {
            List<CountryColourVJ> lstresponse = new List<CountryColourVJ>();

            using (PJEntities entity = new PJEntities())
            {
                var countryData = entity.Country_Master.ToList();

                for (int i = 0; i < lstCountryId.Count; i++)
                {
                    int countryID = Convert.ToInt32(lstCountryId[i]);

                    CountryColourVJ countrylist = (from h in countryData
                                                   where h.Country_Master_Id == countryID
                                                   where h.Is_Active == true
                                                   select new CountryColourVJ()
                                                   {
                                                       CountryNames = h.Country_Name,
                                                       CountryIDs = h.Country_Master_Id
                                                   }).FirstOrDefault();

                    lstresponse.Add(countrylist);
                }

            }
            return lstresponse;
        }

        public static List<SelectedProduct> GetProductNames(List<String> lstProductId)
        {
            List<SelectedProduct> lstresponse = new List<SelectedProduct>();

            using (PJEntities entity = new PJEntities())
            {
                var brandData = entity.Brand_Master.ToList();

                for (int i = 0; i < lstProductId.Count; i++)
                {
                    int productID = Convert.ToInt32(lstProductId[i]);

                    SelectedProduct productlist = (from h in brandData
                                                   where h.Brand_Master_Id == productID
                                                   where h.Is_Active == true
                                                   select new SelectedProduct()
                                                   {
                                                       ProductName = h.Brand_Name,
                                                       ProductId = h.Brand_Master_Id
                                                   }).FirstOrDefault();

                    lstresponse.Add(productlist);
                }

            }
            return lstresponse;
        }

        public static List<AreaList> GetAreaName(List<String> lstAreaId)
        {
            List<AreaList> lstresponse = new List<AreaList>();

            using (PJEntities entity = new PJEntities())
            {
                var areaData = entity.Area_Master.ToList();

                for (int i = 0; i < lstAreaId.Count; i++)
                {
                    int areaID = Convert.ToInt32(lstAreaId[i]);

                    AreaList arealist = (from h in areaData
                                         where h.Area_Master_Id == areaID
                                         select new AreaList()
                                            {
                                                AreaName = h.Area_Name
                                            }).FirstOrDefault();

                    lstresponse.Add(arealist);
                }

            }
            return lstresponse;
        }

        public static List<ArchetypeList> GetArchetypeName(List<String> lstArchetypeId)
        {
            List<ArchetypeList> lstresponse = new List<ArchetypeList>();

            using (PJEntities entity = new PJEntities())
            {
                var archetypeData = entity.Archetype_Master.ToList();

                for (int i = 0; i < lstArchetypeId.Count; i++)
                {
                    int archetypeID = Convert.ToInt32(lstArchetypeId[i]);

                    var archetypelist = (from h in archetypeData
                                         where h.Archetype_Master_Id == archetypeID
                                         where h.Is_Active == true
                                         select new ArchetypeList()
                                         {
                                             ArchetypeName = h.Archetype_Name
                                         }).ToList();

                    lstresponse = archetypelist;
                }

            }
            return lstresponse;
        }

        public static List<Country_Master> GetAllCountries(int[] countryid)
        {
            using (PJEntities entity = new PJEntities())
            {
                Country_Master country = null;
                List<Country_Master> countryList = new List<Country_Master>();
                for (int i = 0; i < countryid.Length; i++)
                {
                    country = new Country_Master();
                    int id = countryid[i];
                    country = entity.Country_Master.Where(x => x.Country_Master_Id == id).FirstOrDefault();
                    countryList.Add(country);
                }
                countryList = countryList.OrderBy(x => x.Country_Name).ToList();
                return countryList;
            }
        }

        public static List<Clinical_Intervention_Master> GetClinicalMasterData()
        {
            using (PJEntities entity = new PJEntities())
            {
                var clinicalList = entity.Clinical_Intervention_Master.OrderBy(x => x.Title).ToList();
                return clinicalList;
            }
        }

        public static List<SubClinical_Intervention_Master> GetSubClinicalMasterData()
        {
            using (PJEntities entity = new PJEntities())
            {
                var subclinicalList = entity.SubClinical_Intervention_Master.OrderBy(x => x.Title).ToList();
                return subclinicalList;
            }
        }

        public static List<Year_Master> GetYearMasterData()
        {
            using (PJEntities entity = new PJEntities())
            {
                var yearList = entity.Year_Master.Where(x => x.Active == true).OrderBy(x => x.Year_Name).ToList();
                return yearList;
            }
        }
    }
}
