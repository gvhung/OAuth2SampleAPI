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
    public static class bsPatientJourney
    {
        public static FullPatientJounrney GetApprovedJourney(string CountryId, string BrandId, string Year)
        {
            FullPatientJounrney lstJourney = new FullPatientJounrney();
            PatientJourneyModel firstresult = new PatientJourneyModel();
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
                    firstresult.PatientJourneyId = result.Patient_Journey_Id;
                    firstresult.JourneyTitle = result.Journey_Title;
                    firstresult.StatusID = Convert.ToInt32(result.Status_Master_Id);
                    List<JourneyStage> stages = bsPatientAdministration.GetJourneyStage(result.Patient_Journey_Id.ToString());
                    lstJourney.Stage = stages;
                    lstJourney.Journey = firstresult;
                }
            }
            catch (Exception)
            {
            }
            return lstJourney;
        }

        public static FullPatientJounrney GetApprovedJourneyFromTemp(string CountryId, string BrandId, string Year)
        {
            FullPatientJounrney lstJourney = new FullPatientJounrney();
            PatientJourneyModel firstresult = new PatientJourneyModel();
            List<String> lstCountryId = new List<String>();
            lstCountryId = CountryId.Split(',').ToList();
            List<String> lstBrandId = new List<String>();
            lstBrandId = BrandId.Split(',').ToList();
            try
            {
                Patient_Journey_Temp result = new Patient_Journey_Temp();
                result = dbPatientJourney.GetApprovedJourneyFromTemp(Convert.ToInt32(lstCountryId[0]), Convert.ToInt32(lstBrandId[0]), Convert.ToInt32(Year));
                PJEntities entity = new PJEntities();
                if (result != null)
                {
                    firstresult.PatientJourneyId = result.Patient_Journey_Temp_Id;
                    firstresult.JourneyTitle = result.Journey_Title;
                    firstresult.StatusID = Convert.ToInt32(result.Status_Master_Id);
                    List<JourneyStage> stages = bsPatientAdministration.GetJourneyStageFromTemp(result.Patient_Journey_Temp_Id.ToString());
                    lstJourney.Stage = stages;
                    lstJourney.Journey = firstresult;
                }
            }
            catch (Exception)
            {
            }
            return lstJourney;
        }

        public static FullPatientTransaction GetFullTransactionDetails(string StageId, string JourneyId)
        {
            List<Transaction> listTransactions = bsPatientAdministration.GetTransactions(StageId);
            List<AssociatedCost> listAssociatedCost=new List<AssociatedCost>();
            List<DesiredOutcome> listDesiredOutcome=new List<DesiredOutcome>();
            List<ClinicalIntervention> listClinicalIntervention=new List<ClinicalIntervention>();
            List<Patient_Journey_Strategic_Moment> listStrategic = new List<Patient_Journey_Strategic_Moment>();
            AssociatedCost assCost = null;
            DesiredOutcome desOutcome = null;
            ClinicalIntervention clinInter = null;
            
            if (listTransactions != null)
            {
                List<List<Patient_Journey_Transactions>> StrategicOutcome = dbPatientJourney.GetSMOMTransactions(Convert.ToInt32(StageId), Convert.ToInt32(JourneyId), out listStrategic);
                
                for(int i=0;i<listTransactions.Count;i++)
                {
                    StrategicMomentDetails strategicdata = null;
                    if (StrategicOutcome != null)
                    {
                        listTransactions[i].StrategicMoment = new List<StrategicMomentDetails>();
                        for (int j = 0; j < StrategicOutcome.Count; j++)
                        {
                            if (StrategicOutcome[j].Exists(x => x.Patient_Journey_Transactions_Id == listTransactions[i].PatientJourneyTransactionId))
                            {
                                        strategicdata = new StrategicMomentDetails();
                                        strategicdata.IsStrategic = 1;
                                        strategicdata.SMOMDescription = listStrategic[j].Description;
                                        strategicdata.SMOMTitle = listStrategic[j].Title;
                                        strategicdata.SMOMCategory = listStrategic[j].Category;
                                        strategicdata.StartStage = dbPatientJourney.GetStageName(listStrategic[j].Patient_Journey_Start_Stage_Id);
                                        strategicdata.EndStage = dbPatientJourney.GetStageName(listStrategic[j].Patient_Journey_End_Stage_Id);
                                        strategicdata.StartTransaction = dbPatientJourney.GetTransactionName(listStrategic[j].Patient_Journey_Start_Transaction_Id);
                                        strategicdata.EndTransaction = dbPatientJourney.GetTransactionName(listStrategic[j].Patient_Journey_End_Transaction_Id);
                                        listTransactions[i].StrategicMoment.Add(strategicdata);
                            }
                            else
                            {
                                strategicdata = new StrategicMomentDetails();
                                strategicdata.IsStrategic = 0;
                                listTransactions[i].StrategicMoment.Add(strategicdata);
                            }
                        }
                       
                    }
                    else
                    {
                        strategicdata = new StrategicMomentDetails();
                        strategicdata.IsStrategic = 0;
                        listTransactions[i].StrategicMoment.Add(strategicdata);
                    }

                    assCost = new AssociatedCost();
                    var resultAsscost = dbPatientJourney.GetAssociatedCost(listTransactions[i].PatientJourneyTransactionId);
                    if (resultAsscost != null)
                    {
                        assCost.AssociatedCostId = resultAsscost.Patient_Journey_Transactions_AssociatedCosts_Id;
                        assCost.AssociatedCosts = resultAsscost.AssociatedCosts;
                        assCost.Evidence = resultAsscost.Evidence;
                        assCost.Description = resultAsscost.Description;                        
                    }
                    assCost.TransactionId = Convert.ToInt32(listTransactions[i].PatientJourneyTransactionId);
                    listAssociatedCost.Add(assCost);
               
                    desOutcome = new DesiredOutcome();
                    var resultdesOutcome = dbPatientJourney.GetDesiredOutcome(listTransactions[i].PatientJourneyTransactionId);
                    if (resultdesOutcome != null)
                    {
                        desOutcome.DesiredOutcomeId = resultdesOutcome.Patient_Journey_Transactions_DesiredOutcomes_Id;
                        desOutcome.DesiredOutcomes = resultdesOutcome.DesiredOutcomes;
                        desOutcome.Evidence = resultdesOutcome.Evidence;
                        desOutcome.Description = resultdesOutcome.Description;
                    }
                    desOutcome.TransactionId = Convert.ToInt32(listTransactions[i].PatientJourneyTransactionId);
                    listDesiredOutcome.Add(desOutcome);
              
                    clinInter = new ClinicalIntervention();
                    var resultClinInt = dbPatientJourney.GetClinicalIntervention(listTransactions[i].PatientJourneyTransactionId);
                    if (resultClinInt != null)
                    {
                        clinInter.ClinicalInterventionId = resultClinInt.Patient_Journey_Trans_Clin_Interventions_Id;
                        clinInter.ClinicalInterventionMasterId = resultClinInt.Clinical_Intervention_Master_Id;
                        if (clinInter.ClinicalInterventionMasterId != null)
                        {
                            var clinicalInterventionMasterDetails = dbPatientAdministration.GetClinicalInterventionDetails(clinInter.ClinicalInterventionMasterId);
                            clinInter.ClinicalInterventionTitle = clinicalInterventionMasterDetails.Select(x => x.Title).FirstOrDefault().ToString();
                            if (clinicalInterventionMasterDetails.Select(x => x.Image_Master_Id).FirstOrDefault() != null)
                            {
                                clinInter.ImageMasterId = (int)clinicalInterventionMasterDetails.Select(x => x.Image_Master_Id).FirstOrDefault();
                                clinInter.ImagePath = dbPatientAdministration.GetImagePath(clinInter.ImageMasterId);
                            }
                        }
                        clinInter.Evidence = resultClinInt.Evidence;
                        clinInter.Description = resultClinInt.Description;
                        
                        List<Patient_Journey_Trans_SubClin_Interventions> lstSubClinicalId = dbPatientJourney.GetSubClinicalIntervention(resultClinInt.Patient_Journey_Trans_Clin_Interventions_Id);
                        if (lstSubClinicalId.Count>0)
                        {
                            clinInter.SubClinicalId=new int[lstSubClinicalId.Count];
                            clinInter.SubClinicalImage = new string[lstSubClinicalId.Count];
                            clinInter.SubClinicalTitle = new string[lstSubClinicalId.Count];
                            for(int j=0;j<lstSubClinicalId.Count;j++)
                            {
                                clinInter.SubClinicalId[j] = lstSubClinicalId[j].SubClinical_Intervention_Master_Id;
                                clinInter.SubClinicalImage[j] = dbPatientAdministration.GetImagePath(lstSubClinicalId[j].SubClinical_Intervention_Master_Id);
                                var subclinMaster = DefaultListBSForPJ.GetSubClinicalMasterData();
                                clinInter.SubClinicalTitle[j] = subclinMaster.Where(x => x.SubClinicalMasterId == lstSubClinicalId[j].SubClinical_Intervention_Master_Id).FirstOrDefault().SubClinicalName;
                            }
                        }

                    }
                    clinInter.TransactionId = Convert.ToInt32(listTransactions[i].PatientJourneyTransactionId);
                    listClinicalIntervention.Add(clinInter);
                }
            }
            FullPatientTransaction fullTransaction = new FullPatientTransaction();
            fullTransaction.Transactions = listTransactions;
            fullTransaction.AssociatedCosts = listAssociatedCost;
            fullTransaction.DesiredOutcomes = listDesiredOutcome;
            fullTransaction.ClinicalInterventions = listClinicalIntervention;
            return fullTransaction;
        }

        public static FullPatientTransaction GetFullTransactionDetailsFromTemp(string StageId, string JourneyId)
        {
            List<Transaction> listTransactions = bsPatientAdministration.GetTransactionsFromTemp(StageId);
            List<AssociatedCost> listAssociatedCost = new List<AssociatedCost>();
            List<DesiredOutcome> listDesiredOutcome = new List<DesiredOutcome>();
            List<ClinicalIntervention> listClinicalIntervention = new List<ClinicalIntervention>();
            List<Patient_Journey_Strategic_Moment_Temp> listStrategic = new List<Patient_Journey_Strategic_Moment_Temp>();
            AssociatedCost assCost = null;
            DesiredOutcome desOutcome = null;
            ClinicalIntervention clinInter = null;

            if (listTransactions != null)
            {
                List<List<Patient_Journey_Transactions_Temp>> StrategicOutcome = dbPatientJourney.GetSMOMTransactionsFromTemp(Convert.ToInt32(StageId), Convert.ToInt32(JourneyId), out listStrategic);

                for (int i = 0; i < listTransactions.Count; i++)
                {
                    StrategicMomentDetails strategicdata = null;
                    if (StrategicOutcome != null)
                    {
                        listTransactions[i].StrategicMoment = new List<StrategicMomentDetails>();
                        for (int j = 0; j < StrategicOutcome.Count; j++)
                        {
                            if (StrategicOutcome[j].Exists(x => x.Patient_Journey_Transactions_Temp_Id == listTransactions[i].PatientJourneyTransactionId))
                            {
                                strategicdata = new StrategicMomentDetails();
                                strategicdata.IsStrategic = 1;
                                strategicdata.SMOMTitle = listStrategic[j].Title;
                                strategicdata.SMOMDescription = listStrategic[j].Description;
                                strategicdata.SMOMCategory = listStrategic[j].Category;
                                strategicdata.StartStage = dbPatientJourney.GetStageNameFromTemp(listStrategic[j].Patient_Journey_Start_Stage_Temp_Id);
                                strategicdata.EndStage = dbPatientJourney.GetStageNameFromTemp(listStrategic[j].Patient_Journey_End_Stage_Temp_Id);
                                strategicdata.StartTransaction = dbPatientJourney.GetTransactionNameFromTemp(listStrategic[j].Patient_Journey_Start_Transaction_Temp_Id);
                                strategicdata.EndTransaction = dbPatientJourney.GetTransactionNameFromTemp(listStrategic[j].Patient_Journey_End_Transaction_Temp_Id);
                                listTransactions[i].StrategicMoment.Add(strategicdata);
                            }
                            else
                            {
                                strategicdata = new StrategicMomentDetails();
                                strategicdata.IsStrategic = 0;
                                listTransactions[i].StrategicMoment.Add(strategicdata);
                            }
                        }

                    }
                    else
                    {
                        strategicdata = new StrategicMomentDetails();
                        strategicdata.IsStrategic = 0;
                        listTransactions[i].StrategicMoment.Add(strategicdata);
                    }

                    assCost = new AssociatedCost();
                    var resultAsscost = dbPatientJourney.GetAssociatedCostFromTemp(listTransactions[i].PatientJourneyTransactionId);
                    if (resultAsscost != null)
                    {
                        assCost.AssociatedCostId = resultAsscost.Patient_Journey_Transactions_AssociatedCosts_Temp_Id;
                        assCost.AssociatedCosts = resultAsscost.AssociatedCosts;
                        assCost.Evidence = resultAsscost.Evidence;
                        assCost.Description = resultAsscost.Description;
                    }
                    assCost.TransactionId = Convert.ToInt32(listTransactions[i].PatientJourneyTransactionId);
                    listAssociatedCost.Add(assCost);

                    desOutcome = new DesiredOutcome();
                    var resultdesOutcome = dbPatientJourney.GetDesiredOutcomeFromTemp(listTransactions[i].PatientJourneyTransactionId);
                    if (resultdesOutcome != null)
                    {
                        desOutcome.DesiredOutcomeId = resultdesOutcome.Patient_Journey_Transactions_DesiredOutcomes_Temp_Id;
                        desOutcome.DesiredOutcomes = resultdesOutcome.DesiredOutcomes;
                        desOutcome.Evidence = resultdesOutcome.Evidence;
                        desOutcome.Description = resultdesOutcome.Description;
                    }
                    desOutcome.TransactionId = Convert.ToInt32(listTransactions[i].PatientJourneyTransactionId);
                    listDesiredOutcome.Add(desOutcome);

                    clinInter = new ClinicalIntervention();
                    var resultClinInt = dbPatientJourney.GetClinicalInterventionFromTemp(listTransactions[i].PatientJourneyTransactionId);
                    if (resultClinInt != null)
                    {
                        clinInter.ClinicalInterventionId = resultClinInt.Patient_Journey_Trans_Clin_Interventions_Temp_Id;
                        clinInter.ClinicalInterventionMasterId =resultClinInt.Clinical_Intervention_Master_Id;
                        if (clinInter.ClinicalInterventionMasterId != null)
                        {
                            var clinicalInterventionMasterDetails = dbPatientAdministration.GetClinicalInterventionDetails(clinInter.ClinicalInterventionMasterId);
                            clinInter.ClinicalInterventionTitle = clinicalInterventionMasterDetails.Select(x => x.Title).FirstOrDefault().ToString();
                            if (clinicalInterventionMasterDetails.Select(x => x.Image_Master_Id).FirstOrDefault() != null)
                            {
                                clinInter.ImageMasterId = (int)clinicalInterventionMasterDetails.Select(x => x.Image_Master_Id).FirstOrDefault();
                                clinInter.ImagePath = dbPatientAdministration.GetImagePath(clinInter.ImageMasterId);
                            }
                        }
                        clinInter.Evidence = resultClinInt.Evidence;
                        clinInter.Description = resultClinInt.Description;
                        
                        List<Patient_Journey_Trans_SubClin_Interventions_Temp> lstSubClinicalId = dbPatientJourney.GetSubClinicalInterventionFromTemp(resultClinInt.Patient_Journey_Trans_Clin_Interventions_Temp_Id);
                        if (lstSubClinicalId.Count > 0)
                        {
                            clinInter.SubClinicalId = new int[lstSubClinicalId.Count];
                            clinInter.SubClinicalImage = new string[lstSubClinicalId.Count];
                            clinInter.SubClinicalTitle = new string[lstSubClinicalId.Count];
                            for (int j = 0; j < lstSubClinicalId.Count; j++)
                            {
                                clinInter.SubClinicalId[j] = lstSubClinicalId[j].SubClinical_Intervention_Master_Id;
                                clinInter.SubClinicalImage[j] = dbPatientAdministration.GetImagePath(lstSubClinicalId[j].SubClinical_Intervention_Master_Id);
                                var subclinMaster = DefaultListBSForPJ.GetSubClinicalMasterData();
                                clinInter.SubClinicalTitle[j] = subclinMaster.Where(x => x.SubClinicalMasterId == lstSubClinicalId[j].SubClinical_Intervention_Master_Id).FirstOrDefault().SubClinicalName;
                            }
                        }

                    }
                    clinInter.TransactionId = Convert.ToInt32(listTransactions[i].PatientJourneyTransactionId);
                    listClinicalIntervention.Add(clinInter);
                }
            }
            FullPatientTransaction fullTransaction = new FullPatientTransaction();
            fullTransaction.Transactions = listTransactions;
            fullTransaction.AssociatedCosts = listAssociatedCost;
            fullTransaction.DesiredOutcomes = listDesiredOutcome;
            fullTransaction.ClinicalInterventions = listClinicalIntervention;
            return fullTransaction;
        }

        public static Int32? AddClinicalInterventionToTemp(ClinicalIntervention clinicalIntervention, string User511, int JourneyId)
        {
            Patient_Journey_Trans_Clin_Interventions_Temp clinicalInt = new Patient_Journey_Trans_Clin_Interventions_Temp();
            clinicalInt.Clinical_Intervention_Master_Id = clinicalIntervention.ClinicalInterventionMasterId == 0 ? null : clinicalIntervention.ClinicalInterventionMasterId;
            clinicalInt.Description = clinicalIntervention.Description;
            clinicalInt.Evidence = clinicalIntervention.Evidence;
            clinicalInt.Patient_Journey_Transactions_Temp_Id = clinicalIntervention.TransactionId;
            clinicalInt.Created_By = User511;
            clinicalInt.Created_Date = DateTime.Now;
            clinicalInt.Modified_By = User511;
            clinicalInt.Modified_Date = DateTime.Now;

            var response = dbPatientJourney.AddClinicalInterventionToTemp(clinicalInt, clinicalIntervention.SubClinicalId);
            if (response != null)
            {
                var toBeApproved = dbPatientAdministration.ToBeApprovedJourney(JourneyId);
                Patient_Journey_VersionDetails_Temp versionDetails = new Patient_Journey_VersionDetails_Temp();
                versionDetails.Patient_Journey_Temp_Id = Convert.ToInt32(JourneyId);
                versionDetails.Version_Comments = GlobalConstants.VersionCommentsConstants.ClinIntAdded;
                versionDetails.Version_Title = GlobalConstants.VersionTitleConstants.ClinIntAdded;
                versionDetails.Created_By = User511;
                versionDetails.Created_Date = DateTime.Now;
                versionDetails.Modified_By = User511;
                versionDetails.Modified_Date = DateTime.Now;
                versionDetails.IsApproved = false;
                var version = dbPatientAdministration.AddVersionDetailsToTemp(versionDetails);
            }
            return response;
        }

        public static Int32? AddAssociatedCostToTemp(AssociatedCost assCost, string User511, int JourneyId)
        {
            Patient_Journey_Transactions_AssociatedCosts_Temp associatedCost = new Patient_Journey_Transactions_AssociatedCosts_Temp();
            associatedCost.AssociatedCosts = assCost.AssociatedCosts;
            associatedCost.Description = assCost.Description;
            associatedCost.Patient_Journey_Transactions_Temp_Id = assCost.TransactionId;
            associatedCost.Evidence = assCost.Evidence;
            associatedCost.Created_By = User511;
            associatedCost.Created_Date = DateTime.Now;
            associatedCost.Modified_By = User511;
            associatedCost.Modified_Date = DateTime.Now;

            var response = dbPatientJourney.AddAssociatedCostToTemp(associatedCost);
            if (response != null)
            {
                var toBeApproved = dbPatientAdministration.ToBeApprovedJourney(JourneyId);
                Patient_Journey_VersionDetails_Temp versionDetails = new Patient_Journey_VersionDetails_Temp();
                versionDetails.Patient_Journey_Temp_Id = Convert.ToInt32(JourneyId);
                versionDetails.Version_Comments = "New associated cost"+ assCost.AssociatedCosts + " added";
                versionDetails.Version_Title = GlobalConstants.VersionTitleConstants.AssociatedAdded;
                versionDetails.Created_By = User511;
                versionDetails.Created_Date = DateTime.Now;
                versionDetails.Modified_By = User511;
                versionDetails.Modified_Date = DateTime.Now;
                versionDetails.IsApproved = false;
                var version = dbPatientAdministration.AddVersionDetailsToTemp(versionDetails);
            }
            return response;
        }

        public static Int32? AddDesiredOutcomeToTemp(DesiredOutcome desOutcome, string User511, int JourneyId)
        {
            Patient_Journey_Transactions_DesiredOutcomes_Temp desiredOutcome = new Patient_Journey_Transactions_DesiredOutcomes_Temp();
            desiredOutcome.DesiredOutcomes = desOutcome.DesiredOutcomes;
            desiredOutcome.Description = desOutcome.Description;
            desiredOutcome.Evidence = desOutcome.Evidence;
            desiredOutcome.Patient_Journey_Transactions_Temp_Id = desOutcome.TransactionId;
            desiredOutcome.Created_By = User511;
            desiredOutcome.Created_Date = DateTime.Now;
            desiredOutcome.MODIFIED_By = User511;
            desiredOutcome.MODIFIED_Date = DateTime.Now;

            var response = dbPatientJourney.AddDesiredOutcomeToTemp(desiredOutcome);
            if (response != null)
            {
                var toBeApproved = dbPatientAdministration.ToBeApprovedJourney(JourneyId);
                Patient_Journey_VersionDetails_Temp versionDetails = new Patient_Journey_VersionDetails_Temp();
                versionDetails.Patient_Journey_Temp_Id = Convert.ToInt32(JourneyId);
                versionDetails.Version_Comments = "New desired outcome" + desOutcome.DesiredOutcomes + " added";
                versionDetails.Version_Title = GlobalConstants.VersionTitleConstants.DesiredAdded;
                versionDetails.Created_By = User511;
                versionDetails.Created_Date = DateTime.Now;
                versionDetails.Modified_By = User511;
                versionDetails.Modified_Date = DateTime.Now;
                versionDetails.IsApproved = false;
                var version = dbPatientAdministration.AddVersionDetailsToTemp(versionDetails);
            }
            return response;
        }

        public static Int32? UpdateClinicalInterventionToTemp(ClinicalIntervention clinicalIntervention, string User511, int JourneyId)
        {
            Patient_Journey_Trans_Clin_Interventions_Temp clinicalInt = new Patient_Journey_Trans_Clin_Interventions_Temp();
            clinicalInt.Patient_Journey_Trans_Clin_Interventions_Temp_Id = clinicalIntervention.ClinicalInterventionId;
            clinicalInt.Clinical_Intervention_Master_Id = clinicalIntervention.ClinicalInterventionMasterId == 0 ? null : clinicalIntervention.ClinicalInterventionMasterId;
            clinicalInt.Description = clinicalIntervention.Description;
            clinicalInt.Evidence = clinicalIntervention.Evidence;
            clinicalInt.Modified_By = User511;
            clinicalInt.Modified_Date = DateTime.Now;

            var response = dbPatientJourney.UpdateClinicalInterventionToTemp(clinicalInt, clinicalIntervention.SubClinicalId);
            if (response != null)
            {
                var toBeApproved = dbPatientAdministration.ToBeApprovedJourney(JourneyId);
                Patient_Journey_VersionDetails_Temp versionDetails = new Patient_Journey_VersionDetails_Temp();
                versionDetails.Patient_Journey_Temp_Id = Convert.ToInt32(JourneyId);
                versionDetails.Version_Comments = GlobalConstants.VersionCommentsConstants.ClinIntUpdated;
                versionDetails.Version_Title = GlobalConstants.VersionTitleConstants.ClinIntUpdated;
                versionDetails.Created_By = User511;
                versionDetails.Created_Date = DateTime.Now;
                versionDetails.Modified_By = User511;
                versionDetails.Modified_Date = DateTime.Now;
                versionDetails.IsApproved = false;
                var version = dbPatientAdministration.AddVersionDetailsToTemp(versionDetails);
            }
            return response;
        }

        public static Int32? UpdateAssociatedCostToTemp(AssociatedCost assCost, string User511, int JourneyId)
        {
            Patient_Journey_Transactions_AssociatedCosts_Temp associatedCost = new Patient_Journey_Transactions_AssociatedCosts_Temp();
            associatedCost.Patient_Journey_Transactions_AssociatedCosts_Temp_Id = assCost.AssociatedCostId;
            associatedCost.AssociatedCosts = assCost.AssociatedCosts;
            associatedCost.Description = assCost.Description;
            associatedCost.Evidence = assCost.Evidence;
            associatedCost.Modified_By = User511;
            associatedCost.Modified_Date = DateTime.Now;

            var response = dbPatientJourney.UpdateAssociatedCostToTemp(associatedCost);
            if (response != null)
            {
                var toBeApproved = dbPatientAdministration.ToBeApprovedJourney(JourneyId);
                Patient_Journey_VersionDetails_Temp versionDetails = new Patient_Journey_VersionDetails_Temp();
                versionDetails.Patient_Journey_Temp_Id = Convert.ToInt32(JourneyId);
                versionDetails.Version_Comments = GlobalConstants.VersionCommentsConstants.AssociatedUpdated;
                versionDetails.Version_Title = GlobalConstants.VersionTitleConstants.AssociatedUpdated ;
                versionDetails.Created_By = User511;
                versionDetails.Created_Date = DateTime.Now;
                versionDetails.Modified_By = User511;
                versionDetails.Modified_Date = DateTime.Now;
                versionDetails.IsApproved = false;
                var version = dbPatientAdministration.AddVersionDetailsToTemp(versionDetails);
            }
            return response;
        }

        public static Int32? UpdateDesiredOutcomeToTemp(DesiredOutcome desOutcome, string User511, int JourneyId)
        {
            Patient_Journey_Transactions_DesiredOutcomes_Temp desiredOutcome = new Patient_Journey_Transactions_DesiredOutcomes_Temp();
            desiredOutcome.Patient_Journey_Transactions_DesiredOutcomes_Temp_Id = desOutcome.DesiredOutcomeId;
            desiredOutcome.DesiredOutcomes = desOutcome.DesiredOutcomes;
            desiredOutcome.Description = desOutcome.Description;
            desiredOutcome.Evidence = desOutcome.Evidence;
            desiredOutcome.MODIFIED_By = User511;
            desiredOutcome.MODIFIED_Date = DateTime.Now;

            var response = dbPatientJourney.UpdateDesiredOutcomeToTemp(desiredOutcome);
            if (response != null)
            {
                var toBeApproved = dbPatientAdministration.ToBeApprovedJourney(JourneyId);
                Patient_Journey_VersionDetails_Temp versionDetails = new Patient_Journey_VersionDetails_Temp();
                versionDetails.Patient_Journey_Temp_Id = Convert.ToInt32(JourneyId);
                versionDetails.Version_Comments = GlobalConstants.VersionCommentsConstants.DesiredUpdated;
                versionDetails.Version_Title = GlobalConstants.VersionTitleConstants.DesiredUpdated;
                versionDetails.Created_By = User511;
                versionDetails.Created_Date = DateTime.Now;
                versionDetails.Modified_By = User511;
                versionDetails.Modified_Date = DateTime.Now;
                versionDetails.IsApproved = false;
                var version = dbPatientAdministration.AddVersionDetailsToTemp(versionDetails);
            }
            return response;
        }

        public static Int32? UpdatePatientRatingToTemp(Transaction transaction, string User511, int JourneyId)
        {
            Patient_Journey_Transactions_Temp patientTransaction = new Patient_Journey_Transactions_Temp();
            patientTransaction.Patient_Journey_Transactions_Temp_Id = transaction.PatientJourneyTransactionId;
            patientTransaction.Patient_Rating = transaction.PatientRating;
            patientTransaction.Patient_Evidence = transaction.PatientEvidence;
            patientTransaction.Patient_Description = transaction.PatientDescription;
            patientTransaction.Modified_By = User511;
            patientTransaction.Modified_Date = DateTime.Now;

            var response = dbPatientJourney.UpdatePatientRatingToTemp(patientTransaction);
            if (response != null)
            {
                var toBeApproved = dbPatientAdministration.ToBeApprovedJourney(JourneyId);
                Patient_Journey_VersionDetails_Temp versionDetails = new Patient_Journey_VersionDetails_Temp();
                versionDetails.Patient_Journey_Temp_Id = Convert.ToInt32(JourneyId);
                versionDetails.Version_Comments = GlobalConstants.VersionCommentsConstants.PatientUpdated;
                versionDetails.Version_Title = GlobalConstants.VersionTitleConstants.PatientUpdated;
                versionDetails.Created_By = User511;
                versionDetails.Created_Date = DateTime.Now;
                versionDetails.Modified_By = User511;
                versionDetails.Modified_Date = DateTime.Now;
                versionDetails.IsApproved = false;
                var version = dbPatientAdministration.AddVersionDetailsToTemp(versionDetails);
            }
            return response;
        }

        public static Int32? UpdateHCPRatingToTemp(Transaction transaction, string User511, int JourneyId)
        {
            Patient_Journey_Transactions_Temp patientTransaction = new Patient_Journey_Transactions_Temp();
            patientTransaction.Patient_Journey_Transactions_Temp_Id = transaction.PatientJourneyTransactionId;
            patientTransaction.HCP_Rating = transaction.HCPRating;
            patientTransaction.HCP_Evidence = transaction.HCPEvidence;
            patientTransaction.HCP_Description = transaction.HCPDescription;
            patientTransaction.Modified_By = User511;
            patientTransaction.Modified_Date = DateTime.Now;

            var response = dbPatientJourney.UpdateHCPRatingToTemp(patientTransaction);
            if (response != null)
            {
                var toBeApproved = dbPatientAdministration.ToBeApprovedJourney(JourneyId);
                Patient_Journey_VersionDetails_Temp versionDetails = new Patient_Journey_VersionDetails_Temp();
                versionDetails.Patient_Journey_Temp_Id = Convert.ToInt32(JourneyId);
                versionDetails.Version_Comments = GlobalConstants.VersionCommentsConstants.HCPUpdated;
                versionDetails.Version_Title = GlobalConstants.VersionTitleConstants.HCPUpdated;
                versionDetails.Created_By = User511;
                versionDetails.Created_Date = DateTime.Now;
                versionDetails.Modified_By = User511;
                versionDetails.Modified_Date = DateTime.Now;
                versionDetails.IsApproved = false;
                var version = dbPatientAdministration.AddVersionDetailsToTemp(versionDetails);
            }
            return response;
        }

        public static Int32? UpdatePayerRatingToTemp(Transaction transaction, string User511, int JourneyId)
        {
            Patient_Journey_Transactions_Temp patientTransaction = new Patient_Journey_Transactions_Temp();
            patientTransaction.Patient_Journey_Transactions_Temp_Id = transaction.PatientJourneyTransactionId;
            patientTransaction.Payer_Rating = transaction.PayerRating;
            patientTransaction.Payer_Evidence = transaction.PayerEvidence;
            patientTransaction.Payer_Description = transaction.PayerDescription;
            patientTransaction.Modified_By = User511;
            patientTransaction.Modified_Date = DateTime.Now;

            var response = dbPatientJourney.UpdatePayerRatingToTemp(patientTransaction);
            if (response != null)
            {
                var toBeApproved = dbPatientAdministration.ToBeApprovedJourney(JourneyId);
                Patient_Journey_VersionDetails_Temp versionDetails = new Patient_Journey_VersionDetails_Temp();
                versionDetails.Patient_Journey_Temp_Id = Convert.ToInt32(JourneyId);
                versionDetails.Version_Comments = GlobalConstants.VersionCommentsConstants.PayerUpdated;
                versionDetails.Version_Title = GlobalConstants.VersionTitleConstants.PayerUpdated;
                versionDetails.Created_By = User511;
                versionDetails.Created_Date = DateTime.Now;
                versionDetails.Modified_By = User511;
                versionDetails.Modified_Date = DateTime.Now;
                versionDetails.IsApproved = false;
                var version = dbPatientAdministration.AddVersionDetailsToTemp(versionDetails);
            }
            return response;
        }

        public static Int32? UpdateFeasibilityRatingToTemp(Transaction transaction, string User511, int JourneyId)
        {
            Patient_Journey_Transactions_Temp patientTransaction = new Patient_Journey_Transactions_Temp();
            patientTransaction.Patient_Journey_Transactions_Temp_Id = transaction.PatientJourneyTransactionId;
            patientTransaction.Feasibility_Rating = transaction.FeasibilityRating;
            patientTransaction.Feasibility_Evidence = transaction.FeasibilityEvidence;
            patientTransaction.Feasibility_Description = transaction.FeasibilityDescription;
            patientTransaction.Modified_By = User511;
            patientTransaction.Modified_Date = DateTime.Now;

            var response = dbPatientJourney.UpdateFeasibilityRatingToTemp(patientTransaction);
            if (response != null)
            {
                var toBeApproved = dbPatientAdministration.ToBeApprovedJourney(JourneyId);
                Patient_Journey_VersionDetails_Temp versionDetails = new Patient_Journey_VersionDetails_Temp();
                versionDetails.Patient_Journey_Temp_Id = Convert.ToInt32(JourneyId);
                versionDetails.Version_Comments = GlobalConstants.VersionCommentsConstants.FeasibilityUpdated;
                versionDetails.Version_Title = GlobalConstants.VersionTitleConstants.FeasibilityUpdated;
                versionDetails.Created_By = User511;
                versionDetails.Created_Date = DateTime.Now;
                versionDetails.Modified_By = User511;
                versionDetails.Modified_Date = DateTime.Now;
                versionDetails.IsApproved = false;
                var version = dbPatientAdministration.AddVersionDetailsToTemp(versionDetails);
            }
            return response;
        }

        public static Int32? UpdateViabilityRatingToTemp(Transaction transaction, string User511, int JourneyId)
        {
            Patient_Journey_Transactions_Temp patientTransaction = new Patient_Journey_Transactions_Temp();
            patientTransaction.Patient_Journey_Transactions_Temp_Id = transaction.PatientJourneyTransactionId;
            patientTransaction.Viability_Rating = transaction.ViabilityRating;
            patientTransaction.Viability_Evidence = transaction.ViabilityEvidence;
            patientTransaction.Viability_Description = transaction.ViabilityDescription;
            patientTransaction.Modified_By = User511;
            patientTransaction.Modified_Date = DateTime.Now;

            var response = dbPatientJourney.UpdateViabilityRatingToTemp(patientTransaction);
            if (response != null)
            {
                var toBeApproved = dbPatientAdministration.ToBeApprovedJourney(JourneyId);
                Patient_Journey_VersionDetails_Temp versionDetails = new Patient_Journey_VersionDetails_Temp();
                versionDetails.Patient_Journey_Temp_Id = Convert.ToInt32(JourneyId);
                versionDetails.Version_Comments = GlobalConstants.VersionCommentsConstants.ViabilityUpdated;
                versionDetails.Version_Title = GlobalConstants.VersionTitleConstants.ViabilityUpdated;
                versionDetails.Created_By = User511;
                versionDetails.Created_Date = DateTime.Now;
                versionDetails.Modified_By = User511;
                versionDetails.Modified_Date = DateTime.Now;
                versionDetails.IsApproved = false;
                var version = dbPatientAdministration.AddVersionDetailsToTemp(versionDetails);
            }
            return response;
        }

    }
}
