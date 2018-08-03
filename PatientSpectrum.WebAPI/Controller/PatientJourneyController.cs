using System.Collections.Generic;
using System.Web.Http;
using PatientJourney.BusinessModel.BuilderModels;
using PatientJourney.GlobalConstants;
using System.Net;
using System.Security.Claims;
using PatientSpectrum.WebAPI.Helper;
using PatientJourney.BusinessModel;
using PatientJourney.Business;
using System;
using Newtonsoft.Json;
using System.Linq;
using System.Text.RegularExpressions;
using PatientSpectrum.WebAPI.WebAPIModels;

namespace PatientSpectrum.WebAPI.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class PatientJourneyController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.ViewerRole)]
        public IHttpActionResult GetJourney()
        {
            //throw new NullReferenceException("SAMPLE");

            if (User.Identity.IsAuthenticated)
            {

                var claimsDictionary = (User as ClaimsPrincipal).Claims.ToClaimsDictionary();
                var countryId = claimsDictionary["usercountry"] ?? string.Empty;

                var lstPatientJourneys = new List<PatientJourneyModel>
                    {
                        new PatientJourneyModel()
                        {
                            PatientJourneyId = 1,
                            TherapeuticArea = "Admin",
                            SubTherapeuticArea = "Last"
                        }
                    };

                return Ok(lstPatientJourneys);
            }

            return Unauthorized();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole)]
        public IHttpActionResult GetJourneyAdmin()
        {

            if (User.Identity.IsAuthenticated)
            {
                var lstPatientJourneys = new List<PatientJourneyModel>
                    {
                        new PatientJourneyModel()
                        {
                            PatientJourneyId = 1,
                            TherapeuticArea = "Non Admin",
                            SubTherapeuticArea = "Last"
                        }
                    };

                return Ok(lstPatientJourneys);
            }

            return Unauthorized();
        }


        #region declaration

        private bsVisualJourney _bsVisualJourney = new bsVisualJourney();
        private bsAuthentication _bsAuthenticationPJ = new bsAuthentication();

        private String UserName
        {
            get
            {

                String username = User.Identity.Name;
                username = (username.Split('\\').Length > 1) ? username.Split('\\')[1] : username;
                if (username == "")
                username = Environment.UserName.ToString();
                return username;
            }
        }

        #endregion

        #region InputValidation
        private InputValidationModel IntegerArrayValidation(string countryIds)
        {
            Regex reg = new Regex(@"^[0-9,]{0,}$");
            InputValidationModel validation = new InputValidationModel();
            if (countryIds != null)
            {  
                validation.Isvalid = reg.IsMatch(countryIds);
                if (validation.Isvalid)
                {
                    validation.IsEmpty = false;
                    validation.ErrorString = null;
                    return validation;
                }
                else
                {
                    validation.IsEmpty = false;
                    validation.ErrorString= " Is Invalid";
                }
            }
            else
            {
                validation.IsEmpty = true;
                validation.Isvalid = false;
                validation.ErrorString= " Cannot be empty";
            }
            return validation;
                 
        }

        private InputValidationModel IntegerValidation(string input)
        {
            Regex reg = new Regex(@"^[0-9]{0,}$");
            InputValidationModel validation = new InputValidationModel();
            if (input != null)
            {
                validation.Isvalid = reg.IsMatch(input);
                if (validation.Isvalid)
                {
                    validation.IsEmpty = false;
                    validation.ErrorString = null;
                    return validation;
                }
                else
                {
                    validation.IsEmpty = false;
                    validation.ErrorString = " Is Invalid.";
                }
            }
            else
            {
                validation.ErrorString = " Cannot be empty.";
                validation.IsEmpty = true;
                validation.Isvalid = false;
            }
            return validation;
        }


        #endregion

        #region AdminPage
        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult GetTherapeuticandArchetypeData(string countryIds)
        {
            if (User.Identity.IsAuthenticated)
            {
                InputValidationModel Input = IntegerArrayValidation(countryIds);
                if (Input.Isvalid)
                {
                    PatientAdminMasterData result = new PatientAdminMasterData();
                    result = DefaultListBSForPJ.GetTherapeuticandArchetypeData(countryIds);
                    return Ok(result);
                }
                else
                {
                    string ErrorMsg = WebAPIConstants.Country + Input.ErrorString;
                    return BadRequest(ErrorMsg);          
                }
            }
            return Unauthorized();
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.ViewerRole)]
        [HttpGet]
        public IHttpActionResult GetTherapeuticDataPJ(int TherapeuticId)
        {
            GetSubTherapeuticAreaListPJ resultPJ = new GetSubTherapeuticAreaListPJ();

            resultPJ = DefaultListBSForPJ.GetSubTherapeuticListBSForPJ(TherapeuticId);

            return Ok(resultPJ);
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.ViewerRole)]
        [HttpGet]
        public IHttpActionResult GetIndicationDataPJ(int SubTherapeuticId, int TherapeuticId)
        {
            GetIndicationListPJ resultPJ = new GetIndicationListPJ();

            resultPJ = DefaultListBSForPJ.GetIndicationListBSForPJ(SubTherapeuticId, TherapeuticId);

            return Ok(resultPJ);
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.ViewerRole)]
        [HttpGet]
        public IHttpActionResult GetProductDataPJ(int IndicationId, int SubTherapeuticId, int TherapeuticId)
        {
            var resultPJ = DefaultListBSForPJ.GetProductListBSForPJ(IndicationId, SubTherapeuticId, TherapeuticId);
            return Ok(resultPJ);
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.ViewerRole)]
        [HttpGet]
        public IHttpActionResult GetCountryData(String AreaId, string countryIds)
        {
            InputValidationModel AreaIdValidation = IntegerValidation(AreaId);
            InputValidationModel countryIdsValidation = IntegerArrayValidation(countryIds);
            if (AreaIdValidation.Isvalid && countryIdsValidation.Isvalid)
            {
                var result = DefaultListBSForPJ.GetCountryData(AreaId, countryIds);
                return Json(result);
            }
            else
            {
                string ErrorMsg = string.Empty;

                if (!AreaIdValidation.Isvalid)
                {
                    ErrorMsg =WebAPIConstants.AreaID + AreaIdValidation.ErrorString;
                }
                if (!countryIdsValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + " " + WebAPIConstants.Country + countryIdsValidation.ErrorString;
                }
                return BadRequest(ErrorMsg);
            }
                
          }


        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)] 
        [HttpGet]
        public IHttpActionResult SearchJourney(string CountryId, string BrandId, string Year, string RoleIds)
        {
            InputValidationModel YearValidation = IntegerValidation(Year);
            InputValidationModel BrandIdValidation = IntegerValidation(BrandId);
            InputValidationModel countryIdsValidation = IntegerValidation(CountryId);

            InputValidationModel RoleIdsValidation = IntegerArrayValidation(RoleIds);
            if (YearValidation.Isvalid && BrandIdValidation.Isvalid && countryIdsValidation.Isvalid && RoleIdsValidation.Isvalid)
            {
                List<JourneySearchResult> result = new List<JourneySearchResult>();
                result = bsPatientAdministration.GetSearchResults(CountryId, BrandId, Year, RoleIds, UserName);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if(!YearValidation.Isvalid)
                {
                    ErrorMsg = WebAPIConstants.Year + YearValidation.ErrorString; 
                }
                if (!BrandIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + " " + WebAPIConstants.BrandID + BrandIdValidation.ErrorString;
                }
                if (!countryIdsValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + " " + WebAPIConstants.Country + countryIdsValidation.ErrorString;
                }
                if (!RoleIdsValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + " " + WebAPIConstants.RoleID + RoleIdsValidation.ErrorString;
                }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult GetCloneJourneyList(string BrandId)
        {
            InputValidationModel BrandIdValidation = IntegerValidation(BrandId);
            if (BrandIdValidation.Isvalid)
            {
                var result = bsPatientAdministration.GetCloneJourneyList(BrandId);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                ErrorMsg = ErrorMsg + " " + WebAPIConstants.BrandID + BrandIdValidation.ErrorString;
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult GetVersionDetails(string JourneyId)
        {
            InputValidationModel JourneyIdValidation = IntegerValidation(JourneyId);
            if (JourneyIdValidation.Isvalid)
            {
                var result = bsPatientAdministration.GetVersionDetails(JourneyId);
            return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                ErrorMsg = ErrorMsg + " " + WebAPIConstants.JourneyID + JourneyIdValidation.ErrorString;
                return BadRequest(ErrorMsg);
            }
        }


        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpPost]
        public IHttpActionResult EditJourney(PatientJourneyModel journey)
        {
            if (journey.PatientJourneyId > 0 && journey.StatusID > 0)
            {
                if (journey.StatusID == 3 || journey.StatusID == 7)
                {
                    journey.TempJourneyId = bsPatientAdministration.GetTempJourneyDetails(journey.PatientJourneyId);
                }
                else
                {
                    journey.TempJourneyId = 0;
                }
                return Ok(journey);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (journey.PatientJourneyId < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.PatientJourneyId; }
                if (journey.StatusID < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.StatusID; }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpPost]
        public IHttpActionResult UpdateJourneyStage(JourneyStage journeyStage)
        {
            if (journeyStage.StageTitle != null && journeyStage.StageTitle != string.Empty && journeyStage.StageMasterId >= 0 && journeyStage.PatientJourneyId >= 0 && journeyStage.PopulationStatistics >= 0
                && journeyStage.TimeStatistics >= 0 && journeyStage.TimeScale >= 0 && journeyStage.PopulationScale >= 0  && journeyStage.StatusID >= 0)
            {
                if (journeyStage.StatusID == 3 || journeyStage.StatusID == 7)
                {
                    var result = bsPatientAdministration.UpdateJourneyStageToTemp(journeyStage, journeyStage.StatusID, UserName);
                    return Ok(result);
                }
                else
                {
                    var result = bsPatientAdministration.UpdateJourneyStage(journeyStage, journeyStage.StatusID, UserName);
                    return Ok(result);
                }
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (journeyStage.StageTitle == null || journeyStage.StageTitle == string.Empty)
                { ErrorMsg = WebAPIConstants.StageTitle; }
                if (journeyStage.StageMasterId < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.StageMasterId; }
                if (journeyStage.PatientJourneyId < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.PatientJourneyId; }
                if (journeyStage.PopulationStatistics < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.PopulationStatistics; }
                if (journeyStage.TimeStatistics < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.TimeStatistics; }
                if (journeyStage.TimeScale < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.TimeScale; }
                if (journeyStage.PopulationScale < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.PopulationScale; }


                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult GetJourneyStage(string JourneyId, int StatusID)
        {
            InputValidationModel JourneyIdValidation = IntegerValidation(JourneyId);
            if (JourneyIdValidation.Isvalid)
            {
                if (StatusID == 3 || StatusID == 7)
                {
                    var result = bsPatientAdministration.GetJourneyStageFromTemp(JourneyId);
                    return Ok(result);
                }
                else
                {
                    var result = bsPatientAdministration.GetJourneyStage(JourneyId);
                    return Ok(result);
                }
            }
            else
            {
                string ErrorMsg = string.Empty;
                ErrorMsg = ErrorMsg + " " + WebAPIConstants.JourneyID + JourneyIdValidation.ErrorString;
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult RemoveJourneyStage(string JourneyId, string StageId,int StatusID)
        {
            InputValidationModel JourneyIdValidation = IntegerValidation(JourneyId);
            InputValidationModel StageIdValidation = IntegerValidation(StageId);
            if (JourneyIdValidation.Isvalid && StageIdValidation.Isvalid)
            {
                if (StatusID == 3 || StatusID == 7)
                {
                    var result = bsPatientAdministration.RemoveJourneyStageToTemp(JourneyId, StageId, StatusID, UserName);
                    return Ok(result);
                }
                else
                {
                    var result = bsPatientAdministration.RemoveJourneyStage(JourneyId, StageId, StatusID, UserName);
                    return Ok(result);
                }
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (!JourneyIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + " " + WebAPIConstants.JourneyID + JourneyIdValidation.ErrorString;
                }
                if (!StageIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + " " + WebAPIConstants.StageID + StageIdValidation.ErrorString;
                }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpPost]
        public IHttpActionResult AddTransaction(Transaction transaction)
        {
            if (transaction.TransactionMasterId> 0 && transaction.TransactionTitle != null &&  transaction.TransactionTitle != string.Empty &&  transaction.LocationId > 0 && 
                transaction.LocationName !=null && transaction.LocationName != string.Empty && 
                transaction.PatientJourneyId > 0 && transaction.PatientStageId > 0 && transaction.StatusID > 0)
            {
                if (transaction.StatusID == 3 || transaction.StatusID == 7)
                {
                    var result = bsPatientAdministration.AddTransactionToTemp(transaction, transaction.StatusID, UserName);
                    return Ok(result);
                }
                else
                {
                    var result = bsPatientAdministration.AddTransaction(transaction, transaction.StatusID, UserName);
                    return Ok(result);
                }
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (transaction.TransactionMasterId <=0)
                { ErrorMsg = WebAPIConstants.TransactionMasterId; }
                if (transaction.TransactionTitle == null || transaction.TransactionTitle == string.Empty)
                { ErrorMsg = ErrorMsg + WebAPIConstants.TransactionTitle; }
                if (transaction.LocationId <= 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.LocationId; }
                if (transaction.LocationName == null || transaction.TransactionTitle == string.Empty)
                { ErrorMsg = ErrorMsg + WebAPIConstants.LocationName; }
                if (transaction.PatientJourneyId <= 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.PatientJourneyId; }
                if (transaction.PatientStageId <= 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.PatientStageId; }
                if (transaction.StatusID <= 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.StatusID; }
  
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpPost]
        public IHttpActionResult UpdateTransaction(Transaction transaction)
        {
            if (transaction.TransactionMasterId > 0 && transaction.TransactionTitle != null && transaction.TransactionTitle != string.Empty && transaction.LocationId > 0 && 
                transaction.LocationName != null && transaction.LocationName != string.Empty && transaction.PatientJourneyId > 0 && transaction.PatientJourneyTransactionId > 0 && 
                transaction.PatientStageId > 0  && transaction.StatusID > 0)
            {
                if (transaction.StatusID == 3 || transaction.StatusID == 7)
                {
                    var result = bsPatientAdministration.UpdateTransactionToTemp(transaction, transaction.StatusID, UserName);
                    return Ok(result);
                }
                else
                {
                    var result = bsPatientAdministration.UpdateTransaction(transaction, transaction.StatusID, UserName);
                    return Ok(result);
                }
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (transaction.TransactionMasterId <= 0)
                { ErrorMsg = WebAPIConstants.TransactionMasterId; }
                if (transaction.TransactionTitle == null || transaction.TransactionTitle == string.Empty)
                { ErrorMsg = ErrorMsg + WebAPIConstants.TransactionTitle; }
                if (transaction.LocationId <= 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.LocationId; }
                if (transaction.LocationName == null || transaction.TransactionTitle == string.Empty)
                { ErrorMsg = ErrorMsg + WebAPIConstants.LocationName; }
                if (transaction.PatientJourneyId <= 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.PatientJourneyId; }
                if (transaction.PatientStageId <= 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.PatientStageId; }
                if (transaction.StatusID <= 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.StatusID; }
                if (transaction.PatientJourneyTransactionId <= 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.PatientJourneyTransactionId; }

                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult RemoveTransactions(string TransactionIds, string StageId, string JourneyId, int PatientJourneyId, int StatusID)
        {
            InputValidationModel StageIdValidation = IntegerValidation(StageId);
            InputValidationModel JourneyIdValidation = IntegerValidation(JourneyId);
            if (TransactionIds != null && TransactionIds != string.Empty && StageIdValidation.Isvalid && JourneyIdValidation.Isvalid && PatientJourneyId > 0 && StatusID > 0)
            {
                string[] arrTransactionId = JsonConvert.DeserializeObject<string[]>(TransactionIds);
                if (StatusID == 3 || StatusID == 7)
                {
                    var result = bsPatientAdministration.RemoveTransactionsToTemp(arrTransactionId, StageId, Convert.ToInt32(JourneyId), StatusID, UserName);
                    return Ok(result);
                }
                else
                {
                    var result = bsPatientAdministration.RemoveTransactions(arrTransactionId, StageId, PatientJourneyId, StatusID, UserName);
                    return Ok(result);
                }
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (!StageIdValidation.Isvalid )
                { ErrorMsg = WebAPIConstants.Stage_ID; }
                if (!JourneyIdValidation.Isvalid)
                { ErrorMsg = ErrorMsg + WebAPIConstants.JourneyId; }
                if (PatientJourneyId <= 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.PatientJourneyId; }
                if (StatusID <= 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.StatusID; }
                if (TransactionIds == null || TransactionIds == string.Empty)
                { ErrorMsg = ErrorMsg + WebAPIConstants.Transaction_Id; }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult ReorderTransaction(string TransactionOrders, string StageId, string JourneyId, int PatientJourneyId, int StatusID)
        {
            var StageIdValidation = IntegerValidation(StageId);
            if (StageIdValidation.Isvalid && TransactionOrders != null && TransactionOrders != string.Empty && PatientJourneyId > 0 && StatusID > 0)
            {
                string[] arrTransactionId = JsonConvert.DeserializeObject<string[]>(TransactionOrders);
                if (StatusID == 3 || StatusID == 7)
                {
                    var result = bsPatientAdministration.ReorderTransactionToTemp(arrTransactionId, StageId, PatientJourneyId, StatusID, UserName);
                    return Ok(result);
                }
                else
                {
                    var result = bsPatientAdministration.ReorderTransaction(arrTransactionId, StageId, PatientJourneyId, StatusID, UserName);
                    return Ok(result);
                }
            }
            else
            {
                string ErrorMsg = string.Empty;
                if(!StageIdValidation.Isvalid)
                {
                    ErrorMsg = WebAPIConstants.StageID + StageIdValidation.ErrorString;
                }
                if(TransactionOrders == null || TransactionOrders == string.Empty)
                { ErrorMsg =ErrorMsg+ WebAPIConstants.TransactionOrders; }
                if (PatientJourneyId <= 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.PatientJourneyId; }
                if (StatusID <= 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.StatusID; }
                return BadRequest(ErrorMsg);

            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult RemoveJourney(string JourneyId)
        {
            InputValidationModel JourneyIdValidation = IntegerValidation(JourneyId);
            if (JourneyIdValidation.Isvalid)
            {
                var result = bsPatientAdministration.RemoveJourney(JourneyId);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                ErrorMsg = ErrorMsg + " " + WebAPIConstants.JourneyID + JourneyIdValidation.ErrorString;
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole)]
        [HttpGet]
        public IHttpActionResult GetAllUsers(bool isActiveUserincluded, bool isInActiveUserincluded)
        {
            var result = bsUserAdministration.GetAllUsersForGrid(isActiveUserincluded, isInActiveUserincluded);
            return Ok(result);
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole)]
        [HttpGet]
        public IHttpActionResult GetSearchResults(String SearchText, bool isActiveUserincluded, bool isInActiveUserincluded)
        {
            var result = bsUserAdministration.GetSearchResultsForGrid(SearchText, isActiveUserincluded, isInActiveUserincluded);
            return Ok(result);
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole)]
        [HttpGet]
        public IHttpActionResult GetMastersForAddUser()
        {
            var result = bsUserAdministration.GetMastersForAddUser();
            return Ok(result);
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole)]
        [HttpGet]
        public IHttpActionResult GetUserDetailsFromADLogon(string User_511, string UPI)
        {
            if (User_511 != null)
            {
                var result = bsUserAdministration.GetUserInfo(User_511.ToUpper(), UPI);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                ErrorMsg = WebAPIConstants.User511ID + WebAPIConstants.NullString;
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole)]
        [HttpPost]
        public IHttpActionResult InsertNewUser(UserModel Userdetails)
        {
            //var RoleIdsValidation = IntegerArrayValidation(Userdetails.RoleIds);
            //if (Userdetails.User511 != null && Userdetails.User511 != string.Empty && RoleIdsValidation.Isvalid)
            //{
                var result = bsUserAdministration.InsertNewUser(Userdetails);
                return Ok(result);
            //}
            //else
            //{

            //}
        }


        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole)]
        [HttpPost]
        public IHttpActionResult UpdateExistingUser(UserModel Userdetails)
        {
            var claimsDictionary = (User as ClaimsPrincipal).Claims.ToClaimsDictionary();
            var User511 = claimsDictionary["user511id"].ToString() ?? string.Empty;
            

            var result = bsUserAdministration.UpdateExistingUser(Userdetails);
            if (PatientJourney.GlobalConstants.SessionHelper.LoggedinUser.UserID != null)
            {
                if (result.ToString() == PatientJourney.GlobalConstants.SessionHelper.LoggedinUser.UserID)
                {
                    var _PJUser = _bsAuthenticationPJ.GetUserDetailswithRoles(User511);
                    string roles = null;
                    for (int i = 0; i < _PJUser.UserRoles.Count; i++)
                    {
                        if (i == _PJUser.UserRoles.Count - 1)
                        {
                            roles = roles + _PJUser.UserRoles[i].RoleID;
                        }
                        else
                        {
                            roles = roles + _PJUser.UserRoles[i].RoleID + ",";
                        }
                    }
                    PatientJourney.GlobalConstants.SessionHelper.LoggedinUser = _PJUser.Userdetails;
                    PatientJourney.GlobalConstants.SessionHelper.LoggedinUser.RoleIds = roles;
                    List<Int32> ids = bsUserAdministration.GetCountryForUser(_PJUser.Userdetails.UserID.ToString());
                    if (ids != null)
                    {
                        PatientJourney.GlobalConstants.SessionHelper.LoggedinUser.CountryIds = string.Join(",", ids.Select(n => n.ToString()).ToArray());
                    }
                }
            }
            return Ok(result);
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole)]
        [HttpGet]
        public IHttpActionResult DeleteUser(string UserId, string RoleId)
        {
            InputValidationModel UserIdValidation = IntegerValidation(UserId);
            if (UserIdValidation.Isvalid)
            {
                var result = bsUserAdministration.DeleteUser(UserId, RoleId);
            return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                ErrorMsg = WebAPIConstants.UserID + UserIdValidation.ErrorString;
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole)]
        [HttpGet]
        public IHttpActionResult GetAuditHistory()
        {
            var result = bsAuditAdministration.GetAuditHistoryForGrid();
            return Ok(result);
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole)]
        [HttpGet]
        public IHttpActionResult GetSearchAuditResults(String SearchText)
        {
            var result = bsAuditAdministration.GetSearchResultsForGrid(SearchText);
            return Ok(result);
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpPost]
        public IHttpActionResult CloneFromOtherJourney(PatientJourneyModel journey)
        {
            if (journey.PatientJourneyId > 0 && journey.isCountryClone > 0 && journey.CountryID > 0)
            {
                var result = bsPatientAdministration.CloneFromOtherJourney(journey, UserName);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (journey.PatientJourneyId <= 0)
                {
                    ErrorMsg = WebAPIConstants.PatientJourneyId;
                }
                if (journey.isCountryClone <= 0)
                {
                    ErrorMsg = ErrorMsg+WebAPIConstants.isCountryClone;
                }
                if (journey.CountryID <= 0)
                {
                    ErrorMsg = ErrorMsg+WebAPIConstants.CountryID;
                }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpPost]
        public IHttpActionResult CloneTemplateJourney(PatientJourneyModel journey)
        {
            if (journey.BrandID > 0 && journey.Year > 0 && journey.CountryID > 0)
            {
                var result = bsPatientAdministration.CloneTemplateJourney(journey, UserName);
            return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (journey.BrandID <= 0)
                {
                    ErrorMsg = WebAPIConstants.Brand_ID;
                }
                if (journey.Year <= 0)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.year;
                }
                if (journey.CountryID <= 0)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.CountryID;
                }
                return BadRequest(ErrorMsg);
            }
        }



        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole)]
        [HttpGet]
        public IHttpActionResult GetRoles(string UserId)
        {
            InputValidationModel UserIdValidation = IntegerValidation(UserId);
            if (UserIdValidation.Isvalid)
            {
                var result = bsUserAdministration.GetRolesForUser(UserId);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                ErrorMsg = WebAPIConstants.UserID + UserIdValidation.ErrorString;
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole)]
        [HttpGet]
        public IHttpActionResult GetCountry(string UserId)
        {
            InputValidationModel UserIdValidation = IntegerValidation(UserId);
            if (UserIdValidation.Isvalid)
            {
                var result = bsUserAdministration.GetCountryForUser(UserId);
            return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                ErrorMsg = WebAPIConstants.UserID + UserIdValidation.ErrorString;
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult GetTransactions(string StageId,int StatusID)
        {
            var StageIdValidation = IntegerValidation(StageId);
            if (StageIdValidation.Isvalid)
            {
                if (StatusID == 3 || StatusID == 7)
                {
                    var result = bsPatientAdministration.GetTransactionsFromTemp(StageId);
                    return Ok(result);
                }
                else
                {
                    var result = bsPatientAdministration.GetTransactions(StageId);
                    return Ok(result);
                }
            }
            else
            {
                string ErrorMsg = WebAPIConstants.StageID + WebAPIConstants.NullString;
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult SubmitJourney(string JourneyId, string Comment,string Email)
        {
            InputValidationModel JourneyIdValidation = IntegerValidation(JourneyId);
            if (JourneyIdValidation.Isvalid)
            {
                var result = bsPatientAdministration.SubmitJourney(JourneyId, Comment, UserName, Email);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (!JourneyIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + " " + WebAPIConstants.JourneyID + JourneyIdValidation.ErrorString;
                }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult ApproveRejectSendbackJourney(string JourneyId, string Comment, int StatusId, string BrandId, string YearName, string CountryId, string Email)
        {
            var JourneyIdValidation = IntegerValidation(JourneyId);
            var BrandIdValidation = IntegerValidation(BrandId);
            var CountryIdValidation = IntegerValidation(CountryId);
            if (JourneyIdValidation.Isvalid && BrandIdValidation.Isvalid && CountryIdValidation.Isvalid)
            {
                var result = bsPatientAdministration.ApproveRejectSendbackJourney(JourneyId, Comment, StatusId, BrandId, YearName, CountryId, UserName, Email);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if(!JourneyIdValidation.Isvalid)
                {
                    ErrorMsg = WebAPIConstants.JourneyId;
                }
                if (!BrandIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg+WebAPIConstants.Brand_ID;
                }
                if (!CountryIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg+ WebAPIConstants.CountryID;
                }
                return BadRequest(ErrorMsg);
            }
        }

        #endregion

        #region Patient Journey

        //[CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.ViewerRole)]
        //[HttpGet]
        //public IHttpActionResult GetInputDataForHeader(GetInputDataModel inputdata)
        //{
        //    MasterDataInput result = new MasterDataInput();

        //    if (inputdata.TherapeuticId_Input != null && inputdata.SubTherapeuticId_Input != null && inputdata.IndicationId_Input != null
        //        && inputdata.ProductId_Input != null && inputdata.AreaId_Input != null && inputdata.CountryId_Input != null)
        //    {
        //        var TherapeuticId_InputValidation = IntegerValidation(inputdata.TherapeuticId_Input);
        //        var SubTherapeuticId_InputValidation = IntegerValidation(inputdata.SubTherapeuticId_Input);
        //        var IndicationId_InputValidation = IntegerValidation(inputdata.IndicationId_Input);
        //        var ProductId_InputValidation = IntegerValidation(inputdata.ProductId_Input);
        //        var AreaId_InputValidation = IntegerValidation(inputdata.AreaId_Input);
        //        var CountryId_InputValidation = IntegerValidation(inputdata.CountryId_Input);

        //        if (TherapeuticId_InputValidation.Isvalid && SubTherapeuticId_InputValidation.Isvalid && IndicationId_InputValidation.Isvalid && ProductId_InputValidation.Isvalid &&
        //            AreaId_InputValidation.Isvalid && CountryId_InputValidation.Isvalid)
        //        {
        //            result.TherapeuticId = Convert.ToInt32(inputdata.TherapeuticId_Input);
        //            result.SubTherapeuticId = Convert.ToInt32(inputdata.SubTherapeuticId_Input);
        //            result.IndicationId = Convert.ToInt32(inputdata.IndicationId_Input);
        //            result.AreaId = inputdata.AreaId_Input;
        //            result.CountryId = inputdata.CountryId_Input;
        //            result.ProductId = inputdata.ProductId_Input;
        //            result.Year = Convert.ToInt32(inputdata.Year_Input);
        //            result.SubTherapeuticName = inputdata.SubTherapeuticName ?? DefaultListBSForPJ.GetSubTherapeuticName(result.SubTherapeuticId);
        //            result.IndicationName = inputdata.IndicationName ?? DefaultListBSForPJ.GetIndicationName(result.IndicationId);
        //            result.TherapeuticName = inputdata.TherapeuticName ?? DefaultListBSForPJ.GetTherapeuticName(result.TherapeuticId);
        //            result.ProductNames = DefaultListBSForPJ.GetProductNames(inputdata.ProductId_Input);
        //            result.CountryNames = DefaultListBSForPJ.GetCountryNames(inputdata.CountryId_Input);
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            string ErrorMsg = string.Empty;
        //            if (TherapeuticId_InputValidation.Isvalid)
        //            {
        //                ErrorMsg = WebAPIConstants.TherapeuticID;
        //            }
        //            if (SubTherapeuticId_InputValidation.Isvalid)
        //            {
        //                ErrorMsg = ErrorMsg + WebAPIConstants.SubTherapeuticID;
        //            }
        //            if (IndicationId_InputValidation.Isvalid)
        //            {
        //                ErrorMsg = ErrorMsg + WebAPIConstants.IndicationID;
        //            }
        //            if (ProductId_InputValidation.Isvalid)
        //            {
        //                ErrorMsg = ErrorMsg + WebAPIConstants.Product_ID;
        //            }
        //            if (AreaId_InputValidation.Isvalid)
        //            {
        //                ErrorMsg = ErrorMsg + WebAPIConstants.Area_ID;
        //            }
        //            if (CountryId_InputValidation.Isvalid)
        //            {
        //                ErrorMsg = ErrorMsg + WebAPIConstants.CountryID;
        //            }
        //            return BadRequest(ErrorMsg);
        //        }
        //    }

        //    return Ok(result);
        //}

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpPost]
        public IHttpActionResult AddJourney(PatientJourneyModel journey)
        {
            if (journey.TherapeuticArea!=null && journey.TherapeuticArea != string.Empty && journey.TherapeuticID>=0 && journey.SubTherapeuticArea!=null && journey.SubTherapeuticArea != string.Empty
                && journey.SubTherapeuticID>=0 && journey.Indication!=null && journey.Indication != string.Empty && journey.IndicationID>=0 && journey.Brand!=null && journey.Brand != string.Empty 
                && journey.BrandID>=0 && journey.Area!=null && journey.Area != string.Empty && journey.AreaID>=0 && journey.Country!=null && journey.Country != string.Empty && journey.CountryID>=0
                )
            { 
            var result = bsPatientAdministration.AddJourney(journey, UserName);
            return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (journey.TherapeuticArea == null || journey.TherapeuticArea == string.Empty)
                { ErrorMsg = WebAPIConstants.TherapeuticArea; }
                if (journey.TherapeuticID < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.TherapeuticID; }
                if (journey.SubTherapeuticArea == null || journey.SubTherapeuticArea == string.Empty)
                { ErrorMsg = ErrorMsg + WebAPIConstants.SubTherapeuticArea; }
                if (journey.SubTherapeuticID < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.SubTherapeuticID; }
                if (journey.Indication == null || journey.Indication == string.Empty)
                { ErrorMsg = ErrorMsg + WebAPIConstants.Indication; }
                if (journey.IndicationID < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.IndicationID; }
                if (journey.Brand == null || journey.Brand == string.Empty)
                { ErrorMsg = ErrorMsg + WebAPIConstants.Brand; }
                if (journey.BrandID < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.Brand_ID; }
                if (journey.Area == null || journey.Area == string.Empty)
                { ErrorMsg = ErrorMsg + WebAPIConstants.Area; }
                if (journey.AreaID < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.Area_ID; }
                if (journey.Country == null || journey.Country == string.Empty)
                { ErrorMsg = ErrorMsg + WebAPIConstants.Countrys; }
                if (journey.CountryID < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.CountryID; }

                return BadRequest(ErrorMsg);

            }
        }
        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult GetApprovedJourneyFromTemp(string CountryId, string BrandId, string Year,string RoleIds,string LoggedInUserCountryIDs)
        {
            var CountryIdValidation = IntegerArrayValidation(CountryId);
            var BrandIdValidation = IntegerArrayValidation(BrandId);
            var RoleIdsValidation = IntegerArrayValidation(RoleIds);
            var YearValidation = IntegerValidation(Year);
            if (CountryIdValidation.Isvalid && BrandIdValidation.Isvalid && RoleIdsValidation.Isvalid && YearValidation.Isvalid)
            {
                FullPatientJounrney result = new FullPatientJounrney();
                result = bsPatientJourney.GetApprovedJourneyFromTemp(CountryId, BrandId, Year);
                var roles = RoleIds;
                if (!roles.Contains('1') && !roles.Contains('2') && roles.Contains('3'))
                {
                    result.IsCurrentUserCountry = 0;
                }
                else
                {
                    List<String> listStrCountries = LoggedInUserCountryIDs.Split(',').ToList();
                    if (listStrCountries.Contains(CountryId))
                    {
                        result.IsCurrentUserCountry = 1;
                    }
                    else
                    {
                        result.IsCurrentUserCountry = 0;
                    }
                }
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if(!CountryIdValidation.Isvalid)
                {
                    ErrorMsg = WebAPIConstants.Country + CountryIdValidation.ErrorString;
                }
                if (!BrandIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg+ WebAPIConstants.BrandID + BrandIdValidation.ErrorString;
                }
                if (!RoleIdsValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg+ WebAPIConstants.RoleID + RoleIdsValidation.ErrorString;
                }
                if (!YearValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg+ WebAPIConstants.Year + YearValidation.ErrorString;
                }
                return BadRequest(ErrorMsg);

            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.ViewerRole)]
        [HttpGet]
        public IHttpActionResult GetApprovedJourney(string CountryId, string BrandId, string Year, string RoleIds, string LoggedInUserCountryIDs)
        {
            var CountryIdValidation = IntegerArrayValidation(CountryId);
            var BrandIdValidation = IntegerArrayValidation(BrandId);
            var RoleIdsValidation = IntegerArrayValidation(RoleIds);
            var YearValidation = IntegerValidation(Year);
            if (CountryIdValidation.Isvalid && BrandIdValidation.Isvalid && RoleIdsValidation.Isvalid && YearValidation.Isvalid)
            {
                FullPatientJounrney result = new FullPatientJounrney();
            result = bsPatientJourney.GetApprovedJourney(CountryId, BrandId, Year);        
            var roles = RoleIds;
            if (!roles.Contains('1') && !roles.Contains('2') && roles.Contains('3'))
            {
                result.IsCurrentUserCountry = 0;
            }
            else
            {
                List<String> listStrCountries = LoggedInUserCountryIDs.Split(',').ToList();
                if (listStrCountries.Contains(CountryId))
                {
                    result.IsCurrentUserCountry = 1;
                }
                else
                {
                    result.IsCurrentUserCountry = 0;
                }
            }
            return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (!CountryIdValidation.Isvalid)
                {
                    ErrorMsg = WebAPIConstants.Country + CountryIdValidation.ErrorString;
                }
                if (!BrandIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.BrandID + BrandIdValidation.ErrorString;
                }
                if (!RoleIdsValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.RoleID + RoleIdsValidation.ErrorString;
                }
                if (!YearValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.Year + YearValidation.ErrorString;
                }
                return BadRequest(ErrorMsg);

            }
        }

        
        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpPost]
        public IHttpActionResult AddJourneyStage(JourneyStage journeyStage)
        {
            if (journeyStage.StageTitle!=null && journeyStage.StageTitle != string.Empty && journeyStage.StageMasterId >= 0 && journeyStage.PatientJourneyId>=0 && journeyStage.PopulationStatistics>=0
                && journeyStage.TimeStatistics >= 0 && journeyStage.TimeScale >= 0 && journeyStage.PopulationScale >= 0  && journeyStage.StatusID >= 0)
            {
                if (journeyStage.StatusID == 3 || journeyStage.StatusID == 7)
                {
                    var result = bsPatientAdministration.AddJourneyStageToTemp(journeyStage, journeyStage.StatusID, UserName);
                    return Ok(result);
                }
                else
                {
                    var result = bsPatientAdministration.AddJourneyStage(journeyStage, journeyStage.StatusID, UserName);
                    return Ok(result);
                }
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (journeyStage.StageTitle == null || journeyStage.StageTitle == string.Empty)
                { ErrorMsg = WebAPIConstants.StageTitle; }
                if (journeyStage.StageMasterId< 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.StageMasterId; }
                if (journeyStage.PatientJourneyId < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.PatientJourneyId; }
                if (journeyStage.PopulationStatistics < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.PopulationStatistics; }
                if (journeyStage.TimeStatistics < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.TimeStatistics; }
                if (journeyStage.TimeScale < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.TimeScale; }
                if (journeyStage.PopulationScale < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.PopulationScale; }               
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpPost]
        public IHttpActionResult AddClinicalInterventionToTemp(ClinicalIntervention clinicalIntervention)
        {
            if (clinicalIntervention.ClinicalInterventionMasterId > 0 && clinicalIntervention.TransactionId > 0 && clinicalIntervention.PatientJourneyId > 0)
            {
                var result = bsPatientJourney.AddClinicalInterventionToTemp(clinicalIntervention, UserName, clinicalIntervention.PatientJourneyId);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (clinicalIntervention.ClinicalInterventionMasterId <= 0)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.ClinicalInterventionId;
                }
                if (clinicalIntervention.TransactionId <= 0)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.Transaction_Id;
                }
                //if (clinicalIntervention.SubClinicalId == null)
                //{
                //    ErrorMsg = ErrorMsg + WebAPIConstants.SubClinicalId;
                //}
                
                if (clinicalIntervention.PatientJourneyId <= 0)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.PatientJourneyId;
                }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpPost]
        public IHttpActionResult AddAssociatedCostToTemp(AssociatedCost associatedCost)
        {
            if (associatedCost.PatientJourneyId>0 && associatedCost.TransactionId > 0 && associatedCost.AssociatedCosts != null && associatedCost.AssociatedCosts != string.Empty)
            {
                var result = bsPatientJourney.AddAssociatedCostToTemp(associatedCost, UserName, associatedCost.PatientJourneyId);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (associatedCost.PatientJourneyId <= 0)
                {
                    ErrorMsg = WebAPIConstants.PatientJourneyId;
                }
                if (associatedCost.TransactionId <= 0)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.Transaction_Id;
                }
                if (associatedCost.AssociatedCosts == null || associatedCost.AssociatedCosts == string.Empty)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.AssociatedCosts;
                }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpPost]
        public IHttpActionResult AddDesiredOutcomeToTemp(DesiredOutcome desiredOutcome)
        {
            if (desiredOutcome.PatientJourneyId > 0 && desiredOutcome.TransactionId > 0 && desiredOutcome.DesiredOutcomes != null && desiredOutcome.DesiredOutcomes != string.Empty)
            {
                var result = bsPatientJourney.AddDesiredOutcomeToTemp(desiredOutcome, UserName, desiredOutcome.PatientJourneyId);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (desiredOutcome.PatientJourneyId <= 0)
                {
                    ErrorMsg = WebAPIConstants.PatientJourneyId;
                }
                if (desiredOutcome.TransactionId <= 0)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.Transaction_Id;
                }
                if (desiredOutcome.DesiredOutcomes == null || desiredOutcome.DesiredOutcomes == string.Empty)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.DesiredOutcomes;
                }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult GetTransactionMasterData(string StageId, string TransactionId, string IndicationId,int StatusID)
        {
            var StageIdValidation = IntegerValidation(StageId);
            var TransactionIdValidation = IntegerValidation(TransactionId);
            var IndicationIdValidation = IntegerValidation(IndicationId);
            if (StageIdValidation.Isvalid && TransactionIdValidation.Isvalid && IndicationIdValidation.Isvalid)
            {
                var result = DefaultListBSForPJ.GetTransactionMasterData(StageId, TransactionId, IndicationId,StatusID);
            return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (!StageIdValidation.Isvalid)
                {
                    ErrorMsg = WebAPIConstants.StageID + StageIdValidation.ErrorString;
                }
                if (!TransactionIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.TransactionId + TransactionIdValidation.ErrorString;
                }
                if (!IndicationIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.indication + IndicationIdValidation.ErrorString;
                }
              
                return BadRequest(ErrorMsg);

            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpPost]
        public IHttpActionResult UpdatePatientRatingToTemp(Transaction patientDetails)
        {
            if (patientDetails.PatientDescription != null && patientDetails.PatientDescription != string.Empty && patientDetails.PatientRating >= 0 &&
                patientDetails.PatientJourneyTransactionId > 0  && patientDetails.PatientJourneyId > 0)
            {
                var result = bsPatientJourney.UpdatePatientRatingToTemp(patientDetails, UserName, patientDetails.PatientJourneyId);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (patientDetails.PatientDescription ==null || patientDetails.PatientDescription== string.Empty)
                {
                    ErrorMsg = WebAPIConstants.PatientDescription;
                }
                if (patientDetails.PatientRating <0 )
                {
                    ErrorMsg = ErrorMsg+WebAPIConstants.PatientRating;
                }
                if (patientDetails.PatientJourneyTransactionId <= 0)
                {
                    ErrorMsg = ErrorMsg+WebAPIConstants.PatientJourneyTransactionId;
                }
                if (patientDetails.PatientJourneyId <=0)
                {
                    ErrorMsg = ErrorMsg+WebAPIConstants.PatientJourneyId;
                }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpPost]
        public IHttpActionResult UpdateHCPRatingToTemp(Transaction patientDetails)
        {
            if (patientDetails.HCPDescription != null && patientDetails.HCPDescription != string.Empty && patientDetails.HCPRating >= 0 &&
                patientDetails.PatientJourneyTransactionId > 0 && patientDetails.PatientJourneyId > 0)
            {
                var result = bsPatientJourney.UpdateHCPRatingToTemp(patientDetails, UserName, patientDetails.PatientJourneyId);
            return Ok(result);
        }
            else
            {
                string ErrorMsg = string.Empty;
                if (patientDetails.HCPDescription == null || patientDetails.HCPDescription == string.Empty)
                {
                    ErrorMsg = WebAPIConstants.HCPDescription;
                }
                if (patientDetails.HCPRating < 0 )
                {
                    ErrorMsg = ErrorMsg+WebAPIConstants.HCPRating;
                }
                if (patientDetails.PatientJourneyTransactionId <= 0)
                {
                    ErrorMsg = ErrorMsg+WebAPIConstants.PatientJourneyTransactionId;
                }
                if (patientDetails.PatientJourneyId <=0)
                {
                    ErrorMsg = ErrorMsg+WebAPIConstants.PatientJourneyId;
                }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpPost]
        public IHttpActionResult UpdatePayerRatingToTemp(Transaction patientDetails)
        {
            if (patientDetails.PayerDescription != null && patientDetails.PayerDescription != string.Empty && patientDetails.PayerRating >= 0 &&
                   patientDetails.PatientJourneyTransactionId > 0  && patientDetails.PatientJourneyId > 0)
            {
                var result = bsPatientJourney.UpdatePayerRatingToTemp(patientDetails, UserName, patientDetails.PatientJourneyId);
            return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (patientDetails.PayerDescription == null || patientDetails.PayerDescription == string.Empty)
                {
                    ErrorMsg = WebAPIConstants.PayerDescription;
                }
                if (patientDetails.PayerRating < 0)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.PayerRating;
                }
                if (patientDetails.PatientJourneyTransactionId <= 0)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.PatientJourneyTransactionId;
                }

                if (patientDetails.PatientJourneyId <= 0)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.PatientJourneyId;
                }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpPost]
        public IHttpActionResult UpdateClinicalInterventionToTemp(ClinicalIntervention clinicalIntervention)
        {
            if (clinicalIntervention.ClinicalInterventionId > 0 && clinicalIntervention.ClinicalInterventionMasterId > 0 && clinicalIntervention.PatientJourneyId > 0)
            {
                var result = bsPatientJourney.UpdateClinicalInterventionToTemp(clinicalIntervention, UserName, clinicalIntervention.PatientJourneyId);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (clinicalIntervention.ClinicalInterventionId <= 0)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.ClinicalInterventionId;
                }
                if (clinicalIntervention.ClinicalInterventionMasterId <= 0)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.ClinicalInterventionMasterId;
                }
                //if (clinicalIntervention.SubClinicalId == null)
                //{
                //    ErrorMsg = ErrorMsg + WebAPIConstants.SubClinicalId;
                //}
                
                if (clinicalIntervention.PatientJourneyId <= 0)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.PatientJourneyId;
                }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpPost]
        public IHttpActionResult UpdateAssociatedCostToTemp(AssociatedCost associatedCost)
        {
            if (associatedCost.PatientJourneyId > 0  && associatedCost.AssociatedCosts != null && associatedCost.AssociatedCosts != string.Empty && associatedCost.AssociatedCostId>0)
            {
                var result = bsPatientJourney.UpdateAssociatedCostToTemp(associatedCost, UserName, associatedCost.PatientJourneyId);
            return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (associatedCost.PatientJourneyId <= 0)
                {
                    ErrorMsg = WebAPIConstants.PatientJourneyId;
                }
               
                if (associatedCost.AssociatedCosts == null || associatedCost.AssociatedCosts == string.Empty)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.AssociatedCosts;
                }
                if (associatedCost.AssociatedCostId <= 0)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.AssociatedCostId;
                }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpPost]
        public IHttpActionResult UpdateDesiredOutcomeToTemp(DesiredOutcome desiredOutcome)
        {
            if (desiredOutcome.PatientJourneyId > 0 && desiredOutcome.DesiredOutcomeId > 0 && desiredOutcome.DesiredOutcomes != null && desiredOutcome.DesiredOutcomes != string.Empty)
            {
                var result = bsPatientJourney.UpdateDesiredOutcomeToTemp(desiredOutcome,UserName, desiredOutcome.PatientJourneyId);
            return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (desiredOutcome.PatientJourneyId <= 0)
                {
                    ErrorMsg = WebAPIConstants.PatientJourneyId;
                }
                if (desiredOutcome.DesiredOutcomeId <= 0)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.DesiredOutcomeId;
                }
                if (desiredOutcome.DesiredOutcomes == null || desiredOutcome.DesiredOutcomes == string.Empty)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.DesiredOutcomes;
                }
                return BadRequest(ErrorMsg);
            }
        }

        //[CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.ViewerRole)]
        //[HttpGet]
        //public IHttpActionResult GetInputData(GetInputDataModel inputdata)
        //{
        //    MasterDataInput result = new MasterDataInput();
        //    GetYears getYear = new GetYears();


        //    if (inputdata.TherapeuticId_Input != null && inputdata.SubTherapeuticId_Input != null && inputdata.IndicationId_Input != null
        //        && inputdata.ProductId_Input != null && inputdata.AreaId_Input != null && inputdata.CountryId_Input != null)
        //    {

        //        var TherapeuticId_InputValidation = IntegerValidation(inputdata.TherapeuticId_Input);
        //        var SubTherapeuticId_InputValidation = IntegerValidation(inputdata.SubTherapeuticId_Input);
        //        var IndicationId_InputValidation = IntegerValidation(inputdata.IndicationId_Input);
        //        var ProductId_InputValidation = IntegerValidation(inputdata.ProductId_Input);
        //        var AreaId_InputValidation = IntegerValidation(inputdata.AreaId_Input);
        //        var CountryId_InputValidation = IntegerValidation(inputdata.CountryId_Input);

        //        if (TherapeuticId_InputValidation.Isvalid && SubTherapeuticId_InputValidation.Isvalid && IndicationId_InputValidation.Isvalid && ProductId_InputValidation.Isvalid &&
        //            AreaId_InputValidation.Isvalid && CountryId_InputValidation.Isvalid)
        //        {
        //            result.TherapeuticId = Convert.ToInt32(inputdata.TherapeuticId_Input);
        //            result.SubTherapeuticId = Convert.ToInt32(inputdata.SubTherapeuticId_Input);
        //            result.IndicationId = Convert.ToInt32(inputdata.IndicationId_Input);
        //            result.ArchetypeId = inputdata.ArchetypeId_Input;
        //            result.AreaId = inputdata.AreaId_Input;
        //            result.CountryId = inputdata.CountryId_Input;
        //            result.ProductId = inputdata.ProductId_Input;
        //            result.Year = Convert.ToInt32(inputdata.Year_Input);
        //            result.SaveFavourite = inputdata.SaveFavourite;
        //            result.SubTherapeuticName = inputdata.SubTherapeuticName ?? DefaultListBSForPJ.GetSubTherapeuticName(result.SubTherapeuticId);
        //            result.IndicationName = inputdata.IndicationName ?? DefaultListBSForPJ.GetIndicationName(result.IndicationId);
        //            result.TherapeuticName = inputdata.TherapeuticName ?? DefaultListBSForPJ.GetTherapeuticName(result.TherapeuticId);

        //            result.TherapeuticList = DefaultListBSForPJ.GetBrandAndAreaListBSForPJ().TherapeuticList;
        //            result.SubTherapeuticList = DefaultListBSForPJ.GetSubTherapeuticListBSForPJ(result.TherapeuticId).SubTherapeuticList;
        //            result.IndicationList = DefaultListBSForPJ.GetIndicationListBSForPJ(result.SubTherapeuticId, result.TherapeuticId).IndicationList;
        //            result.ProductList = DefaultListBSForPJ.GetProductListBSForPJ(result.IndicationId, result.SubTherapeuticId, result.TherapeuticId);
        //            result.AreaList = DefaultListBSForPJ.GetBrandAndAreaListBSForPJ().AreaList;
        //            result.CountryList = DefaultListBSForPJ.GetCountryListBSForPJ(result.AreaId);
        //            result.ArchetypeList = DefaultListBSForPJ.GetBrandAndAreaListBSForPJ().ArchetypeLists;
        //            result.YearList = getYear.GetYear();

        //            //}
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            string ErrorMsg = string.Empty;
        //            if (TherapeuticId_InputValidation.Isvalid)
        //            {
        //                ErrorMsg = WebAPIConstants.TherapeuticID;
        //            }
        //            if (SubTherapeuticId_InputValidation.Isvalid)
        //            {
        //                ErrorMsg = ErrorMsg + WebAPIConstants.SubTherapeuticID;
        //            }
        //            if (IndicationId_InputValidation.Isvalid)
        //            {
        //                ErrorMsg = ErrorMsg + WebAPIConstants.IndicationID;
        //            }
        //            if (ProductId_InputValidation.Isvalid)
        //            {
        //                ErrorMsg = ErrorMsg + WebAPIConstants.Product_ID;
        //            }
        //            if (AreaId_InputValidation.Isvalid)
        //            {
        //                ErrorMsg = ErrorMsg + WebAPIConstants.Area_ID;
        //            }
        //            if (CountryId_InputValidation.Isvalid)
        //            {
        //                ErrorMsg = ErrorMsg + WebAPIConstants.CountryID;
        //            }
        //            return BadRequest(ErrorMsg);
        //        }
        //    }
        //    return Ok(result);
        //}


        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.ViewerRole)]
        [HttpGet]
        public IHttpActionResult GetSavedSearchData()
        {
            MasterDataInput result = new MasterDataInput();
            GetYears getYear = new GetYears();
            bool userSearchPresent = _bsAuthenticationPJ.GetSearchCriteriaForUser(UserName);

            if (userSearchPresent == true)
            {
                SEARCH_CRITERIA Result = DefaultListBSForPJ.GetSearchCriteria(UserName);
                result.TherapeuticId = Result.THERAPEUTIC_ID;
                result.SubTherapeuticId = Result.SUB_THERAPEUTIC_ID;
                result.IndicationId = Result.INDICATION_ID;
                result.ArchetypeId = Result.ARCHETYPE_ID;
                result.AreaId = Result.AREA_ID;
                result.CountryId = Result.COUNTRY_ID;
                result.ProductId = Result.BRAND_ID;
                result.Year = Result.YEAR;


                result.TherapeuticName = DefaultListBSForPJ.GetTherapeuticName(result.TherapeuticId);
                result.SubTherapeuticName = DefaultListBSForPJ.GetSubTherapeuticName(result.SubTherapeuticId);
                result.IndicationName = DefaultListBSForPJ.GetIndicationName(result.IndicationId);
                result.BrandName = DefaultListBSForPJ.GetBrandName(result.ProductId);
                result.ArchetypeName = DefaultListBSForPJ.GetArchetypeName(result.ArchetypeId);
                result.AreaName = DefaultListBSForPJ.GetAreaName(result.AreaId);
                result.CountryName = DefaultListBSForPJ.GetCountryName(result.CountryId);

            }
            return Ok(result);
        }


        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.ViewerRole)]
        [HttpGet]
        public IHttpActionResult GetvisualJourneyData(int TherapeuticId, int SubTherapeuticId, int IndicationId, string AreaId, string CountryId, string ProductId, int Year)
        {
            InputValidationModel AreaIdValidation = IntegerArrayValidation(AreaId);
            InputValidationModel CountryIdValidation = IntegerArrayValidation(CountryId);
            InputValidationModel ProductIdValidation = IntegerArrayValidation(ProductId);
            if (AreaIdValidation.Isvalid && CountryIdValidation.Isvalid && ProductIdValidation.Isvalid)
            {
                VJChartInput input = new VJChartInput();
                VJChartModel result = new VJChartModel();

                input.SubTherapeuticId = SubTherapeuticId;
                input.TherapeuticId = TherapeuticId;
                input.IndicationId = IndicationId;
                input.AreaId = AreaId;
                input.CountryId = CountryId;
                input.ProductId = ProductId;
                input.Year = Year;
                result = ChartListBSForPJ.GetVJChartListBS(input);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (!AreaIdValidation.Isvalid)
                {
                    ErrorMsg = WebAPIConstants.AreaID + AreaIdValidation.ErrorString;
                }
                if (!CountryIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.Country + CountryIdValidation.ErrorString;
                }
                if (!ProductIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.ProductID + ProductIdValidation.ErrorString;
                }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.ViewerRole)]
        [HttpGet]
        public IHttpActionResult SaveInputData(string TherapeuticId, string SubTherapeuticId, string IndicationId, string ArchetypeId, string AreaId, string CountryId, string ProductId,
           string Year, string SubTherapeuticName, string IndicationName, string TherapeuticName, bool SaveFavourite)
        {
            var TherapeuticId_InputValidation = IntegerValidation(TherapeuticId);
            var SubTherapeuticId_InputValidation = IntegerValidation(SubTherapeuticId);
            var IndicationId_InputValidation = IntegerValidation(IndicationId);
            var ProductId_InputValidation = IntegerArrayValidation(ProductId);
            var AreaId_InputValidation = IntegerArrayValidation(AreaId);
            var CountryId_InputValidation = IntegerArrayValidation(CountryId);
            var ArchetypeId_InputValidation = IntegerArrayValidation(ArchetypeId);
            var Year_InputValidation = IntegerValidation(Year);
            if (TherapeuticId_InputValidation.Isvalid && SubTherapeuticId_InputValidation.Isvalid && IndicationId_InputValidation.Isvalid && ProductId_InputValidation.Isvalid &&
                AreaId_InputValidation.Isvalid && CountryId_InputValidation.Isvalid && ArchetypeId_InputValidation.Isvalid && Year_InputValidation.Isvalid )
            {
              
                if (SaveFavourite == true)
                {
                    DefaultListBSForPJ.SaveSearchCriteria(TherapeuticId, SubTherapeuticId, IndicationId, ArchetypeId, AreaId, CountryId, ProductId, Year, UserName);
                }


                return Ok("TRUE");
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (TherapeuticId_InputValidation.Isvalid)
                { ErrorMsg = WebAPIConstants.TherapeuticID; }
                  if ( SubTherapeuticId_InputValidation.Isvalid )
                { ErrorMsg = ErrorMsg + WebAPIConstants.SubTherapeuticID; }
                  if ( IndicationId_InputValidation.Isvalid   )
                { ErrorMsg = ErrorMsg + WebAPIConstants.IndicationID; }
                  if ( ProductId_InputValidation.Isvalid    )
                { ErrorMsg = ErrorMsg + WebAPIConstants.Product_ID; }
                  if ( AreaId_InputValidation.Isvalid     )
                { ErrorMsg = ErrorMsg + WebAPIConstants.Area_ID; }
                  if ( CountryId_InputValidation.Isvalid   )
                { ErrorMsg = ErrorMsg + WebAPIConstants.CountryID; }
                  if ( ArchetypeId_InputValidation.Isvalid  )
                { ErrorMsg = ErrorMsg + WebAPIConstants.ArchetypeId; }
                  if (Year_InputValidation.Isvalid)
                { ErrorMsg = ErrorMsg + WebAPIConstants.Year; }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.ViewerRole)]
        [HttpGet]
        public IHttpActionResult GetBrandandAreaDataPJ(string UserNameForDisplay, GetBrandAndAreaListPJ MasterDataForPJ)
        {
            GetBrandAndAreaListPJ resultPJ = new GetBrandAndAreaListPJ();
            GetYears getYear = new GetYears();
            bool userSearchPresent = _bsAuthenticationPJ.GetSearchCriteriaForUser(UserName);
          //  SessionHelper.Search_Present = userSearchPresent;

            if (MasterDataForPJ == null)
            {
                resultPJ = DefaultListBSForPJ.GetBrandAndAreaListBSForPJ();
                resultPJ.YearList = getYear.GetYear();
                resultPJ.UserNameForDisplay =UserNameForDisplay;
               // SessionHelper.Reset_Session = false;

                if (resultPJ.UserNameForDisplay == null)
                {
                    var _PJUser = _bsAuthenticationPJ.GetUserNameForDisplay(UserName);
                    resultPJ.UserNameForDisplay = _PJUser.Userdetails.FirstName;
                     
                }
               // SessionHelper.MasterDataForPJ = resultPJ;
            }
            else
            {
                resultPJ = MasterDataForPJ;
                resultPJ.UserNameForDisplay = resultPJ.UserNameForDisplay;
            }

            resultPJ.YearList = getYear.GetYear();
            resultPJ.SearchPresent = userSearchPresent;
            return Ok(resultPJ);
        }

        [HttpGet]
        public IHttpActionResult GenerateTabularViewVJ(String BrandID, String CountryID, String Year)
        {
            InputValidationModel BrandIDValidation = IntegerValidation(BrandID);
            InputValidationModel CountryIdValidation = IntegerArrayValidation(CountryID);
            InputValidationModel YearValidation = IntegerValidation(Year);
            if (BrandIDValidation.Isvalid && CountryIdValidation.Isvalid && YearValidation.Isvalid)
            {
                var result = _bsVisualJourney.GetTabularviewForVJ(CountryID, BrandID, Year);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (!BrandIDValidation.Isvalid)
                {
                    ErrorMsg = WebAPIConstants.BrandID + BrandIDValidation.ErrorString;
                }
                if (!CountryIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.Country + CountryIdValidation.ErrorString;
                }
                if (!YearValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.Year + YearValidation.ErrorString;
                }
                return BadRequest(ErrorMsg);
            }
        }

        [HttpGet]
        public IHttpActionResult GenerateKeyLevers(String BrandID, String CountryID, String Year)
        {
            InputValidationModel BrandIDValidation = IntegerValidation(BrandID);
            InputValidationModel CountryIdValidation = IntegerArrayValidation(CountryID);
            InputValidationModel YearValidation = IntegerValidation(Year);
            if (BrandIDValidation.Isvalid && CountryIdValidation.Isvalid && YearValidation.Isvalid)
            {
                var result = _bsVisualJourney.GetKeyLeversforCountriesandBrands(CountryID, BrandID, Year);
            return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (!BrandIDValidation.Isvalid)
                {
                    ErrorMsg = WebAPIConstants.BrandID + BrandIDValidation.ErrorString;
                }
                if (!CountryIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.Country + CountryIdValidation.ErrorString;
                }
                if (!YearValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.Year + YearValidation.ErrorString;
                }
                return BadRequest(ErrorMsg);
            }
        }


        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.ViewerRole)]
        [HttpGet]
        public IHttpActionResult GetBarChartData(int TherapeuticId, int SubTherapeuticId, int IndicationId, string AreaId, string CountryId, string ProductId, int Year)
        {
            InputValidationModel AreaIdValidation = IntegerArrayValidation(AreaId);
            InputValidationModel CountryIdValidation = IntegerArrayValidation(CountryId);
            InputValidationModel ProductIdValidation = IntegerArrayValidation(ProductId);
            if (AreaIdValidation.Isvalid && CountryIdValidation.Isvalid && ProductIdValidation.Isvalid)
            {
                ChartInput input = new ChartInput();

            input.SubTherapeuticId = SubTherapeuticId;
            input.TherapeuticId = TherapeuticId;
            input.IndicationId = IndicationId;
            input.AreaId = AreaId;
            input.CountryId = CountryId;
            input.ProductId = ProductId;
            input.Year = Year;
            ChartModel result = ChartListBSForPJ.GetChartListBS(input);
            return Ok(result);  
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (!AreaIdValidation.Isvalid)
                {
                    ErrorMsg = WebAPIConstants.AreaID + AreaIdValidation.ErrorString;
                }
                if (!CountryIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.Country + CountryIdValidation.ErrorString;
                }
                if (!ProductIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.ProductID + ProductIdValidation.ErrorString;
                }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult GetAllStrategicMoment(string CountryId, string BrandId, string Year, string RoleIds, string LoggedInUserCountryIDs)
        {
            InputValidationModel BrandIdValidation = IntegerArrayValidation(BrandId);
            InputValidationModel CountryIdValidation = IntegerArrayValidation(CountryId);
            InputValidationModel RoleIdsValidation = IntegerArrayValidation(RoleIds);
            InputValidationModel YearValidation = IntegerValidation(Year);
            InputValidationModel LoggedInUserCountryIDsValidation = IntegerArrayValidation(LoggedInUserCountryIDs);
            if (BrandIdValidation.Isvalid && CountryIdValidation.Isvalid && RoleIdsValidation.Isvalid && YearValidation.Isvalid && LoggedInUserCountryIDsValidation.Isvalid)
            {
                StrategicMoment result = new StrategicMoment();
                result = bsStrategicMoment.GetAllStrategicMoment(CountryId, BrandId, Year);
                //PatientJourney.GlobalConstants.SessionHelper.StrategicMoment = result.Strategic_Moment;
                //if (PatientJourney.GlobalConstants.SessionHelper.LoggedinUser == null)
                //{
                //    AssignSession();
                //}
                var roles = RoleIds;
                if (!roles.Contains('1') && !roles.Contains('2') && roles.Contains('3'))
                {
                    result.IsCurrentUserCountry = 0;
                }
                else
                {
                    List<String> listStrCountries = LoggedInUserCountryIDs.Split(',').ToList();
                    if (listStrCountries.Contains(CountryId))
                    {
                        result.IsCurrentUserCountry = 1;
                    }
                    else
                    {
                        result.IsCurrentUserCountry = 0;
                    }
                }
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (!BrandIdValidation.Isvalid)
                {
                    ErrorMsg = WebAPIConstants.BrandID + BrandIdValidation.ErrorString;
                }
                if (!CountryIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg+ WebAPIConstants.Country + CountryIdValidation.ErrorString;
                }
                if (!RoleIdsValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.RoleID + RoleIdsValidation.ErrorString;
                }
                if (!YearValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.Year + YearValidation.ErrorString;
                }
                if (!LoggedInUserCountryIDsValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + "LoggedInUserCountryID" + LoggedInUserCountryIDsValidation.ErrorString;
                }
                return BadRequest(ErrorMsg);

            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult GetStages(string JourneyId)
        {
            InputValidationModel JourneyIdValidation = IntegerValidation(JourneyId);
            if (JourneyIdValidation.Isvalid)
            {

                NewStrategicMoment result = new NewStrategicMoment();
                result = bsStrategicMoment.GetStages(JourneyId);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                ErrorMsg = ErrorMsg + " " + WebAPIConstants.JourneyID + JourneyIdValidation.ErrorString;  
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult GetEndStage(string JourneyId, string StageId)
        {
            InputValidationModel JourneyIdValidation = IntegerValidation(JourneyId);
            InputValidationModel StageIdValidation = IntegerValidation(StageId);
            if (JourneyIdValidation.Isvalid && StageIdValidation.Isvalid)
            {

                NewStrategicMoment result = new NewStrategicMoment();
            result = bsStrategicMoment.GetEndStage(JourneyId, StageId);
            return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (!JourneyIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + " " + WebAPIConstants.JourneyID + JourneyIdValidation.ErrorString;
                }
                if (!StageIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + " " + WebAPIConstants.StageID + StageIdValidation.ErrorString;
                }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult GetTransactionsFromStage(string StageId)
        {
            InputValidationModel StageIdValidation = IntegerValidation(StageId);
            if (StageIdValidation.Isvalid)
            {
                var result = bsPatientAdministration.GetTransactions(StageId);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;              
                ErrorMsg = ErrorMsg + " " + WebAPIConstants.StageID + StageIdValidation.ErrorString;
                return BadRequest(ErrorMsg);
            }
        }


        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpPost]
        public IHttpActionResult UpdateStrategicMoment(StrategicMomentModel strategicMoment)
        {
            if (strategicMoment.Category != null && strategicMoment.Description != null && strategicMoment.Title != null && strategicMoment.Category != string.Empty &&
                strategicMoment.Description != string.Empty && strategicMoment.Title != string.Empty && strategicMoment.StartStageId >= 0 && strategicMoment.EndStageId >= 0
                && strategicMoment.StartTransactionId >= 0 && strategicMoment.EndTransactionId >= 0 && strategicMoment.JourneyId >= 0 && 
                strategicMoment.StrategicMomentId >= 0 && strategicMoment.StrategicMomentTempId >= 0 )
            {
                var result = bsStrategicMoment.UpdateStrategicMoment(strategicMoment, UserName);
            return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (strategicMoment.Category == null || strategicMoment.Category == string.Empty)
                { ErrorMsg = WebAPIConstants.Category; }
                if (strategicMoment.Description == null || strategicMoment.Description == string.Empty)
                { ErrorMsg = ErrorMsg + WebAPIConstants.Description; }
                if (strategicMoment.Title == null || strategicMoment.Title == string.Empty)
                { ErrorMsg = ErrorMsg + WebAPIConstants.Title; }
                if (strategicMoment.StartStageId < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.StartStageId; }
                if (strategicMoment.EndStageId < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.EndStageId; }
                if (strategicMoment.StartTransactionId < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.StartTransactionId; }
                if (strategicMoment.EndTransactionId < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.EndTransactionId; }
                if (strategicMoment.JourneyId < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.JourneyId; }
                if (strategicMoment.StrategicMomentId < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.StrategicMomentId; }
                if (strategicMoment.StrategicMomentTempId < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.StrategicMomentTempId; }
                return BadRequest(ErrorMsg);

            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult DeleteStrategicMoment(string MomentId, string TempMomentId)
        {
            InputValidationModel MomentIdValidation = IntegerValidation(MomentId);
            InputValidationModel TempMomentIdValidation = IntegerValidation(TempMomentId);
            if (MomentIdValidation.Isvalid && TempMomentIdValidation.Isvalid)
            {
                var result = bsStrategicMoment.DeleteStrategicMoment(MomentId, TempMomentId);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;

                if (!MomentIdValidation.Isvalid)
                {
                    ErrorMsg = WebAPIConstants.MomentID + MomentIdValidation.ErrorString;
                }
                if (!TempMomentIdValidation.Isvalid)
                {
                    ErrorMsg = WebAPIConstants.TempMomentID + TempMomentIdValidation.ErrorString;
                }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpPost]
        public IHttpActionResult AddStrategicMoment(StrategicMomentModel strategicMoment)
        {
            if (strategicMoment.Category != null && strategicMoment.Description != null && strategicMoment.Title != null && strategicMoment.Category != string.Empty &&
                strategicMoment.Description != string.Empty && strategicMoment.Title != string.Empty && strategicMoment.StartStageId >= 0 && strategicMoment.EndStageId>=0
                && strategicMoment.StartTransactionId >= 0 && strategicMoment.EndTransactionId >= 0 && strategicMoment.JourneyId >= 0)
            {
                var result = bsStrategicMoment.AddStrategicMoment(strategicMoment, UserName);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (strategicMoment.Category == null || strategicMoment.Category == string.Empty)
                { ErrorMsg = WebAPIConstants.Category; }
                if (strategicMoment.Description == null || strategicMoment.Description == string.Empty)
                { ErrorMsg = ErrorMsg+ WebAPIConstants.Description; }
                if (strategicMoment.Title == null || strategicMoment.Title == string.Empty)
                { ErrorMsg = ErrorMsg + WebAPIConstants.Title; }
                if (strategicMoment.StartStageId < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.StartStageId; }
                if (strategicMoment.EndStageId < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.EndStageId; }
                if (strategicMoment.StartTransactionId < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.StartTransactionId; }
                if (strategicMoment.EndTransactionId < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.EndTransactionId; }
                if (strategicMoment.JourneyId < 0)
                { ErrorMsg = ErrorMsg + WebAPIConstants.JourneyId; }
                return BadRequest(ErrorMsg);
                
            }
        }

        [HttpGet]
        public IHttpActionResult GetTopKeyLeversData(String TransactionName, int ProductId, string StageName, int JourneyId, int Year)
        {
            var result = _bsVisualJourney.GetTopCountriesForKeyLevers(TransactionName, ProductId, StageName, JourneyId, Year);
            return Ok(result);
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult ReorderStage(string StageOrders, string JourneyId,int StatusID)
        {
            
            var JourneyIdValidation = IntegerValidation(JourneyId);
            if (StageOrders != null && StageOrders != string.Empty && JourneyIdValidation.Isvalid  && StatusID>0)
            {
                string[] arrStageId = JsonConvert.DeserializeObject<string[]>(StageOrders);
                if (StatusID == 3 || StatusID == 7)
                {
                    var result = bsPatientAdministration.ReorderStageToTemp(arrStageId, JourneyId, StatusID, UserName);
                    return Ok(result);
                }
                else
                {
                    var result = bsPatientAdministration.ReorderStage(arrStageId, JourneyId, StatusID, UserName);
                    return Ok(result);
                }
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (StageOrders == null || StageOrders == string.Empty)
                { ErrorMsg = WebAPIConstants.StageOrders; }
                if(!JourneyIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.JourneyId;
                }
                if(StatusID<=0)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.StatusID ;
                }
                return BadRequest(ErrorMsg);
            }
        }

        [CustomAuthorizeAttribute(OAuthPJConstants.ViewerRole)]
        [HttpGet]
        public IHttpActionResult GetFullTransactionDetails(string StageId, string JourneyId)
        {
            var StageIdValidation = IntegerValidation(StageId);
            var JourneyIdValidation = IntegerValidation(JourneyId);
            if (StageIdValidation.Isvalid && JourneyIdValidation.Isvalid)
            {
                FullPatientTransaction result = new FullPatientTransaction();
            result = bsPatientJourney.GetFullTransactionDetails(StageId, JourneyId);
            return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (!StageIdValidation.Isvalid)
                {
                    ErrorMsg = WebAPIConstants.StageID + StageIdValidation.ErrorString;
                }
                if (!JourneyIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.JourneyID + JourneyIdValidation.ErrorString;
                }
                return BadRequest(ErrorMsg);
            }
        }


        [CustomAuthorizeAttribute(OAuthPJConstants.AdminRole + OAuthPJConstants.ScopeDelimiter + OAuthPJConstants.EditorRole)]
        [HttpGet]
        public IHttpActionResult GetFullTransactionDetailsFromTemp(string StageId, string JourneyId)
        {
            var StageIdValidation = IntegerValidation(StageId);
            var JourneyIdValidation = IntegerValidation(JourneyId);
            if (StageIdValidation.Isvalid && JourneyIdValidation.Isvalid)
            {
                FullPatientTransaction result = new FullPatientTransaction();
                result = bsPatientJourney.GetFullTransactionDetailsFromTemp(StageId, JourneyId);
                return Ok(result);
            }
            else
            {
                string ErrorMsg = string.Empty;
                if (!StageIdValidation.Isvalid)
                {
                    ErrorMsg = WebAPIConstants.StageID + StageIdValidation.ErrorString;
                }
                if (!JourneyIdValidation.Isvalid)
                {
                    ErrorMsg = ErrorMsg + WebAPIConstants.JourneyID + JourneyIdValidation.ErrorString;
                }
                return BadRequest(ErrorMsg);
            }
        }
        #endregion

    }
}