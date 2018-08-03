using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientJourney.BusinessModel;
using PatientJourney.DataAccess.Data;
using PatientJourney.DataAccess.DataAccess;
using PatientJourney.BusinessModel.BuilderModels;

namespace PatientJourney.Business
{
    public static class bsPdfJourney
    {
        public static EntirePatientJourney GetApprovedJourneyForPdf(string CountryId, string BrandId, string Year)
        {
            EntirePatientJourney lstJourney = new EntirePatientJourney();
            JourneyPdfModel FirstResult = new JourneyPdfModel();
            FullJourneyTransaction lstFullJourney = new FullJourneyTransaction();
            List<String> lstCountryId = new List<String>();
            lstCountryId = CountryId.Split(',').ToList();
            List<String> lstBrandId = new List<String>();
            lstBrandId = BrandId.Split(',').ToList();
            try
            {
                Patient_Journey result = new Patient_Journey();
                result = dbPatientJourney.GetApprovedJourney(Convert.ToInt32(lstCountryId[0]), Convert.ToInt32(lstBrandId[0]), Convert.ToInt32(Year));
                PJEntities entity = new PJEntities();
                if (result != null)
                {
                    List<JourneyStages> stages = bsPdfJourney.GetJourneyStageNames(result.Patient_Journey_Id.ToString());
                    lstJourney.Stages = stages;

                    lstJourney.StageCount = stages.Count();
                    lstJourney.IndicationName = result.Journey_Title.Substring(0, result.Journey_Title.IndexOf("-"));

                    List<StrategicMomentAll> strategicAll = bsPdfJourney.GetStrategicMoment(result.Patient_Journey_Id.ToString());
                    lstJourney.StrategicMoment = strategicAll;

                    if (stages != null)
                    {
                        FullJourneyTransaction Full_Transactions = bsPdfJourney.GetAllTransactions(result.Patient_Journey_Id.ToString(), stages);
                        lstFullJourney.Transactions = Full_Transactions.Transactions;
                        lstFullJourney.ClinicalInterventions = Full_Transactions.ClinicalInterventions;
                        lstFullJourney.AssociatedCosts = Full_Transactions.AssociatedCosts;
                        lstFullJourney.DesiredOutcomes = Full_Transactions.DesiredOutcomes;
                    }
                    lstJourney.FullJourneyTransaction = lstFullJourney;

                    FirstResult.PatientJourneyId = result.Patient_Journey_Id;
                    FirstResult.JourneyTitle = result.Journey_Title;
                    FirstResult.StagesCount = stages.Count;

                    lstJourney.Journey = FirstResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstJourney;
        }

        public static List<JourneyStages> GetJourneyStageNames(string JourneyId)
        {
            List<JourneyStages> listJourneyStage = new List<JourneyStages>();
            PJEntities entity = new PJEntities();
            var stages = dbPatientAdministration.GetJourneyStage(Convert.ToInt32(JourneyId));

            for (int i = 0; i < stages.Count; i++)
            {
                JourneyStages journeyStage = new JourneyStages();
                journeyStage.StageNumber = i + 1;
                journeyStage.PatientJourneyId = Convert.ToInt32(stages[i].Patient_Journey_Id);
                journeyStage.PatientStageId = stages[i].Patient_Journey_Stages_Id;
                journeyStage.StageTitle = " " + stages[i].Stage_Title.ToUpper();
                journeyStage.StageMasterId = Convert.ToInt32(stages[i].Stage_Master_Id);
                journeyStage.StageColor = (i % 2 == 1) ? "rgba(204, 204, 204, 0.48)" : "rgba(158, 158, 158, 0.48)";
                journeyStage.FeasibilityColor = "rgba(255, 235, 59, 0.58)";
                journeyStage.ViabilityColor = "rgba(255, 235, 59, 0.58)";
                journeyStage.Description = stages[i].Description;
                journeyStage.TransactionCount = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Id == journeyStage.PatientJourneyId && x.Patient_Journey_Stages_Id == journeyStage.PatientStageId).Count();
                //if (journeyStage.TransactionCount >= 4) { journeyStage.StageWidth = (journeyStage.TransactionCount * 120) + "px"; }
                //else if (journeyStage.TransactionCount == 1) { journeyStage.StageWidth = "170px"; }
                //else if (journeyStage.TransactionCount == 2) { journeyStage.StageWidth = "300px"; }
                //else if (journeyStage.TransactionCount == 3) { journeyStage.StageWidth = "420px"; }
                journeyStage.StageWidth = (journeyStage.TransactionCount * 120) + "px";
                //else { journeyStage.StageWidth = "175px"; }
                if (stages[i].Time_Statistics != 0)
                {
                    journeyStage.TimeStats = stages[i].Time_Statistics + " Months";
                }
                else
                {
                    journeyStage.TimeStats = "";
                }
                if (stages[i].Population_Statistics != 0)
                {
                    journeyStage.PopulationStats = stages[i].Population_Statistics + " Hundreds";
                }
                else
                {
                    journeyStage.PopulationStats = "";
                }
                listJourneyStage.Add(journeyStage);
            }
            return listJourneyStage;
        }

        public static FullJourneyTransaction GetAllTransactions(string JourneyId, List<JourneyStages> stages)
        {
            FullJourneyTransaction fullJourneyTransaction = new FullJourneyTransaction();

            List<Journey_ClinicalInterventions> listClinicalIntervention = new List<Journey_ClinicalInterventions>();
            Journey_ClinicalInterventions clinInter = null;

            List<Journey_DesiredOutcomes> listDesiredOutcomes = new List<Journey_DesiredOutcomes>();
            Journey_DesiredOutcomes desiredOutcomes = null;

            List<Journey_AssociatedCosts> listAssociatedCosts = new List<Journey_AssociatedCosts>();
            Journey_AssociatedCosts associatedCost = null;

            #region Get Transaction

            for (int i = 0; i < stages.Count; i++)
            {
                Journey_Transaction _journeyTransaction = new Journey_Transaction();
                _journeyTransaction = bsPdfJourney.GetTransactions(JourneyId, stages[i].PatientStageId.ToString(), i);
                _journeyTransaction.StageColor = (i % 2 == 1) ? "#fff" : "rgba(204, 204, 204, 0.48)";
                _journeyTransaction.StageColorForLocation = (i % 2 == 1) ? "rgba(204, 204, 204, 0.48)" : "rgba(158, 158, 158, 0.48)";
                fullJourneyTransaction.Transactions.Add(_journeyTransaction);
            }
            #endregion

            if (fullJourneyTransaction.Transactions != null)
            {
                #region Get Clinical Intervention

                for (int i = 0; i < fullJourneyTransaction.Transactions.Count; i++)
                {
                    clinInter = new Journey_ClinicalInterventions();
                    clinInter.StageColor = (i % 2 == 1) ? "rgb(255, 255, 255)" : "rgba(204, 204, 204, 0.48)";
                    clinInter.StageColorForLocation = (i % 2 == 1) ? "rgba(204, 204, 204, 0.48)" : "rgba(158, 158, 158, 0.48)";
                    List<Journey_ClinicalInterventions_Details> ClinicalInterventionsDetails = new List<Journey_ClinicalInterventions_Details>();

                    for (int j = 0; j < fullJourneyTransaction.Transactions[i].TransactionsDetails.Count; j++)
                    {
                        clinInter.TransactionId = Convert.ToInt32(fullJourneyTransaction.Transactions[i].TransactionsDetails[j].PatientJourneyTransactionId);

                        var resultClinInt = dbPatientJourney.GetClinicalIntervention(fullJourneyTransaction.Transactions[i].TransactionsDetails[j].PatientJourneyTransactionId);

                        if (resultClinInt != null)
                        {
                            Journey_ClinicalInterventions_Details Journey_ClinicalInterventions_Details = new Journey_ClinicalInterventions_Details();
                            Journey_ClinicalInterventions_Details.ClinicalInterventionId = resultClinInt.Patient_Journey_Trans_Clin_Interventions_Id;
                            Journey_ClinicalInterventions_Details.ClinicalInterventionMasterId = resultClinInt.Clinical_Intervention_Master_Id;
                            if (Journey_ClinicalInterventions_Details.ClinicalInterventionMasterId != null)
                            {
                                var clinicalInterventionMasterDetails = dbPatientAdministration.GetClinicalInterventionDetails(Journey_ClinicalInterventions_Details.ClinicalInterventionMasterId);
                                Journey_ClinicalInterventions_Details.ClinicalInterventionTitle = clinicalInterventionMasterDetails.Select(x => x.Title).FirstOrDefault().ToString();
                                if (clinicalInterventionMasterDetails.Select(x => x.Image_Master_Id).FirstOrDefault() != null)
                                {
                                    Journey_ClinicalInterventions_Details.ImageMasterId = (int)clinicalInterventionMasterDetails.Select(x => x.Image_Master_Id).FirstOrDefault();
                                    Journey_ClinicalInterventions_Details.ImagePath = dbPatientAdministration.GetImagePath(Journey_ClinicalInterventions_Details.ImageMasterId);
                                }
                            }
                            Journey_ClinicalInterventions_Details.Description = resultClinInt.Description;

                            ClinicalInterventionsDetails.Add(Journey_ClinicalInterventions_Details);
                        }
                        else
                        {
                            Journey_ClinicalInterventions_Details Journey_ClinicalInterventions_Details = new Journey_ClinicalInterventions_Details();
                            ClinicalInterventionsDetails.Add(Journey_ClinicalInterventions_Details);
                        }

                        clinInter.ClinicalInterventionsDetails = ClinicalInterventionsDetails;
                    }

                    fullJourneyTransaction.ClinicalInterventions.Add(clinInter);
                }
                #endregion

                #region Get Desired Outcomes
                Journey_DesiredOutcomes_Details Journey_DesiredOutcomes_Details;

                for (int i = 0; i < fullJourneyTransaction.Transactions.Count; i++)
                {
                    desiredOutcomes = new Journey_DesiredOutcomes();
                    desiredOutcomes.StageColor = (i % 2 == 1) ? "rgb(255, 255, 255)" : "rgba(204, 204, 204, 0.48)";
                    desiredOutcomes.StageColorForLocation = (i % 2 == 1) ? "rgba(204, 204, 204, 0.48)" : "rgba(158, 158, 158, 0.48)";
                    List<Journey_DesiredOutcomes_Details> DesiredOutcomesDetails = new List<Journey_DesiredOutcomes_Details>();

                    for (int j = 0; j < fullJourneyTransaction.Transactions[i].TransactionsDetails.Count; j++)
                    {
                        desiredOutcomes.TransactionId = Convert.ToInt32(fullJourneyTransaction.Transactions[i].TransactionsDetails[j].PatientJourneyTransactionId);

                        var resultDesiredOutcomes = dbPatientJourney.GetDesiredOutcome(fullJourneyTransaction.Transactions[i].TransactionsDetails[j].PatientJourneyTransactionId);

                        if (resultDesiredOutcomes != null)
                        {
                            Journey_DesiredOutcomes_Details = new Journey_DesiredOutcomes_Details();
                            Journey_DesiredOutcomes_Details.DesiredOutcomeId = resultDesiredOutcomes.Patient_Journey_Transactions_DesiredOutcomes_Id;
                            Journey_DesiredOutcomes_Details.Description = resultDesiredOutcomes.Description;

                            DesiredOutcomesDetails.Add(Journey_DesiredOutcomes_Details);
                        }
                        else
                        {
                            Journey_DesiredOutcomes_Details = new Journey_DesiredOutcomes_Details();
                            DesiredOutcomesDetails.Add(Journey_DesiredOutcomes_Details);
                        }

                        desiredOutcomes.DesiredOutcomesDetails = DesiredOutcomesDetails;
                    }
                    fullJourneyTransaction.DesiredOutcomes.Add(desiredOutcomes);
                }
                #endregion

                #region Get Associated Costs

                for (int i = 0; i < fullJourneyTransaction.Transactions.Count; i++)
                {
                    associatedCost = new Journey_AssociatedCosts();
                    associatedCost.StageColor = (i % 2 == 1) ? "rgb(255, 255, 255)" : "rgba(204, 204, 204, 0.48)";
                    associatedCost.StageColorForLocation = (i % 2 == 1) ? "rgba(204, 204, 204, 0.48)" : "rgba(158, 158, 158, 0.48)";
                    List<Journey_AssociatedCosts_Details> AssociatedCostDetails = new List<Journey_AssociatedCosts_Details>();

                    for (int j = 0; j < fullJourneyTransaction.Transactions[i].TransactionsDetails.Count; j++)
                    {
                        associatedCost.TransactionId = Convert.ToInt32(fullJourneyTransaction.Transactions[i].TransactionsDetails[j].PatientJourneyTransactionId);

                        var resultAssociatedCost = dbPatientJourney.GetAssociatedCost(fullJourneyTransaction.Transactions[i].TransactionsDetails[j].PatientJourneyTransactionId);

                        if (resultAssociatedCost != null)
                        {
                            Journey_AssociatedCosts_Details Journey_AssociatedCost_Details = new Journey_AssociatedCosts_Details();
                            Journey_AssociatedCost_Details.AssociatedCostId = resultAssociatedCost.Patient_Journey_Transactions_AssociatedCosts_Id;
                            Journey_AssociatedCost_Details.Description = resultAssociatedCost.Description;

                            AssociatedCostDetails.Add(Journey_AssociatedCost_Details);
                        }
                        else
                        {
                            Journey_AssociatedCosts_Details Journey_AssociatedCost_Details = new Journey_AssociatedCosts_Details();
                            AssociatedCostDetails.Add(Journey_AssociatedCost_Details);
                        }

                        associatedCost.AssociatedCostDetails = AssociatedCostDetails;
                    }
                    fullJourneyTransaction.AssociatedCosts.Add(associatedCost);
                }
                #endregion
            }

            FullJourneyTransaction fullTransaction = new FullJourneyTransaction();
            fullTransaction.Transactions = fullJourneyTransaction.Transactions;
            fullTransaction.ClinicalInterventions = fullJourneyTransaction.ClinicalInterventions;
            fullTransaction.DesiredOutcomes = fullJourneyTransaction.DesiredOutcomes;
            fullTransaction.AssociatedCosts = fullJourneyTransaction.AssociatedCosts;
            return fullTransaction;
        }

        public static Journey_Transaction GetTransactions(string JourneyId, string StageId, int id)
        {
            //List<Patient_Journey_Strategic_Moment> listStrategic = new List<Patient_Journey_Strategic_Moment>();
            List<Journey_Transaction> listTransactions = new List<Journey_Transaction>();
            Journey_Transaction patienttransaction = new Journey_Transaction();

            patienttransaction.PatientStageId = Convert.ToInt32(StageId);

            var transaction = dbPatientAdministration.GetTransactions(Convert.ToInt32(StageId));

            //List<List<Patient_Journey_Transactions>> StrategicOutcome = dbPatientJourney.GetSMOMTransactions(Convert.ToInt32(StageId), Convert.ToInt32(JourneyId), out listStrategic);

            List<Journey_Transaction_Details> listTransactionsDetails = new List<Journey_Transaction_Details>();

            for (int i = 0; i < transaction.Count; i++)
            {
                List<ChartDetails> chartDetailsList = new List<ChartDetails>();
                List<StrategicMomentAll> strategicMomentAll = new List<StrategicMomentAll>();
                //StrategicMomentAll strategicdata = null;

                Journey_Transaction_Details Journey_Transaction_Details = new Journey_Transaction_Details();
                Journey_Transaction_Details.PatientJourneyTransactionId = transaction[i].Patient_Journey_Transactions_Id;
                Journey_Transaction_Details.PatientStageId = Convert.ToInt32(transaction[i].Patient_Journey_Stages_Id);
                Journey_Transaction_Details.PatientJourneyId = Convert.ToInt32(transaction[i].Patient_Journey_Id);
                Journey_Transaction_Details.TransactionMasterId = Convert.ToInt32(transaction[i].Transaction_Master_Id);
                Journey_Transaction_Details.ImageMasterId = dbPatientAdministration.GetImageMasterID(Journey_Transaction_Details.TransactionMasterId);
                Journey_Transaction_Details.ImagePath = dbPatientAdministration.GetImagePath(Journey_Transaction_Details.ImageMasterId);
                Journey_Transaction_Details.TransactionTitle = transaction[i].Transaction_Title;
                Journey_Transaction_Details.LocationId = Convert.ToInt32(transaction[i].Transaction_Location_Master_Id);
                Journey_Transaction_Details.LocationName = transaction[i].Transaction_Location_Title;
                Journey_Transaction_Details.TransactionDescription = transaction[i].Description;
                Journey_Transaction_Details.DisplayOrder = Convert.ToInt32(transaction[i].Transaction_Display_Order);
                Journey_Transaction_Details.HCPDescription = transaction[i].HCP_Description;
                Journey_Transaction_Details.HCPRating = transaction[i].HCP_Rating;
                Journey_Transaction_Details.PatientDescription = transaction[i].Patient_Description;
                Journey_Transaction_Details.PatientRating = transaction[i].Patient_Rating;
                Journey_Transaction_Details.PayerDescription = transaction[i].Payer_Description;
                Journey_Transaction_Details.PayerRating = transaction[i].Payer_Rating;
                Journey_Transaction_Details.FeasibilityDescription = transaction[i].Feasibility_Description;
                Journey_Transaction_Details.FeasibilityRating = transaction[i].Feasibility_Rating;
                Journey_Transaction_Details.ViabilityDescription = transaction[i].Viability_Description;
                Journey_Transaction_Details.ViabilityRating = transaction[i].Viability_Rating;

                for (int k = 0; k < 10; k++)
                {
                    ChartDetails _ChartDetails = new ChartDetails();

                    if (k == Journey_Transaction_Details.PatientRating)
                    {
                        _ChartDetails.DotColor = "#0019a2";
                        _ChartDetails.LineHeight = "15px";
                        _ChartDetails.MarginLeft = "12px";
                        _ChartDetails.FontSize = "40px";
                        chartDetailsList.Add(_ChartDetails);
                    }

                    else if (k == Journey_Transaction_Details.HCPRating)
                    {
                        _ChartDetails.DotColor = "#f44336";
                        _ChartDetails.LineHeight = "15px";
                        _ChartDetails.MarginLeft = "12px";
                        _ChartDetails.FontSize = "40px";
                        chartDetailsList.Add(_ChartDetails);
                    }

                    else if (k == Journey_Transaction_Details.PayerRating)
                    {
                        _ChartDetails.DotColor = "#4caf50";
                        _ChartDetails.LineHeight = "15px";
                        _ChartDetails.MarginLeft = "12px";
                        _ChartDetails.FontSize = "40px";
                        chartDetailsList.Add(_ChartDetails);
                    }

                    else
                    {
                        _ChartDetails.DotColor = "#ccc";
                        _ChartDetails.LineHeight = "15px";
                        _ChartDetails.MarginLeft = null;
                        _ChartDetails.FontSize = null;
                        chartDetailsList.Add(_ChartDetails);
                    }
                }
                Journey_Transaction_Details.ChartDetails = chartDetailsList;

                //var stagesAll = dbPatientAdministration.GetPatientJourneyStages();
                //var transactionAll = dbPatientAdministration.GetPatientJourneyTransactions();

                //if (StrategicOutcome != null)
                //{
                //    for (int j = 0; j < StrategicOutcome.Count; j++)
                //    {
                //        if (StrategicOutcome[j].Exists(x => x.Patient_Journey_Transactions_Id == transaction[i].Patient_Journey_Transactions_Id))
                //        {
                //            strategicdata = new StrategicMomentAll();
                //            strategicdata.IsStrategic = 1;
                //            strategicdata.SMOMDescription = listStrategic[j].Description;
                //            strategicdata.SMOMCategory = listStrategic[j].Category;
                //            if (listStrategic[j].Category == "HCP") { strategicdata.SMOMColor = "#ef8de3"; }
                //            else if (listStrategic[j].Category == "Patient") { strategicdata.SMOMColor = "#92dead"; }
                //            else if (listStrategic[j].Category == "Payer") { strategicdata.SMOMColor = "#d2cf70"; }
                //            strategicdata.StartStage = stagesAll.Where(x => x.Patient_Journey_Stages_Id == listStrategic[j].Patient_Journey_Start_Stage_Id).FirstOrDefault().Stage_Title;
                //            strategicdata.EndStage = stagesAll.Where(x => x.Patient_Journey_Stages_Id == listStrategic[j].Patient_Journey_End_Stage_Id).FirstOrDefault().Stage_Title;
                //            strategicdata.StartTransaction = transactionAll.Where(x => x.Patient_Journey_Transactions_Id == listStrategic[j].Patient_Journey_Start_Transaction_Id).FirstOrDefault().Transaction_Title;
                //            strategicdata.EndTransaction = transactionAll.Where(x => x.Patient_Journey_Transactions_Id == listStrategic[j].Patient_Journey_End_Transaction_Id).FirstOrDefault().Transaction_Title;
                //            strategicMomentAll.Add(strategicdata);
                //        }
                //        else
                //        {
                //            strategicdata = new StrategicMomentAll();
                //            strategicdata.SMOMColor = (id % 2 == 1) ? "rgba(204, 204, 204, 0)" : "rgba(158, 158, 158, 0)";
                //            strategicdata.IsStrategic = 0;
                //            strategicMomentAll.Add(strategicdata);
                //        }
                //    }
                //}
                //else
                //{
                //    strategicdata = new StrategicMomentAll();
                //    strategicdata.SMOMColor = (id % 2 == 1) ? "rgba(204, 204, 204, 0)" : "rgba(158, 158, 158, 0)";
                //    strategicdata.IsStrategic = 0;
                //    strategicMomentAll.Add(strategicdata);
                //}
                //Journey_Transaction_Details.StrategicMomentAll = strategicMomentAll;
                listTransactionsDetails.Add(Journey_Transaction_Details);
            }
            patienttransaction.TransactionsDetails = listTransactionsDetails;

            return patienttransaction;
        }


        public static int? SavePDF(string CountryId, string BrandId, string Year, byte[] file)
        {
            int? status = dbPatientJourney.SavePDF(Convert.ToInt32(CountryId), Convert.ToInt32(BrandId), Convert.ToInt32(Year), file);
            return status;
        }

        public static byte[] GetPDF(string CountryId, string BrandId, string Year)
        {
            byte[] status = dbPatientJourney.GetPDF(Convert.ToInt32(CountryId), Convert.ToInt32(BrandId), Convert.ToInt32(Year));
            return status;
        }

        public static List<StrategicMomentAll> GetStrategicMoment(string JourneyId)
        {
            List<StrategicMomentAll> strategicMomentList = new List<StrategicMomentAll>();
            PJEntities entity = new PJEntities();
            var strategic = dbPatientAdministration.GetStrategicMoment(Convert.ToInt32(JourneyId));
            int? TotalTransactions = dbPatientAdministration.GetAllTransactionCount(Convert.ToInt32(JourneyId));

            for (int i = 0; i < strategic.Count; i++)
            {
                StrategicMomentAll journeyStage = new StrategicMomentAll();
                journeyStage.JourneyId = strategic[i].JourneyId;
                journeyStage.StartStage = strategic[i].StartStage;
                journeyStage.EndStage = strategic[i].EndStage;
                journeyStage.StartTransaction = strategic[i].StartTransaction;
                journeyStage.EndTransaction = strategic[i].EndTransaction;
                journeyStage.SMOMCategory = strategic[i].SMOMCategory;
                journeyStage.SMOMDescription = strategic[i].SMOMDescription;
                //if (journeyStage.SMOMCategory == "HCP") { journeyStage.SMOMColor = "#ef8de3"; }
                //else if (journeyStage.SMOMCategory == "Patient") { journeyStage.SMOMColor = "#92dead"; }
                //else if (journeyStage.SMOMCategory == "Payer") { journeyStage.SMOMColor = "#d2cf70"; }
                journeyStage.SMOMColor = "#3f51b5";

                if (strategic[i].Stage_Display_Order == 1 && strategic[i].Transaction_Display_Order == 1)
                {
                    journeyStage.MarginLeft = (strategic[i].Stage_Display_Order * strategic[i].Transaction_Display_Order) * 0 + "px";
                }
                else if (strategic[i].Stage_Display_Order == 1 && strategic[i].Transaction_Display_Order == 2)
                {
                    journeyStage.MarginLeft = (strategic[i].Stage_Display_Order * strategic[i].Transaction_Display_Order) * 100 + "px";
                }
                else if (strategic[i].Stage_Display_Order == 1 && strategic[i].Transaction_Display_Order > 2)
                {
                    journeyStage.MarginLeft = (strategic[i].Stage_Display_Order * strategic[i].Transaction_Display_Order) * 107 + "px";
                }
                else
                {
                    int? previousCount = dbPatientAdministration.GetPreviousTransactionCount(strategic[i].Stage_Display_Order, journeyStage.JourneyId);
                    journeyStage.MarginLeft = (previousCount * 120) + (strategic[i].Transaction_Display_Order * 70) + "px";
                }

                int? width = dbPatientAdministration.GetWidth(journeyStage.StartStage, journeyStage.EndStage, journeyStage.JourneyId, journeyStage.StartTransaction, journeyStage.EndTransaction);
                journeyStage.Width = width * 105 + "px";

                strategicMomentList.Add(journeyStage);
            }
            return strategicMomentList;
        }

        public static int? SaveTempData(string CountryId, string BrandId, string Year, string UserName)
        {
            int? status = dbPatientJourney.SaveTempData(Convert.ToInt32(CountryId), Convert.ToInt32(BrandId), Convert.ToInt32(Year), UserName);
            return status;
        }

        public static List<Journey_Pdf_TempList> GetTempData(string UserName)
        {
            var pdfTempData = dbPatientJourney.GetTempData(UserName);
            return pdfTempData;
        }
    }
}
