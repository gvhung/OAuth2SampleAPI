using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientJourney.DataAccess.Data;
using PatientJourney.BusinessModel;

namespace PatientJourney.DataAccess.DataAccess
{
    public static class dbPatientJourney
    {
        public static Patient_Journey GetApprovedJourney(int CountryId, int BrandId, int Year)
        {
            Patient_Journey listWithSearch = new Patient_Journey();
            try
            {
                using (PJEntities entity = new PJEntities())
                {

                    listWithSearch = (from journey in entity.Patient_Journey
                                      where journey.Country_Master_Id == CountryId && journey.Brand_Master_Id == BrandId && journey.Year == Year && (journey.Status_Master_Id == 3 || journey.Status_Master_Id == 7)
                                      select journey).FirstOrDefault();
                }

            }
            catch (Exception)
            {
            }
            return listWithSearch;
        }

        public static Patient_Journey_Temp GetApprovedJourneyFromTemp(int CountryId, int BrandId, int Year)
        {
            Patient_Journey_Temp listWithSearch = new Patient_Journey_Temp();
            try
            {
                using (PJEntities entity = new PJEntities())
                {

                    listWithSearch = (from journey in entity.Patient_Journey_Temp
                                      where journey.Country_Master_Id == CountryId && journey.Brand_Master_Id == BrandId && journey.Year == Year && (journey.Status_Master_Id == 3 || journey.Status_Master_Id == 7)
                                      select journey).FirstOrDefault();
                }

            }
            catch (Exception)
            {
            }
            return listWithSearch;
        }

        public static Patient_Journey_Transactions_AssociatedCosts GetAssociatedCost(int TransactionId)
        {
            Patient_Journey_Transactions_AssociatedCosts listAsscost = new Patient_Journey_Transactions_AssociatedCosts();
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    listAsscost = entity.Patient_Journey_Transactions_AssociatedCosts.Where(x => x.Patient_Journey_Transactions_Id == TransactionId).FirstOrDefault();
                }
            }
            catch (Exception)
            {
            }
            return listAsscost;
        }

        public static Patient_Journey_Transactions_DesiredOutcomes GetDesiredOutcome(int TransactionId)
        {
            Patient_Journey_Transactions_DesiredOutcomes listDesout = new Patient_Journey_Transactions_DesiredOutcomes();
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    listDesout = entity.Patient_Journey_Transactions_DesiredOutcomes.Where(x => x.Patient_Journey_Transactions_Id == TransactionId).FirstOrDefault();
                }
            }
            catch (Exception)
            {
            }
            return listDesout;
        }

        public static Patient_Journey_Trans_Clin_Interventions GetClinicalIntervention(int TransactionId)
        {
            Patient_Journey_Trans_Clin_Interventions listClinicalInt = new Patient_Journey_Trans_Clin_Interventions();
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    listClinicalInt = entity.Patient_Journey_Trans_Clin_Interventions.Where(x => x.Patient_Journey_Transactions_Id == TransactionId).FirstOrDefault();
                }
            }
            catch (Exception)
            {
            }
            return listClinicalInt;
        }

        public static List<Patient_Journey_Trans_SubClin_Interventions> GetSubClinicalIntervention(int ClinicalIntId)
        {
            List<Patient_Journey_Trans_SubClin_Interventions> subClinInt = new List<Patient_Journey_Trans_SubClin_Interventions>();
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    subClinInt = entity.Patient_Journey_Trans_SubClin_Interventions.Where(x => x.Patient_Journey_Trans_Clin_Interventions_Id == ClinicalIntId).ToList();
                }
            }
            catch (Exception)
            {
            }
            return subClinInt;
        }

        public static Int32? AddClinicalInterventionToTemp(Patient_Journey_Trans_Clin_Interventions_Temp clinicalIntervention, int[] lstSubClinical)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    entity.Patient_Journey_Trans_Clin_Interventions_Temp.Add(clinicalIntervention);
                    Patient_Journey_Trans_SubClin_Interventions_Temp subClinical = null;
                    if (lstSubClinical != null)
                    {
                        for (int i = 0; i < lstSubClinical.Length; i++)
                        {
                            subClinical = new Patient_Journey_Trans_SubClin_Interventions_Temp();
                            subClinical.Patient_Journey_Trans_Clin_Interventions_Temp_Id = clinicalIntervention.Patient_Journey_Trans_Clin_Interventions_Temp_Id;
                            subClinical.SubClinical_Intervention_Master_Id = lstSubClinical[i];
                            subClinical.Created_By = clinicalIntervention.Created_By;
                            subClinical.Created_Date = clinicalIntervention.Created_Date;
                            subClinical.Modified_By = clinicalIntervention.Modified_By;
                            subClinical.Modified_Date = clinicalIntervention.Modified_Date;
                            entity.Patient_Journey_Trans_SubClin_Interventions_Temp.Add(subClinical);
                        }
                    }
                    entity.SaveChanges();
                    return clinicalIntervention.Patient_Journey_Trans_Clin_Interventions_Temp_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? AddSubClinicalInterventionToTemp(List<Patient_Journey_Trans_SubClin_Interventions_Temp> subclinicalIntervention)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    Patient_Journey_Trans_SubClin_Interventions_Temp subClinical = null;
                    if (subclinicalIntervention != null)
                    {
                        for (int i = 0; i < subclinicalIntervention.Count; i++)
                        {
                            subClinical = new Patient_Journey_Trans_SubClin_Interventions_Temp();
                            subClinical.Patient_Journey_Trans_Clin_Interventions_Temp_Id = subclinicalIntervention[i].Patient_Journey_Trans_Clin_Interventions_Temp_Id;
                            subClinical.SubClinical_Intervention_Master_Id = subclinicalIntervention[i].SubClinical_Intervention_Master_Id;
                            subClinical.Created_By = subclinicalIntervention[i].Created_By;
                            subClinical.Created_Date = subclinicalIntervention[i].Created_Date;
                            subClinical.Modified_By = subclinicalIntervention[i].Modified_By;
                            subClinical.Modified_Date = subclinicalIntervention[i].Modified_Date;
                            entity.Patient_Journey_Trans_SubClin_Interventions_Temp.Add(subClinical);
                            entity.SaveChanges();
                        }
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? AddAssociatedCostToTemp(Patient_Journey_Transactions_AssociatedCosts_Temp associatedCost)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    entity.Patient_Journey_Transactions_AssociatedCosts_Temp.Add(associatedCost);
                    entity.SaveChanges();
                    return associatedCost.Patient_Journey_Transactions_AssociatedCosts_Temp_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static Int32? AddDesiredOutcomeToTemp(Patient_Journey_Transactions_DesiredOutcomes_Temp desiredOutcome)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    entity.Patient_Journey_Transactions_DesiredOutcomes_Temp.Add(desiredOutcome);
                    entity.SaveChanges();
                    return desiredOutcome.Patient_Journey_Transactions_DesiredOutcomes_Temp_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static Int32? UpdateClinicalInterventionToTemp(Patient_Journey_Trans_Clin_Interventions_Temp clinicalIntervention, int[] lstSubClinical)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var currentclinical = entity.Patient_Journey_Trans_Clin_Interventions_Temp.Where(s => s.Patient_Journey_Trans_Clin_Interventions_Temp_Id == clinicalIntervention.Patient_Journey_Trans_Clin_Interventions_Temp_Id).FirstOrDefault();
                    var existingSubClinical = entity.Patient_Journey_Trans_SubClin_Interventions_Temp.Where(s => s.Patient_Journey_Trans_Clin_Interventions_Temp_Id == clinicalIntervention.Patient_Journey_Trans_Clin_Interventions_Temp_Id).ToList();
                    for (int i = 0; i < existingSubClinical.Count; i++)
                    {
                        entity.Patient_Journey_Trans_SubClin_Interventions_Temp.Remove(existingSubClinical[i]);
                    }
                    if (currentclinical != null)
                    {
                        currentclinical.Clinical_Intervention_Master_Id = clinicalIntervention.Clinical_Intervention_Master_Id;
                        currentclinical.Description = clinicalIntervention.Description;
                        currentclinical.Evidence = clinicalIntervention.Evidence;
                        currentclinical.Modified_By = clinicalIntervention.Modified_By;
                        currentclinical.Modified_Date = clinicalIntervention.Modified_Date;

                        Patient_Journey_Trans_SubClin_Interventions_Temp subClinical = null;
                        if (lstSubClinical != null)
                        {
                            for (int i = 0; i < lstSubClinical.Length; i++)
                            {
                                subClinical = new Patient_Journey_Trans_SubClin_Interventions_Temp();
                                subClinical.Patient_Journey_Trans_Clin_Interventions_Temp_Id = clinicalIntervention.Patient_Journey_Trans_Clin_Interventions_Temp_Id;
                                subClinical.SubClinical_Intervention_Master_Id = lstSubClinical[i];
                                subClinical.Created_By = clinicalIntervention.Created_By;
                                subClinical.Created_Date = DateTime.Now;
                                subClinical.Modified_By = clinicalIntervention.Modified_By;
                                subClinical.Modified_Date = clinicalIntervention.Modified_Date;
                                entity.Patient_Journey_Trans_SubClin_Interventions_Temp.Add(subClinical);
                            }
                        }
                    }
                    entity.SaveChanges();
                    return currentclinical.Patient_Journey_Trans_Clin_Interventions_Temp_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? UpdateAssociatedCostToTemp(Patient_Journey_Transactions_AssociatedCosts_Temp associatedCost)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var currentassociated = entity.Patient_Journey_Transactions_AssociatedCosts_Temp.Where(s => s.Patient_Journey_Transactions_AssociatedCosts_Temp_Id == associatedCost.Patient_Journey_Transactions_AssociatedCosts_Temp_Id).FirstOrDefault();
                    if (currentassociated != null)
                    {
                        currentassociated.AssociatedCosts = associatedCost.AssociatedCosts;
                        currentassociated.Description = associatedCost.Description;
                        currentassociated.Evidence = associatedCost.Evidence;
                        currentassociated.Modified_By = associatedCost.Modified_By;
                        currentassociated.Modified_Date = associatedCost.Modified_Date;
                    }
                    entity.SaveChanges();
                    return currentassociated.Patient_Journey_Transactions_AssociatedCosts_Temp_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? UpdateDesiredOutcomeToTemp(Patient_Journey_Transactions_DesiredOutcomes_Temp desiredOutcome)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var currentdesired = entity.Patient_Journey_Transactions_DesiredOutcomes_Temp.Where(s => s.Patient_Journey_Transactions_DesiredOutcomes_Temp_Id == desiredOutcome.Patient_Journey_Transactions_DesiredOutcomes_Temp_Id).FirstOrDefault();
                    if (currentdesired != null)
                    {
                        currentdesired.DesiredOutcomes = desiredOutcome.DesiredOutcomes;
                        currentdesired.Description = desiredOutcome.Description;
                        currentdesired.Evidence = desiredOutcome.Evidence;
                        currentdesired.MODIFIED_Date = desiredOutcome.MODIFIED_Date;
                        currentdesired.MODIFIED_By = desiredOutcome.MODIFIED_By;
                    }
                    entity.SaveChanges();
                    return currentdesired.Patient_Journey_Transactions_DesiredOutcomes_Temp_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? UpdatePatientRatingToTemp(Patient_Journey_Transactions_Temp transaction)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var currenttransaction = entity.Patient_Journey_Transactions_Temp.Where(s => s.Patient_Journey_Transactions_Temp_Id == transaction.Patient_Journey_Transactions_Temp_Id).FirstOrDefault();
                    if (currenttransaction != null)
                    {
                        currenttransaction.Patient_Description = transaction.Patient_Description;
                        currenttransaction.Patient_Rating = transaction.Patient_Rating;
                        currenttransaction.Patient_Evidence = transaction.Patient_Evidence;
                        currenttransaction.Modified_By = transaction.Modified_By;
                        currenttransaction.Modified_Date = transaction.Modified_Date;
                    }
                    entity.SaveChanges();
                    return currenttransaction.Patient_Journey_Transactions_Temp_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? UpdateHCPRatingToTemp(Patient_Journey_Transactions_Temp transaction)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var currenttransaction = entity.Patient_Journey_Transactions_Temp.Where(s => s.Patient_Journey_Transactions_Temp_Id == transaction.Patient_Journey_Transactions_Temp_Id).FirstOrDefault();
                    if (currenttransaction != null)
                    {
                        currenttransaction.HCP_Description = transaction.HCP_Description;
                        currenttransaction.HCP_Rating = transaction.HCP_Rating;
                        currenttransaction.HCP_Evidence = transaction.HCP_Evidence;
                        currenttransaction.Modified_By = transaction.Modified_By;
                        currenttransaction.Modified_Date = transaction.Modified_Date;
                    }
                    entity.SaveChanges();
                    return currenttransaction.Patient_Journey_Transactions_Temp_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? UpdatePayerRatingToTemp(Patient_Journey_Transactions_Temp transaction)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var currenttransaction = entity.Patient_Journey_Transactions_Temp.Where(s => s.Patient_Journey_Transactions_Temp_Id == transaction.Patient_Journey_Transactions_Temp_Id).FirstOrDefault();
                    if (currenttransaction != null)
                    {
                        currenttransaction.Payer_Description = transaction.Payer_Description;
                        currenttransaction.Payer_Rating = transaction.Payer_Rating;
                        currenttransaction.Modified_By = transaction.Modified_By;
                        currenttransaction.Payer_Evidence = transaction.Payer_Evidence;
                        currenttransaction.Modified_Date = transaction.Modified_Date;
                    }
                    entity.SaveChanges();
                    return currenttransaction.Patient_Journey_Transactions_Temp_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? UpdateFeasibilityRatingToTemp(Patient_Journey_Transactions_Temp transaction)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var currenttransaction = entity.Patient_Journey_Transactions_Temp.Where(s => s.Patient_Journey_Transactions_Temp_Id == transaction.Patient_Journey_Transactions_Temp_Id).FirstOrDefault();
                    if (currenttransaction != null)
                    {
                        currenttransaction.Feasibility_Description = transaction.Feasibility_Description;
                        currenttransaction.Feasibility_Rating = transaction.Feasibility_Rating;
                        currenttransaction.Feasibility_Evidence = transaction.Feasibility_Evidence;
                        currenttransaction.Modified_By = transaction.Modified_By;
                        currenttransaction.Modified_Date = transaction.Modified_Date;
                    }
                    entity.SaveChanges();
                    return currenttransaction.Patient_Journey_Transactions_Temp_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? UpdateViabilityRatingToTemp(Patient_Journey_Transactions_Temp transaction)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var currenttransaction = entity.Patient_Journey_Transactions_Temp.Where(s => s.Patient_Journey_Transactions_Temp_Id == transaction.Patient_Journey_Transactions_Temp_Id).FirstOrDefault();
                    if (currenttransaction != null)
                    {
                        currenttransaction.Viability_Rating = transaction.Viability_Rating;
                        currenttransaction.Viability_Description = transaction.Viability_Description;
                        currenttransaction.Viability_Evidence = transaction.Viability_Evidence;
                        currenttransaction.Modified_By = transaction.Modified_By;
                        currenttransaction.Modified_Date = transaction.Modified_Date;
                    }
                    entity.SaveChanges();
                    return currenttransaction.Patient_Journey_Transactions_Temp_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static List<List<Patient_Journey_Transactions>> GetSMOMTransactions(int Stageid, int JourneyId, out List<Patient_Journey_Strategic_Moment> listStrategic)
        {
            List<Patient_Journey_Strategic_Moment> strategic = new List<Patient_Journey_Strategic_Moment>();
            List<List<Patient_Journey_Transactions>> lstTransactions = new List<List<Patient_Journey_Transactions>>();
            listStrategic = new List<Patient_Journey_Strategic_Moment>();
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var CurrentStage = entity.Patient_Journey_Stages.Where(x => x.Patient_Journey_Stages_Id == Stageid).FirstOrDefault();
                    strategic = entity.Patient_Journey_Strategic_Moment.Where(x => x.Patient_Journey_Id == JourneyId).ToList();

                    for (int i = 0; i < strategic.Count; i++)
                    {
                        var startstage = strategic[i].Patient_Journey_Start_Stage_Id;
                        var endstage = strategic[i].Patient_Journey_End_Stage_Id;
                        var firstStage = entity.Patient_Journey_Stages.Where(x => x.Patient_Journey_Stages_Id == startstage).FirstOrDefault();
                        var lastStage = entity.Patient_Journey_Stages.Where(x => x.Patient_Journey_Stages_Id == endstage).FirstOrDefault();
                        if (startstage == endstage && CurrentStage.Stage_Display_Order == firstStage.Stage_Display_Order)
                        {
                            var starttransaction = strategic[i].Patient_Journey_Start_Transaction_Id;
                            var endtransaction = strategic[i].Patient_Journey_End_Transaction_Id;
                            var firstTransaction = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Transactions_Id == starttransaction).FirstOrDefault();
                            var lastTransaction = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Transactions_Id == endtransaction).FirstOrDefault();
                            lstTransactions.Add(entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Stages_Id == Stageid && x.Transaction_Display_Order >= firstTransaction.Transaction_Display_Order && x.Transaction_Display_Order <= lastTransaction.Transaction_Display_Order).OrderBy(x => x.Transaction_Display_Order).ToList());
                            listStrategic.Add(strategic[i]);
                        }
                        else if (CurrentStage.Stage_Display_Order == firstStage.Stage_Display_Order)
                        {
                            var starttransaction = strategic[i].Patient_Journey_Start_Transaction_Id;
                            var firstTransaction = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Transactions_Id == starttransaction).FirstOrDefault();
                            lstTransactions.Add(entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Stages_Id == Stageid && x.Transaction_Display_Order >= firstTransaction.Transaction_Display_Order).OrderBy(x => x.Transaction_Display_Order).ToList());
                            listStrategic.Add(strategic[i]);
                        }
                        else if (CurrentStage.Stage_Display_Order == lastStage.Stage_Display_Order)
                        {
                            var endtransaction = strategic[i].Patient_Journey_End_Transaction_Id;
                            var lastTransaction = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Transactions_Id == endtransaction).FirstOrDefault();
                            lstTransactions.Add(entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Stages_Id == Stageid && x.Transaction_Display_Order <= lastTransaction.Transaction_Display_Order).OrderBy(x => x.Transaction_Display_Order).ToList());
                            listStrategic.Add(strategic[i]);
                        }
                        else if (CurrentStage.Stage_Display_Order > firstStage.Stage_Display_Order && CurrentStage.Stage_Display_Order < lastStage.Stage_Display_Order)
                        {
                            lstTransactions.Add(entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Stages_Id == Stageid).OrderBy(x => x.Transaction_Display_Order).ToList());
                            listStrategic.Add(strategic[i]);
                        }
                    }

                }
            }
            catch (Exception)
            {
            }
            return lstTransactions;
        }

        public static List<List<Patient_Journey_Transactions_Temp>> GetSMOMTransactionsFromTemp(int Stageid, int JourneyId, out List<Patient_Journey_Strategic_Moment_Temp> listStrategic)
        {
            List<Patient_Journey_Strategic_Moment_Temp> strategic = new List<Patient_Journey_Strategic_Moment_Temp>();
            List<List<Patient_Journey_Transactions_Temp>> lstTransactions = new List<List<Patient_Journey_Transactions_Temp>>();
            listStrategic = new List<Patient_Journey_Strategic_Moment_Temp>();
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var CurrentStage = entity.Patient_Journey_Stages_Temp.Where(x => x.Patient_Journey_Stages_Temp_Id == Stageid).FirstOrDefault();
                    var tempJourneyId = entity.Patient_Journey_Temp.Where(x => x.Patient_Journey_Temp_Id == JourneyId).FirstOrDefault();
                    strategic = entity.Patient_Journey_Strategic_Moment_Temp.Where(x => x.Patient_Journey_Temp_Id == tempJourneyId.Patient_Journey_Temp_Id).ToList();

                    for (int i = 0; i < strategic.Count; i++)
                    {
                        var startstage = strategic[i].Patient_Journey_Start_Stage_Temp_Id;
                        var endstage = strategic[i].Patient_Journey_End_Stage_Temp_Id;
                        var firstStage = entity.Patient_Journey_Stages_Temp.Where(x => x.Patient_Journey_Stages_Temp_Id == startstage).FirstOrDefault();
                        var lastStage = entity.Patient_Journey_Stages_Temp.Where(x => x.Patient_Journey_Stages_Temp_Id == endstage).FirstOrDefault();
                        if (startstage == endstage && CurrentStage.Stage_Display_Order == firstStage.Stage_Display_Order)
                        {
                            var starttransaction = strategic[i].Patient_Journey_Start_Transaction_Temp_Id;
                            var endtransaction = strategic[i].Patient_Journey_End_Transaction_Temp_Id;
                            var firstTransaction = entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == starttransaction).FirstOrDefault();
                            var lastTransaction = entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == endtransaction).FirstOrDefault();
                            lstTransactions.Add(entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Stages_Temp_Id == Stageid && x.Transaction_Display_Order >= firstTransaction.Transaction_Display_Order && x.Transaction_Display_Order <= lastTransaction.Transaction_Display_Order).OrderBy(x => x.Transaction_Display_Order).ToList());
                            listStrategic.Add(strategic[i]);
                        }
                        else if (CurrentStage.Stage_Display_Order == firstStage.Stage_Display_Order)
                        {
                            var starttransaction = strategic[i].Patient_Journey_Start_Transaction_Temp_Id;
                            var firstTransaction = entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == starttransaction).FirstOrDefault();
                            lstTransactions.Add(entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Stages_Temp_Id == Stageid && x.Transaction_Display_Order >= firstTransaction.Transaction_Display_Order).OrderBy(x => x.Transaction_Display_Order).ToList());
                            listStrategic.Add(strategic[i]);
                        }
                        else if (CurrentStage.Stage_Display_Order == lastStage.Stage_Display_Order)
                        {
                            var endtransaction = strategic[i].Patient_Journey_End_Transaction_Temp_Id;
                            var lastTransaction = entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == endtransaction).FirstOrDefault();
                            lstTransactions.Add(entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Stages_Temp_Id == Stageid && x.Transaction_Display_Order <= lastTransaction.Transaction_Display_Order).OrderBy(x => x.Transaction_Display_Order).ToList());
                            listStrategic.Add(strategic[i]);
                        }
                        else if (CurrentStage.Stage_Display_Order > firstStage.Stage_Display_Order && CurrentStage.Stage_Display_Order < lastStage.Stage_Display_Order)
                        {
                            lstTransactions.Add(entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Stages_Temp_Id == Stageid).OrderBy(x => x.Transaction_Display_Order).ToList());
                            listStrategic.Add(strategic[i]);
                        }
                    }

                }
            }
            catch (Exception)
            {
            }
            return lstTransactions;
        }

        public static string GetStageName(Int32? Stageid)
        {
            string StageName = "";
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    StageName = entity.Patient_Journey_Stages.Where(x => x.Patient_Journey_Stages_Id == Stageid).FirstOrDefault().Stage_Title;
                }
                return StageName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetTransactionName(Int32? Transactionid)
        {
            string TransactionName = "";
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    TransactionName = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Transactions_Id == Transactionid).FirstOrDefault().Transaction_Title;
                }
                return TransactionName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetStageNameFromTemp(Int32? Stageid)
        {
            string StageName = "";
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    StageName = entity.Patient_Journey_Stages_Temp.Where(x => x.Patient_Journey_Stages_Temp_Id == Stageid).FirstOrDefault().Stage_Title;
                }
                return StageName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetTransactionNameFromTemp(Int32? Transactionid)
        {
            string TransactionName = "";
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    TransactionName = entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == Transactionid).FirstOrDefault().Transaction_Title;
                }
                return TransactionName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Patient_Journey_Transactions_DesiredOutcomes_Temp GetDesiredOutcomeFromTemp(int TransactionId)
        {
            Patient_Journey_Transactions_DesiredOutcomes_Temp desiredOut = new Patient_Journey_Transactions_DesiredOutcomes_Temp();
            using (PJEntities entity = new PJEntities())
            {
                desiredOut = entity.Patient_Journey_Transactions_DesiredOutcomes_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == TransactionId).FirstOrDefault();
                return desiredOut;
            }
        }

        public static Patient_Journey_Trans_Clin_Interventions_Temp GetClinicalInterventionFromTemp(int TransactionId)
        {
            Patient_Journey_Trans_Clin_Interventions_Temp clinInt = new Patient_Journey_Trans_Clin_Interventions_Temp();
            using (PJEntities entity = new PJEntities())
            {
                clinInt = entity.Patient_Journey_Trans_Clin_Interventions_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == TransactionId).FirstOrDefault();
                return clinInt;
            }
        }

        public static List<Patient_Journey_Trans_SubClin_Interventions_Temp> GetSubClinicalInterventionFromTemp(int ClinicalIntId)
        {
            List<Patient_Journey_Trans_SubClin_Interventions_Temp> subClinInt = new List<Patient_Journey_Trans_SubClin_Interventions_Temp>();
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    subClinInt = entity.Patient_Journey_Trans_SubClin_Interventions_Temp.Where(x => x.Patient_Journey_Trans_Clin_Interventions_Temp_Id == ClinicalIntId).ToList();
                }
            }
            catch (Exception)
            {
            }
            return subClinInt;
        }

        public static Patient_Journey_Transactions_AssociatedCosts_Temp GetAssociatedCostFromTemp(int TransactionId)
        {
            Patient_Journey_Transactions_AssociatedCosts_Temp assCost = new Patient_Journey_Transactions_AssociatedCosts_Temp();
            using (PJEntities entity = new PJEntities())
            {
                assCost = entity.Patient_Journey_Transactions_AssociatedCosts_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == TransactionId).FirstOrDefault();
                return assCost;
            }
        }


        public static int? SavePDF(int CountryId, int BrandId, int Year, byte[] file)
        {
            int status = 0;
            using (PJEntities entity = new PJEntities())
            {
                int Yearid = entity.Year_Master.Where(x => x.Year_Name == Year).Select(x => x.Year_Master_Id).FirstOrDefault();
                var currentPdf = entity.Journey_Pdf.Where(s => s.Country_Id == CountryId && s.Brand_Id == BrandId && s.Year == Yearid);

                foreach (var _currentPdf in currentPdf)
                {
                    entity.Journey_Pdf.Remove(_currentPdf);
                }
                entity.SaveChanges();

                //if (currentPdf != null)
                //{
                //    currentPdf.Country_Id = CountryId;
                //    currentPdf.Brand_Id = BrandId;
                //    currentPdf.Year = Yearid;
                //    currentPdf.Pdf_Byte = file;
                //    entity.SaveChanges();
                //    status = 2;
                //}
                //else
                //{
                Journey_Pdf pdf = new Journey_Pdf();
                pdf.Brand_Id = Convert.ToInt32(BrandId);
                pdf.Country_Id = Convert.ToInt32(CountryId);
                pdf.Year = Convert.ToInt32(Yearid);
                pdf.Pdf_Byte = file;
                pdf.CreatedBy = "ALAGAKX";
                pdf.CreatedDate = DateTime.Now;
                entity.Journey_Pdf.Add(pdf);
                entity.SaveChanges();
                status = 1;
                //}
            }
            return status;
        }

        public static byte[] GetPDF(int CountryId, int BrandId, int Year)
        {
            byte[] status = null;
            using (PJEntities entity = new PJEntities())
            {
                int Yearid = entity.Year_Master.Where(x => x.Year_Name == Year).Select(x => x.Year_Master_Id).FirstOrDefault();
                status = (byte[])entity.Journey_Pdf.Where(s => s.Country_Id == CountryId && s.Brand_Id == BrandId
                    && s.Year == Yearid)
                    .Select(x => x.Pdf_Byte).FirstOrDefault();
            }
            return status;
        }

        public static int? SaveTempData(int CountryId, int BrandId, int Year, string UserName)
        {
            int status = 0;
            using (PJEntities entity = new PJEntities())
            {
                var currentPdf = entity.Journey_Pdf_TempData.Where(s => s.UserName.ToUpper() == UserName.ToUpper());

                foreach (var _currentPdf in currentPdf)
                {
                    entity.Journey_Pdf_TempData.Remove(_currentPdf);
                }
                entity.SaveChanges();

                Journey_Pdf_TempData pdfTempData = new Journey_Pdf_TempData();
                pdfTempData.Brand_Id = Convert.ToInt32(BrandId);
                pdfTempData.Country_Id = Convert.ToInt32(CountryId);
                pdfTempData.Year = Convert.ToInt32(Year);
                pdfTempData.UserName = UserName.ToUpper();
                pdfTempData.CreatedBy = UserName;
                pdfTempData.CreatedDate = DateTime.UtcNow;
                pdfTempData.ModifiedDate = DateTime.UtcNow;
                entity.Journey_Pdf_TempData.Add(pdfTempData);
                entity.SaveChanges();
                status = 1;
            }
            return status;
        }

        public static List<Journey_Pdf_TempList> GetTempData(string UserName)
        {
            List<Journey_Pdf_TempList> pdfTempData = new List<Journey_Pdf_TempList>();
            using (PJEntities entity = new PJEntities())
            {
                pdfTempData = (from a in entity.Journey_Pdf_TempData
                               where a.UserName.ToUpper() == UserName.ToUpper()
                               select new Journey_Pdf_TempList()
                                                {
                                                    Country_Id = a.Country_Id,
                                                    Brand_Id = a.Brand_Id,
                                                    Year = a.Year,
                                                    UserName = a.UserName,
                                                    ModifiedDate = a.ModifiedDate
                                                }).OrderByDescending(x => x.ModifiedDate).ToList();

                return pdfTempData;
            }
        }

        public static Int32? GetStagesCount(Int32? JourneyId)
        {
            int StagesCount = 0;
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    StagesCount = entity.Patient_Journey_Stages.Where(x => x.Patient_Journey_Id == JourneyId).Select(x => x.Patient_Journey_Stages_Id).Count();
                }
                return StagesCount;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? GetTransactionCount(Int32? JourneyId)
        {
            int TransactionsCount = 0;
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    TransactionsCount = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Id == JourneyId).Select(x => x.Patient_Journey_Transactions_Id).Count();
                }
                return TransactionsCount;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? GetSmomCount(Int32? JourneyId)
        {
            int SMOMCount = 0;
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    SMOMCount = entity.Patient_Journey_Strategic_Moment.Where(x => x.Patient_Journey_Id == JourneyId).Select(x => x.Patient_Journey_Strategic_Moment_Id).Count();
                }
                return SMOMCount;
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }
}
