using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientJourney.DataAccess.Data;
using System.Data;
using PatientJourney.BusinessModel.BuilderModels;

namespace PatientJourney.DataAccess.DataAccess
{
    public static class dbPatientAdministration
    {
        public static Int32? AddJourney(Patient_Journey patientJourney)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    entity.Patient_Journey.Add(patientJourney);
                    entity.SaveChanges();
                    return patientJourney.Patient_Journey_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static Int32? RemoveJourney(int JourneyId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var journeyid = entity.Patient_Journey.Where(x => x.Patient_Journey_Id == JourneyId).FirstOrDefault();
                    var stageid = entity.Patient_Journey_Stages.Where(x => x.Patient_Journey_Id == JourneyId).ToList();
                    if (stageid.Count > 0)
                    {
                        for (int i = 0; i < stageid.Count; i++)
                        {
                            var transactionid = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Id == JourneyId).ToList();

                            if (transactionid.Count > 0)
                            {
                                for (int j = 0; j < transactionid.Count; j++)
                                {
                                    var transid = transactionid[j].Patient_Journey_Transactions_Id;
                                    var asscostid = entity.Patient_Journey_Transactions_AssociatedCosts.Where(x => x.Patient_Journey_Transactions_Id == transid).ToList();
                                    if (asscostid.Count > 0)
                                    {
                                        for (int k = 0; k < asscostid.Count; k++)
                                        {
                                            entity.Patient_Journey_Transactions_AssociatedCosts.Remove(asscostid[k]);
                                        }
                                    }
                                    var desiredid = entity.Patient_Journey_Transactions_DesiredOutcomes.Where(x => x.Patient_Journey_Transactions_Id == transid).ToList();
                                    if (desiredid.Count > 0)
                                    {
                                        for (int k = 0; k < desiredid.Count; k++)
                                        {
                                            entity.Patient_Journey_Transactions_DesiredOutcomes.Remove(desiredid[k]);
                                        }
                                    }
                                    var clininterid = entity.Patient_Journey_Trans_Clin_Interventions.Where(x => x.Patient_Journey_Transactions_Id == transid).ToList();
                                    if (clininterid.Count > 0)
                                    {
                                        for (int k = 0; k < clininterid.Count; k++)
                                        {
                                            int clinid = clininterid[k].Patient_Journey_Trans_Clin_Interventions_Id;
                                            var subclininterid = entity.Patient_Journey_Trans_SubClin_Interventions.Where(x => x.Patient_Journey_Trans_Clin_Interventions_Id == clinid).ToList();
                                            if (subclininterid != null)
                                            {
                                                for (int l = 0; l < subclininterid.Count; l++)
                                                {
                                                    entity.Patient_Journey_Trans_SubClin_Interventions.Remove(subclininterid[l]);
                                                }
                                            }
                                            entity.Patient_Journey_Trans_Clin_Interventions.Remove(clininterid[k]);
                                        }
                                    }
                                    entity.Patient_Journey_Transactions.Remove(transactionid[j]);
                                }
                            }
                            entity.Patient_Journey_Stages.Remove(stageid[i]);
                        }
                    }
                    entity.Patient_Journey.Remove(journeyid);
                    entity.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? SubmitJourney(Patient_Journey patientJourney)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var journeyrecord = entity.Patient_Journey.Where(s => s.Patient_Journey_Id == patientJourney.Patient_Journey_Id).FirstOrDefault();
                    journeyrecord.User_Comments = patientJourney.User_Comments;
                    journeyrecord.Status_Master_Id = patientJourney.Status_Master_Id;
                    journeyrecord.Submitted_By = patientJourney.Submitted_By;
                    journeyrecord.Submitted_Date = patientJourney.Submitted_Date;
                    journeyrecord.Modified_By = patientJourney.Modified_By;
                    journeyrecord.Modified_Date = patientJourney.Modified_Date;
                    entity.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static List<Patient_Journey> GetSearchResults(int CountryId, int BrandId, int Year)
        {
            List<Patient_Journey> listWithSearch = new List<Patient_Journey>();
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    //if (RoleIds.Contains("2"))
                    //{
                    listWithSearch = (from journey in entity.Patient_Journey
                                      where journey.Country_Master_Id == CountryId && journey.Brand_Master_Id == BrandId && journey.Year == Year
                                      orderby journey.Modified_Date descending
                                      select journey).ToList();
                    //}
                    //else
                    //{
                    //    listWithSearch = (from journey in entity.Patient_Journey
                    //                      where (journey.Country_Master_Id == CountryId && journey.Brand_Master_Id == BrandId && journey.Year == Year && journey.Created_By == User511)
                    //                      || (journey.Country_Master_Id == CountryId && journey.Brand_Master_Id == BrandId && journey.Year == Year && journey.Created_By != User511 && (journey.Status_Master_Id==2 || journey.Status_Master_Id==3))
                    //                      orderby journey.Modified_Date descending
                    //                      select journey).ToList();
                    //}
                }

            }
            catch (Exception)
            {
            }
            return listWithSearch;
        }

        public static Int32? ApproveRejectSendbackJourney(Patient_Journey patientJourney, out Patient_Journey otherapproved)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var journeyrecord = entity.Patient_Journey.Where(s => s.Patient_Journey_Id == patientJourney.Patient_Journey_Id).FirstOrDefault();
                    var journeyrecordtemp = entity.Patient_Journey_Temp.Where(s => s.Patient_Journey_Id == patientJourney.Patient_Journey_Id).FirstOrDefault();
                    if (patientJourney.Status_Master_Id == 3 || patientJourney.Status_Master_Id == 7)
                    {
                        otherapproved = entity.Patient_Journey.Where(s => (s.Status_Master_Id == 3 || s.Status_Master_Id == 7) && s.Brand_Master_Id == patientJourney.Brand_Master_Id && s.Country_Master_Id == patientJourney.Country_Master_Id && s.Year == patientJourney.Year).FirstOrDefault();
                        if (otherapproved != null)
                        {
                            otherapproved.Status_Master_Id = 6;
                        }
                        var otherapprovedtemp = entity.Patient_Journey_Temp.Where(s => (s.Status_Master_Id == 3 || s.Status_Master_Id == 7) && s.Brand_Master_Id == patientJourney.Brand_Master_Id && s.Country_Master_Id == patientJourney.Country_Master_Id && s.Year == patientJourney.Year).FirstOrDefault();
                        if (otherapprovedtemp != null)
                        {
                            otherapprovedtemp.Status_Master_Id = 6;
                        }
                    }
                    else
                    {
                        otherapproved = null;
                    }

                    journeyrecord.Approver_Comments = patientJourney.Approver_Comments;
                    journeyrecord.Status_Master_Id = patientJourney.Status_Master_Id;
                    journeyrecord.Approved_By = patientJourney.Approved_By;
                    journeyrecord.Approved_Date = patientJourney.Approved_Date;
                    journeyrecord.Modified_By = patientJourney.Modified_By;
                    journeyrecord.Modified_Date = patientJourney.Modified_Date;
                    if (journeyrecordtemp != null)
                    {
                        journeyrecordtemp.Approver_Comments = patientJourney.Approver_Comments;
                        journeyrecordtemp.Status_Master_Id = Convert.ToInt32(patientJourney.Status_Master_Id);
                        journeyrecordtemp.Approved_By = patientJourney.Approved_By;
                        journeyrecordtemp.Approved_Date = patientJourney.Approved_Date;
                        journeyrecordtemp.Modified_By = patientJourney.Modified_By;
                        journeyrecordtemp.Modified_Date = patientJourney.Modified_Date;
                    }
                    entity.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            {
                otherapproved = null;
                return 0;
            }
        }

        public static Int32? ReorderStage(int[] newOrder, int[] oldOrder, int JourneyId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var stagerecord = entity.Patient_Journey_Stages.Where(s => s.Patient_Journey_Id == JourneyId).OrderBy(x => x.Stage_Display_Order).ToList();
                    int[] oldstagerecord = new int[stagerecord.Count];
                    for (int i = 0; i < oldstagerecord.Length; i++)
                    {
                        oldstagerecord[i] = Convert.ToInt32(stagerecord[i].Stage_Display_Order);
                    }
                    for (int j = 0; j < oldOrder.Length; j++)
                    {
                        for (int i = 0; i < oldstagerecord.Length; i++)
                        {
                            if (oldstagerecord[i] == oldOrder[j])
                            {
                                stagerecord[i].Stage_Display_Order = newOrder[j];
                            }
                        }
                    }
                    entity.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? AddJourneyStage(Patient_Journey_Stages patientJourneyStage)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {

                    entity.Patient_Journey_Stages.Add(patientJourneyStage);
                    entity.SaveChanges();
                    return patientJourneyStage.Patient_Journey_Stages_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? UpdateJourneyStage(Patient_Journey_Stages patientJourneyStage)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var JourneyStagesList = from r in entity.Patient_Journey_Stages select r;
                    var currentstage = entity.Patient_Journey_Stages.Where(s => s.Patient_Journey_Stages_Id == patientJourneyStage.Patient_Journey_Stages_Id).FirstOrDefault();
                    int? currentDisplayOrder = currentstage.Stage_Display_Order;
                    if (patientJourneyStage.Stage_Display_Order != null)
                    {
                        var orderedData = entity.Patient_Journey_Stages.Where(x => x.Patient_Journey_Id == patientJourneyStage.Patient_Journey_Id).OrderBy(x => x.Stage_Display_Order).ToList();

                        string Order = (currentstage.Stage_Display_Order < patientJourneyStage.Stage_Display_Order) ? "INCREASED" : "DECREASED";

                        var filteredOrderedData = (currentstage.Stage_Display_Order < patientJourneyStage.Stage_Display_Order) ?
                            orderedData.Where(x => x.Stage_Display_Order >= currentstage.Stage_Display_Order && x.Stage_Display_Order <= patientJourneyStage.Stage_Display_Order).ToList()
                            : orderedData.Where(x => x.Stage_Display_Order <= currentstage.Stage_Display_Order && x.Stage_Display_Order >= patientJourneyStage.Stage_Display_Order).ToList();

                        if (Order == "INCREASED")
                        {
                            for (int i = 0; i < filteredOrderedData.Count; i++)
                            {
                                int currentStageId = filteredOrderedData[i].Patient_Journey_Stages_Id;
                                var objData = JourneyStagesList.Where(x => x.Patient_Journey_Stages_Id == currentStageId).FirstOrDefault();

                                if (objData.Stage_Display_Order == currentDisplayOrder)
                                {
                                    objData.Stage_Display_Order = patientJourneyStage.Stage_Display_Order;
                                    objData.Modified_Date = DateTime.Now;
                                    entity.Patient_Journey_Stages.Add(objData);
                                    entity.Entry(objData).State = EntityState.Modified;
                                }
                                else
                                {
                                    objData.Stage_Display_Order = objData.Stage_Display_Order - 1;
                                    objData.Modified_Date = DateTime.Now;
                                    entity.Patient_Journey_Stages.Add(objData);
                                    entity.Entry(objData).State = EntityState.Modified;
                                }
                            }
                        }

                        if (Order == "DECREASED")
                        {
                            for (int i = 0; i < filteredOrderedData.Count; i++)
                            {
                                int currentStageId = filteredOrderedData[i].Patient_Journey_Stages_Id;
                                var objData = JourneyStagesList.Where(x => x.Patient_Journey_Stages_Id == currentStageId).FirstOrDefault();

                                if (objData.Stage_Display_Order == currentDisplayOrder)
                                {
                                    objData.Stage_Display_Order = patientJourneyStage.Stage_Display_Order;
                                    objData.Modified_Date = DateTime.Now;
                                    entity.Patient_Journey_Stages.Add(objData);
                                    entity.Entry(objData).State = EntityState.Modified;
                                }
                                else
                                {
                                    objData.Stage_Display_Order = objData.Stage_Display_Order + 1;
                                    objData.Modified_Date = DateTime.Now;
                                    entity.Patient_Journey_Stages.Add(objData);
                                    entity.Entry(objData).State = EntityState.Modified;
                                }
                            }
                        }

                        //for (int i = 0; i < filteredOrderedData.Count; i++)
                        //{
                        //    int currentStageId = filteredOrderedData[i].Patient_Journey_Stages_Id;
                        //    var objData = JourneyStagesList.Where(x => x.Patient_Journey_Stages_Id == currentStageId).FirstOrDefault();

                        //    if (objData.Stage_Display_Order == currentDisplayOrder)
                        //    {
                        //        objData.Stage_Display_Order = patientJourneyStage.Stage_Display_Order;
                        //        objData.Modified_Date = DateTime.Now;
                        //        entity.Patient_Journey_Stages.Add(objData);
                        //        entity.Entry(objData).State = EntityState.Modified;
                        //    }
                        //    else if (objData.Stage_Display_Order > patientJourneyStage.Stage_Display_Order)
                        //    {
                        //        objData.Stage_Display_Order = objData.Stage_Display_Order + 1;
                        //        objData.Modified_Date = DateTime.Now;
                        //        entity.Patient_Journey_Stages.Add(objData);
                        //        entity.Entry(objData).State = EntityState.Modified;
                        //    }
                        //    else if (objData.Stage_Display_Order < patientJourneyStage.Stage_Display_Order)
                        //    {
                        //        objData.Stage_Display_Order = objData.Stage_Display_Order - 1;
                        //        objData.Modified_Date = DateTime.Now;
                        //        entity.Patient_Journey_Stages.Add(objData);
                        //        entity.Entry(objData).State = EntityState.Modified;
                        //    }
                        //}
                        entity.SaveChanges();
                        currentstage.Stage_Display_Order = patientJourneyStage.Stage_Display_Order;
                    }
                    currentstage.Time_Statistics = patientJourneyStage.Time_Statistics;
                    currentstage.Population_Statistics = patientJourneyStage.Population_Statistics;
                    currentstage.Modified_By = patientJourneyStage.Modified_By;
                    currentstage.Modified_Date = patientJourneyStage.Modified_Date;
                    entity.SaveChanges();
                    return currentstage.Patient_Journey_Stages_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static Int32? RemoveJourneyStage(int JourneyId, int StageId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var journeystageid = entity.Patient_Journey_Stages.Where(x => x.Patient_Journey_Id == JourneyId && x.Patient_Journey_Stages_Id == StageId).FirstOrDefault();//current stage
                    var orderedStage = entity.Patient_Journey_Stages.Where(x => x.Patient_Journey_Id == JourneyId).OrderBy(x => x.Stage_Display_Order).ToList();
                    var transactionid = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Id == JourneyId && x.Patient_Journey_Stages_Id == StageId).ToList();
                    if (transactionid.Count > 0)
                    {
                        for (int i = 0; i < transactionid.Count; i++)
                        {
                            var transid = transactionid[i].Patient_Journey_Transactions_Id;
                            var asscostid = entity.Patient_Journey_Transactions_AssociatedCosts.Where(x => x.Patient_Journey_Transactions_Id == transid).ToList();
                            if (asscostid.Count > 0)
                            {
                                for (int k = 0; k < asscostid.Count; k++)
                                {
                                    entity.Patient_Journey_Transactions_AssociatedCosts.Remove(asscostid[k]);
                                }
                            }
                            var desiredid = entity.Patient_Journey_Transactions_DesiredOutcomes.Where(x => x.Patient_Journey_Transactions_Id == transid).ToList();
                            if (desiredid.Count > 0)
                            {
                                for (int k = 0; k < desiredid.Count; k++)
                                {
                                    entity.Patient_Journey_Transactions_DesiredOutcomes.Remove(desiredid[k]);
                                }
                            }
                            var clininterid = entity.Patient_Journey_Trans_Clin_Interventions.Where(x => x.Patient_Journey_Transactions_Id == transid).ToList();
                            if (clininterid.Count > 0)
                            {
                                for (int k = 0; k < clininterid.Count; k++)
                                {
                                    int clinid = clininterid[k].Patient_Journey_Trans_Clin_Interventions_Id;
                                    var subclininterid = entity.Patient_Journey_Trans_SubClin_Interventions.Where(x => x.Patient_Journey_Trans_Clin_Interventions_Id == clinid).ToList();
                                    if (subclininterid != null)
                                    {
                                        for (int l = 0; l < subclininterid.Count; l++)
                                        {
                                            entity.Patient_Journey_Trans_SubClin_Interventions.Remove(subclininterid[l]);
                                        }
                                    }
                                    entity.Patient_Journey_Trans_Clin_Interventions.Remove(clininterid[k]);
                                }
                            }
                            entity.Patient_Journey_Transactions.Remove(transactionid[i]);
                        }
                    }
                    int count = Convert.ToInt32(journeystageid.Stage_Display_Order);
                    for (int j = count; j < orderedStage.Count; j++)
                    {
                        orderedStage[j].Stage_Display_Order = orderedStage[j].Stage_Display_Order - 1;
                    }
                    entity.Patient_Journey_Stages.Remove(journeystageid);

                    entity.SaveChanges();
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static List<Patient_Journey_Stages> GetJourneyStage(int JourneyId)
        {
            List<Patient_Journey_Stages> journeyStages = new List<Patient_Journey_Stages>();
            using (PJEntities entity = new PJEntities())
            {
                journeyStages = entity.Patient_Journey_Stages.Where(x => x.Patient_Journey_Id == JourneyId).OrderBy(x => x.Stage_Display_Order).ToList();
                return journeyStages;
            }
        }

        public static List<Patient_Journey_Stages_Temp> GetJourneyStageFromTemp(int JourneyId)
        {
            List<Patient_Journey_Stages_Temp> journeyStages = new List<Patient_Journey_Stages_Temp>();

            using (PJEntities entity = new PJEntities())
            {
                //var currentJourneyId = entity.Patient_Journey_Temp.Where(x => x.Patient_Journey_Id == JourneyId).FirstOrDefault();
                journeyStages = entity.Patient_Journey_Stages_Temp.Where(x => x.Patient_Journey_Temp_Id == JourneyId).OrderBy(x => x.Stage_Display_Order).ToList();
                return journeyStages;
            }
        }

        public static Int32? ReorderTransaction(int[] newOrder, int[] oldOrder, int StageId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var transactionrecord = entity.Patient_Journey_Transactions.Where(s => s.Patient_Journey_Stages_Id == StageId).OrderBy(x => x.Transaction_Display_Order).ToList();
                    int[] oldtransactionrecord = new int[transactionrecord.Count];
                    for (int i = 0; i < oldtransactionrecord.Length; i++)
                    {
                        oldtransactionrecord[i] = Convert.ToInt32(transactionrecord[i].Transaction_Display_Order);
                    }
                    for (int j = 0; j < oldOrder.Length; j++)
                    {
                        for (int i = 0; i < oldtransactionrecord.Length; i++)
                        {
                            if (oldtransactionrecord[i] == oldOrder[j])
                            {
                                transactionrecord[i].Transaction_Display_Order = newOrder[j];
                            }
                        }
                    }
                    entity.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? AddTransaction(Patient_Journey_Transactions patientTransaction, out int TransactionCount)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    entity.Patient_Journey_Transactions.Add(patientTransaction);
                    entity.SaveChanges();
                    TransactionCount = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Stages_Id == patientTransaction.Patient_Journey_Stages_Id).Count();
                    return patientTransaction.Patient_Journey_Transactions_Id;
                }
            }
            catch (Exception)
            {
                TransactionCount = 0;
                return 0;
            }
        }

        public static Int32? UpdateTransaction(Patient_Journey_Transactions patientTransaction, out int TransactionCount)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var currenttransaction = entity.Patient_Journey_Transactions.Where(s => s.Patient_Journey_Transactions_Id == patientTransaction.Patient_Journey_Transactions_Id).FirstOrDefault();

                    currenttransaction.Description = patientTransaction.Description;
                    currenttransaction.Transaction_Master_Id = patientTransaction.Transaction_Master_Id;
                    currenttransaction.Transaction_Title = patientTransaction.Transaction_Title;
                    currenttransaction.Transaction_Location_Master_Id = patientTransaction.Transaction_Location_Master_Id;
                    currenttransaction.Transaction_Location_Title = patientTransaction.Transaction_Location_Title;
                    currenttransaction.HCP_Rating = patientTransaction.HCP_Rating;
                    currenttransaction.Payer_Rating = patientTransaction.Payer_Rating;
                    currenttransaction.Patient_Rating = patientTransaction.Patient_Rating;
                    currenttransaction.Feasibility_Rating = patientTransaction.Feasibility_Rating;
                    currenttransaction.Viability_Rating = patientTransaction.Viability_Rating;
                    currenttransaction.HCP_Description = patientTransaction.HCP_Description;
                    currenttransaction.Payer_Description = patientTransaction.Payer_Description;
                    currenttransaction.Patient_Description = patientTransaction.Patient_Description;
                    currenttransaction.Feasibility_Description = patientTransaction.Feasibility_Description;
                    currenttransaction.Viability_Description = patientTransaction.Viability_Description;
                    currenttransaction.Modified_By = patientTransaction.Modified_By;
                    currenttransaction.Modified_Date = DateTime.Now;

                    entity.SaveChanges();
                    TransactionCount = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Stages_Id == currenttransaction.Patient_Journey_Stages_Id).Count();
                    return patientTransaction.Patient_Journey_Transactions_Id;
                }
            }
            catch (Exception)
            {
                TransactionCount = 0;
                return 0;
            }
        }

        public static List<Patient_Journey_Transactions> GetTransactions(int StageId)
        {
            List<Patient_Journey_Transactions> transactions = new List<Patient_Journey_Transactions>();
            using (PJEntities entity = new PJEntities())
            {
                transactions = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Stages_Id == StageId).OrderBy(x => x.Transaction_Display_Order).ToList();
                return transactions;
            }
        }

        public static List<Patient_Journey_Transactions_Temp> GetTransactionsFromTemp(int StageId)
        {
            List<Patient_Journey_Transactions_Temp> transactions = new List<Patient_Journey_Transactions_Temp>();
            using (PJEntities entity = new PJEntities())
            {
                transactions = entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Stages_Temp_Id == StageId).OrderBy(x => x.Transaction_Display_Order).ToList();
                return transactions;
            }
        }

        public static Int32? RemoveTransactions(List<int> TransactionIds, int StageId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {

                    for (int j = 0; j < TransactionIds.Count; j++)
                    {
                        Patient_Journey_Transactions patient_Journey_Transactions = new Patient_Journey_Transactions();
                        var orderedTransaction = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Stages_Id == StageId).OrderBy(x => x.Transaction_Display_Order).ToList();
                        patient_Journey_Transactions.Patient_Journey_Transactions_Id = TransactionIds[j];
                        var currentTransaction = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Transactions_Id == patient_Journey_Transactions.Patient_Journey_Transactions_Id).FirstOrDefault();
                        for (int i = Convert.ToInt32(currentTransaction.Transaction_Display_Order); i < orderedTransaction.Count; i++)
                        {
                            orderedTransaction[i].Transaction_Display_Order = orderedTransaction[i].Transaction_Display_Order - 1;
                        }
                        var asscostid = entity.Patient_Journey_Transactions_AssociatedCosts.Where(x => x.Patient_Journey_Transactions_Id == currentTransaction.Patient_Journey_Transactions_Id).ToList();
                        if (asscostid.Count > 0)
                        {
                            for (int k = 0; k < asscostid.Count; k++)
                            {
                                entity.Patient_Journey_Transactions_AssociatedCosts.Remove(asscostid[k]);
                            }
                        }
                        var desiredid = entity.Patient_Journey_Transactions_DesiredOutcomes.Where(x => x.Patient_Journey_Transactions_Id == currentTransaction.Patient_Journey_Transactions_Id).ToList();
                        if (desiredid.Count > 0)
                        {
                            for (int k = 0; k < desiredid.Count; k++)
                            {
                                entity.Patient_Journey_Transactions_DesiredOutcomes.Remove(desiredid[k]);
                            }
                        }
                        var clininterid = entity.Patient_Journey_Trans_Clin_Interventions.Where(x => x.Patient_Journey_Transactions_Id == currentTransaction.Patient_Journey_Transactions_Id).ToList();
                        if (clininterid.Count > 0)
                        {
                            for (int k = 0; k < clininterid.Count; k++)
                            {
                                int clinid = clininterid[k].Patient_Journey_Trans_Clin_Interventions_Id;
                                var subclininterid = entity.Patient_Journey_Trans_SubClin_Interventions.Where(x => x.Patient_Journey_Trans_Clin_Interventions_Id == clinid).ToList();
                                if (subclininterid != null)
                                {
                                    for (int l = 0; l < subclininterid.Count; l++)
                                    {
                                        entity.Patient_Journey_Trans_SubClin_Interventions.Remove(subclininterid[l]);
                                    }
                                }
                                entity.Patient_Journey_Trans_Clin_Interventions.Remove(clininterid[k]);
                            }
                        }
                        entity.Patient_Journey_Transactions.Remove(currentTransaction);
                        entity.SaveChanges();
                    }
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static List<Patient_Journey> GetCloneJourneyList(int BrandId)
        {
            List<Patient_Journey> listJourney = new List<Patient_Journey>();
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    listJourney = (from journey in entity.Patient_Journey
                                   where journey.Brand_Master_Id == BrandId && journey.Status_Master_Id == 3
                                   orderby journey.Modified_Date descending
                                   select journey).ToList();
                }
            }
            catch (Exception)
            {
            }
            return listJourney;
        }

        public static Patient_Journey GetJourneyDetails(int JourneyId)
        {
            Patient_Journey journey = new Patient_Journey();
            using (PJEntities entity = new PJEntities())
            {
                journey = entity.Patient_Journey.Where(x => x.Patient_Journey_Id == JourneyId).FirstOrDefault();
                return journey;
            }
        }

        public static Patient_Journey_Temp GetTempJourneyDetails(int JourneyId)
        {
            Patient_Journey_Temp journey = new Patient_Journey_Temp();
            using (PJEntities entity = new PJEntities())
            {
                journey = entity.Patient_Journey_Temp.Where(x => x.Patient_Journey_Id == JourneyId).FirstOrDefault();
                return journey;
            }
        }

        public static Patient_Journey_Transactions_DesiredOutcomes GetDesiredOutcome(int TransactionId)
        {
            Patient_Journey_Transactions_DesiredOutcomes desiredOut = new Patient_Journey_Transactions_DesiredOutcomes();
            using (PJEntities entity = new PJEntities())
            {
                desiredOut = entity.Patient_Journey_Transactions_DesiredOutcomes.Where(x => x.Patient_Journey_Transactions_Id == TransactionId).FirstOrDefault();
                return desiredOut;
            }
        }

        public static Patient_Journey_Trans_Clin_Interventions GetClinicalIntervention(int TransactionId)
        {
            Patient_Journey_Trans_Clin_Interventions clinInt = new Patient_Journey_Trans_Clin_Interventions();
            using (PJEntities entity = new PJEntities())
            {
                clinInt = entity.Patient_Journey_Trans_Clin_Interventions.Where(x => x.Patient_Journey_Transactions_Id == TransactionId).FirstOrDefault();
                return clinInt;
            }
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

        public static Patient_Journey_Transactions_AssociatedCosts GetAssociatedCost(int TransactionId)
        {
            Patient_Journey_Transactions_AssociatedCosts assCost = new Patient_Journey_Transactions_AssociatedCosts();
            using (PJEntities entity = new PJEntities())
            {
                assCost = entity.Patient_Journey_Transactions_AssociatedCosts.Where(x => x.Patient_Journey_Transactions_Id == TransactionId).FirstOrDefault();
                return assCost;
            }
        }

        public static List<Patient_Journey_VersionDetails_Temp> GetVersionDetails(int JourneyId)
        {
            List<Patient_Journey_VersionDetails_Temp> listVersion = new List<Patient_Journey_VersionDetails_Temp>();
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    listVersion = entity.Patient_Journey_VersionDetails_Temp.Where(x => x.Patient_Journey_Temp_Id == JourneyId).OrderByDescending(x => x.Created_Date).ToList();
                }
            }
            catch (Exception)
            {
            }
            return listVersion;
        }

        public static Int32? AddVersionDetails(Patient_Journey_VersionDetails versionDetails)
        {
            using (PJEntities entity = new PJEntities())
            {
                entity.Patient_Journey_VersionDetails.Add(versionDetails);
                entity.SaveChanges();
                return versionDetails.Patient_Journey_VersionDetails_Id;
            }
        }

        public static List<Clinical_Intervention_Master> GetClinicalInterventionDetails(int? ClinicalInterventionMasterId)
        {
            using (PJEntities entity = new PJEntities())
            {
                var clinInt = entity.Clinical_Intervention_Master.Where(x => x.Clinical_Intervention_Master_Id == ClinicalInterventionMasterId).ToList();
                return clinInt;
            }
        }

        public static string GetImagePath(int? ImageMasterId)
        {
            using (PJEntities entity = new PJEntities())
            {
                string path = entity.Image_Master.Where(x => x.Image_Master_Id == ImageMasterId).Select(x => x.Image_Path).FirstOrDefault().ToString();
                return path;
            }
        }

        public static int? GetImageMasterID(int TransactionMasterId)
        {
            using (PJEntities entity = new PJEntities())
            {
                int? imageMasterId = entity.Transaction_Master.Where(x => x.Transaction_Master_Id == TransactionMasterId).Select(x => x.Image_Master_Id).FirstOrDefault();
                return imageMasterId;
            }
        }

        public static int? GetLocationImageMasterID(int LocationMasterId)
        {
            using (PJEntities entity = new PJEntities())
            {
                int? imageMasterId = entity.Transaction_Location_Master.Where(x => x.Transaction_Location_Master_Id == LocationMasterId).Select(x => x.Image_Master_Id).FirstOrDefault();
                return imageMasterId;
            }
        }

        public static List<StrategicMomentAll> GetStrategicMoment(int JourneyId)
        {
            List<StrategicMomentAll> listMoment = new List<StrategicMomentAll>();
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var strategicMomentMaster = from r in entity.Patient_Journey_Strategic_Moment select r;
                    var journeyStages = from r in entity.Patient_Journey_Stages select r;
                    var journeyTransactions = from r in entity.Patient_Journey_Transactions select r;

                    listMoment = (from kd in strategicMomentMaster
                                  join kp in journeyStages on kd.Patient_Journey_Start_Stage_Id equals kp.Patient_Journey_Stages_Id
                                  join ks in journeyTransactions on kd.Patient_Journey_Start_Transaction_Id equals ks.Patient_Journey_Transactions_Id
                                  where kd.Patient_Journey_Id == JourneyId
                                  select new StrategicMomentAll()
                                                         {
                                                             JourneyId = kd.Patient_Journey_Id,
                                                             StartStage = kd.Patient_Journey_Start_Stage_Id,
                                                             EndStage = kd.Patient_Journey_End_Stage_Id,
                                                             StartTransaction = kd.Patient_Journey_Start_Transaction_Id,
                                                             EndTransaction = kd.Patient_Journey_End_Transaction_Id,
                                                             SMOMCategory = kd.Category,
                                                             SMOMDescription = kd.Description,
                                                             Stage_Display_Order = kp.Stage_Display_Order,
                                                             Transaction_Display_Order = ks.Transaction_Display_Order
                                                         }).OrderBy(x => x.Stage_Display_Order).ToList();
                }
            }
            catch (Exception)
            {
            }
            return listMoment;
        }

        public static Int32? AddClinicalIntervention(Patient_Journey_Trans_Clin_Interventions clinicalIntervention)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    entity.Patient_Journey_Trans_Clin_Interventions.Add(clinicalIntervention);
                    entity.SaveChanges();
                    return clinicalIntervention.Patient_Journey_Trans_Clin_Interventions_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? AddSubClinicalIntervention(List<Patient_Journey_Trans_SubClin_Interventions> subclinicalIntervention)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    Patient_Journey_Trans_SubClin_Interventions subClinical = null;
                    if (subclinicalIntervention != null)
                    {
                        for (int i = 0; i < subclinicalIntervention.Count; i++)
                        {
                            subClinical = new Patient_Journey_Trans_SubClin_Interventions();
                            subClinical.Patient_Journey_Trans_Clin_Interventions_Id = subclinicalIntervention[i].Patient_Journey_Trans_Clin_Interventions_Id;
                            subClinical.SubClinical_Intervention_Master_Id = subclinicalIntervention[i].SubClinical_Intervention_Master_Id;
                            subClinical.Created_By = subclinicalIntervention[i].Created_By;
                            subClinical.Created_Date = subclinicalIntervention[i].Created_Date;
                            subClinical.Modified_By = subclinicalIntervention[i].Modified_By;
                            subClinical.Modified_Date = subclinicalIntervention[i].Modified_Date;
                            entity.Patient_Journey_Trans_SubClin_Interventions.Add(subClinical);
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

        public static Int32? AddAssociatedCost(Patient_Journey_Transactions_AssociatedCosts associatedCost)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    entity.Patient_Journey_Transactions_AssociatedCosts.Add(associatedCost);
                    entity.SaveChanges();
                    return associatedCost.Patient_Journey_Transactions_AssociatedCosts_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static Int32? AddDesiredOutcome(Patient_Journey_Transactions_DesiredOutcomes desiredOutcome)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    entity.Patient_Journey_Transactions_DesiredOutcomes.Add(desiredOutcome);
                    entity.SaveChanges();
                    return desiredOutcome.Patient_Journey_Transactions_DesiredOutcomes_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static Int32? AddJourneyToTemp(Patient_Journey_Temp JourneyDetails)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    entity.Patient_Journey_Temp.Add(JourneyDetails);
                    entity.SaveChanges();
                    return JourneyDetails.Patient_Journey_Temp_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? AddStageToTemp(Patient_Journey_Stages_Temp StageDetails)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    entity.Patient_Journey_Stages_Temp.Add(StageDetails);
                    entity.SaveChanges();
                    return StageDetails.Patient_Journey_Stages_Temp_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? ReorderStageToTemp(int[] newOrder, int[] oldOrder, int JourneyId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var stagerecord = entity.Patient_Journey_Stages_Temp.Where(s => s.Patient_Journey_Temp_Id == JourneyId).OrderBy(x => x.Stage_Display_Order).ToList();
                    var strategic = entity.Patient_Journey_Strategic_Moment_Temp.Where(x => x.Patient_Journey_Temp_Id == JourneyId).ToList();
                    bool deleteFlag = false;

                    for (int i = 0; i < stagerecord.Count; i++)
                    {
                        if (stagerecord[i].Stage_Display_Order != oldOrder[i])
                        {
                            for (int j = 0; j < strategic.Count; j++)
                            {
                                if (strategic[j].Patient_Journey_Start_Stage_Temp_Id == stagerecord[i].Patient_Journey_Stages_Temp_Id || strategic[j].Patient_Journey_End_Stage_Temp_Id == stagerecord[i].Patient_Journey_Stages_Temp_Id)
                                {
                                    deleteFlag = true;
                                }
                            }
                        }
                    }
                    if (deleteFlag == false)
                    {
                        int[] oldstagerecord = new int[stagerecord.Count];
                        for (int i = 0; i < oldstagerecord.Length; i++)
                        {
                            oldstagerecord[i] = Convert.ToInt32(stagerecord[i].Stage_Display_Order);
                        }
                        for (int j = 0; j < oldOrder.Length; j++)
                        {
                            for (int i = 0; i < oldstagerecord.Length; i++)
                            {
                                if (oldstagerecord[i] == oldOrder[j])
                                {
                                    stagerecord[i].Stage_Display_Order = newOrder[j];
                                }
                            }
                        }
                        entity.SaveChanges();
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

        public static Int32? UpdateJourneyStageToTemp(Patient_Journey_Stages_Temp patientJourneyStage)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var JourneyStagesList = from r in entity.Patient_Journey_Stages_Temp select r;
                    var currentstage = entity.Patient_Journey_Stages_Temp.Where(s => s.Patient_Journey_Stages_Temp_Id == patientJourneyStage.Patient_Journey_Stages_Temp_Id).FirstOrDefault();
                    int? currentDisplayOrder = currentstage.Stage_Display_Order;
                    if (patientJourneyStage.Stage_Display_Order != null && patientJourneyStage.Stage_Display_Order != 0)
                    {
                        var orderedData = entity.Patient_Journey_Stages_Temp.Where(x => x.Patient_Journey_Temp_Id == patientJourneyStage.Patient_Journey_Temp_Id).OrderBy(x => x.Stage_Display_Order).ToList();

                        string Order = (currentstage.Stage_Display_Order < patientJourneyStage.Stage_Display_Order) ? "INCREASED" : "DECREASED";

                        var filteredOrderedData = (currentstage.Stage_Display_Order < patientJourneyStage.Stage_Display_Order) ?
                            orderedData.Where(x => x.Stage_Display_Order >= currentstage.Stage_Display_Order && x.Stage_Display_Order <= patientJourneyStage.Stage_Display_Order).ToList()
                            : orderedData.Where(x => x.Stage_Display_Order <= currentstage.Stage_Display_Order && x.Stage_Display_Order >= patientJourneyStage.Stage_Display_Order).ToList();

                        if (Order == "INCREASED")
                        {
                            for (int i = 0; i < filteredOrderedData.Count; i++)
                            {
                                int currentStageId = filteredOrderedData[i].Patient_Journey_Stages_Temp_Id;
                                var objData = JourneyStagesList.Where(x => x.Patient_Journey_Stages_Temp_Id == currentStageId).FirstOrDefault();

                                if (objData.Stage_Display_Order == currentDisplayOrder)
                                {
                                    objData.Stage_Display_Order = patientJourneyStage.Stage_Display_Order;
                                    objData.Modified_Date = DateTime.Now;
                                    entity.Patient_Journey_Stages_Temp.Add(objData);
                                    entity.Entry(objData).State = EntityState.Modified;
                                }
                                else
                                {
                                    objData.Stage_Display_Order = objData.Stage_Display_Order - 1;
                                    objData.Modified_Date = DateTime.Now;
                                    entity.Patient_Journey_Stages_Temp.Add(objData);
                                    entity.Entry(objData).State = EntityState.Modified;
                                }
                            }
                        }

                        if (Order == "DECREASED")
                        {
                            for (int i = 0; i < filteredOrderedData.Count; i++)
                            {
                                int currentStageId = filteredOrderedData[i].Patient_Journey_Stages_Temp_Id;
                                var objData = JourneyStagesList.Where(x => x.Patient_Journey_Stages_Temp_Id == currentStageId).FirstOrDefault();

                                if (objData.Stage_Display_Order == currentDisplayOrder)
                                {
                                    objData.Stage_Display_Order = patientJourneyStage.Stage_Display_Order;
                                    objData.Modified_Date = DateTime.Now;
                                    entity.Patient_Journey_Stages_Temp.Add(objData);
                                    entity.Entry(objData).State = EntityState.Modified;
                                }
                                else
                                {
                                    objData.Stage_Display_Order = objData.Stage_Display_Order + 1;
                                    objData.Modified_Date = DateTime.Now;
                                    entity.Patient_Journey_Stages_Temp.Add(objData);
                                    entity.Entry(objData).State = EntityState.Modified;
                                }
                            }
                        }
                        entity.SaveChanges();
                        currentstage.Stage_Display_Order = patientJourneyStage.Stage_Display_Order;
                    }
                    currentstage.Time_Statistics = patientJourneyStage.Time_Statistics;
                    currentstage.Population_Statistics = patientJourneyStage.Population_Statistics;
                    currentstage.Modified_By = patientJourneyStage.Modified_By;
                    currentstage.Modified_Date = patientJourneyStage.Modified_Date;
                    entity.SaveChanges();
                    return currentstage.Patient_Journey_Stages_Temp_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static Int32? RemoveJourneyStageToTemp(int JourneyId, int StageId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var strategicdata = entity.Patient_Journey_Strategic_Moment_Temp.Where(x => x.Patient_Journey_Temp_Id == JourneyId).ToList();
                    bool deleteFlag = false;
                    for (int i = 0; i < strategicdata.Count; i++)
                    {
                        if (strategicdata[i].Patient_Journey_End_Stage_Temp_Id == StageId || strategicdata[i].Patient_Journey_Start_Stage_Temp_Id == StageId)
                        {
                            deleteFlag = true;
                        }
                    }
                    if (deleteFlag == false)
                    {
                        var journeystageid = entity.Patient_Journey_Stages_Temp.Where(x => x.Patient_Journey_Temp_Id == JourneyId && x.Patient_Journey_Stages_Temp_Id == StageId).FirstOrDefault();//current stage
                        var orderedStage = entity.Patient_Journey_Stages_Temp.Where(x => x.Patient_Journey_Temp_Id == JourneyId).OrderBy(x => x.Stage_Display_Order).ToList();
                        var transactionid = entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Temp_Id == JourneyId && x.Patient_Journey_Stages_Temp_Id == StageId).ToList();
                        if (transactionid.Count > 0)
                        {
                            for (int i = 0; i < transactionid.Count; i++)
                            {
                                var transid = transactionid[i].Patient_Journey_Transactions_Temp_Id;
                                var asscostid = entity.Patient_Journey_Transactions_AssociatedCosts_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == transid).ToList();
                                if (asscostid.Count > 0)
                                {
                                    for (int k = 0; k < asscostid.Count; k++)
                                    {
                                        entity.Patient_Journey_Transactions_AssociatedCosts_Temp.Remove(asscostid[k]);
                                    }
                                }
                                var desiredid = entity.Patient_Journey_Transactions_DesiredOutcomes_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == transid).ToList();
                                if (desiredid.Count > 0)
                                {
                                    for (int k = 0; k < desiredid.Count; k++)
                                    {
                                        entity.Patient_Journey_Transactions_DesiredOutcomes_Temp.Remove(desiredid[k]);
                                    }
                                }
                                var clininterid = entity.Patient_Journey_Trans_Clin_Interventions_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == transid).ToList();
                                if (clininterid.Count > 0)
                                {
                                    for (int k = 0; k < clininterid.Count; k++)
                                    {
                                        int clinid = clininterid[k].Patient_Journey_Trans_Clin_Interventions_Temp_Id;
                                        var subclininterid = entity.Patient_Journey_Trans_SubClin_Interventions_Temp.Where(x => x.Patient_Journey_Trans_Clin_Interventions_Temp_Id == clinid).ToList();
                                        if (subclininterid != null)
                                        {
                                            for (int l = 0; l < subclininterid.Count; l++)
                                            {
                                                entity.Patient_Journey_Trans_SubClin_Interventions_Temp.Remove(subclininterid[l]);
                                            }
                                        }
                                        entity.Patient_Journey_Trans_Clin_Interventions_Temp.Remove(clininterid[k]);
                                    }
                                }
                                entity.Patient_Journey_Transactions_Temp.Remove(transactionid[i]);
                            }
                        }
                        int count = Convert.ToInt32(journeystageid.Stage_Display_Order);
                        for (int j = count; j < orderedStage.Count; j++)
                        {
                            orderedStage[j].Stage_Display_Order = orderedStage[j].Stage_Display_Order - 1;
                        }
                        entity.Patient_Journey_Stages_Temp.Remove(journeystageid);

                        entity.SaveChanges();
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

        public static Int32? AddTransactionToTemp(Patient_Journey_Transactions_Temp TransactionDetails, out int TransactionCount)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    entity.Patient_Journey_Transactions_Temp.Add(TransactionDetails);
                    entity.SaveChanges();
                    TransactionCount = entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Stages_Temp_Id == TransactionDetails.Patient_Journey_Stages_Temp_Id).Count();
                    return TransactionDetails.Patient_Journey_Transactions_Temp_Id;
                }
            }
            catch (Exception)
            {
                TransactionCount = 0;
                return 0;
            }
        }

        public static Int32? ReorderTransactionToTemp(int[] newOrder, int[] oldOrder, int StageId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var transactionrecord = entity.Patient_Journey_Transactions_Temp.Where(s => s.Patient_Journey_Stages_Temp_Id == StageId).OrderBy(x => x.Transaction_Display_Order).ToList();
                    var strategic = entity.Patient_Journey_Strategic_Moment_Temp.ToList();
                    bool deleteFlag = false;

                    for (int i = 0; i < transactionrecord.Count; i++)
                    {
                        if (transactionrecord[i].Transaction_Display_Order != oldOrder[i])
                        {
                            for (int j = 0; j < strategic.Count; j++)
                            {
                                if (strategic[j].Patient_Journey_Start_Transaction_Temp_Id == transactionrecord[i].Patient_Journey_Transactions_Temp_Id || strategic[j].Patient_Journey_End_Transaction_Temp_Id == transactionrecord[i].Patient_Journey_Transactions_Temp_Id)
                                {
                                    deleteFlag = true;
                                }
                            }
                        }
                    }
                    if (deleteFlag == false)
                    {
                        int[] oldtransactionrecord = new int[transactionrecord.Count];
                        for (int i = 0; i < oldtransactionrecord.Length; i++)
                        {
                            oldtransactionrecord[i] = Convert.ToInt32(transactionrecord[i].Transaction_Display_Order);
                        }
                        for (int j = 0; j < oldOrder.Length; j++)
                        {
                            for (int i = 0; i < oldtransactionrecord.Length; i++)
                            {
                                if (oldtransactionrecord[i] == oldOrder[j])
                                {
                                    transactionrecord[i].Transaction_Display_Order = newOrder[j];
                                }
                            }
                        }
                        entity.SaveChanges();
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

        public static Int32? UpdateTransactionToTemp(Patient_Journey_Transactions_Temp patientTransaction, out int TransactionCount)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var currenttransaction = entity.Patient_Journey_Transactions_Temp.Where(s => s.Patient_Journey_Transactions_Temp_Id == patientTransaction.Patient_Journey_Transactions_Temp_Id).FirstOrDefault();

                    currenttransaction.Description = patientTransaction.Description;
                    currenttransaction.Transaction_Master_Id = patientTransaction.Transaction_Master_Id;
                    currenttransaction.Transaction_Title = patientTransaction.Transaction_Title;
                    currenttransaction.Transaction_Location_Master_Id = patientTransaction.Transaction_Location_Master_Id;
                    currenttransaction.Transaction_Location_Title = patientTransaction.Transaction_Location_Title;
                    currenttransaction.HCP_Rating = patientTransaction.HCP_Rating;
                    currenttransaction.Payer_Rating = patientTransaction.Payer_Rating;
                    currenttransaction.Patient_Rating = patientTransaction.Patient_Rating;
                    currenttransaction.Feasibility_Rating = patientTransaction.Feasibility_Rating;
                    currenttransaction.Viability_Rating = patientTransaction.Viability_Rating;
                    currenttransaction.HCP_Description = patientTransaction.HCP_Description;
                    currenttransaction.Payer_Description = patientTransaction.Payer_Description;
                    currenttransaction.Patient_Description = patientTransaction.Patient_Description;
                    currenttransaction.Feasibility_Description = patientTransaction.Feasibility_Description;
                    currenttransaction.Viability_Description = patientTransaction.Viability_Description;
                    currenttransaction.Modified_By = patientTransaction.Modified_By;
                    currenttransaction.Modified_Date = DateTime.Now;

                    entity.SaveChanges();
                    TransactionCount = entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Stages_Temp_Id == currenttransaction.Patient_Journey_Stages_Temp_Id).Count();
                    return currenttransaction.Patient_Journey_Transactions_Temp_Id;

                }
            }
            catch (Exception)
            {
                TransactionCount = 0;
                return 0;
            }
        }

        public static Int32? RemoveTransactionsToTemp(List<int> TransactionIds, int StageId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    bool deleteFlag = false;
                    var strategicdata = entity.Patient_Journey_Strategic_Moment_Temp.ToList();
                    for (int i = 0; i < TransactionIds.Count; i++)
                    {
                        for (int j = 0; j < strategicdata.Count; j++)
                        {
                            if (TransactionIds[i] == strategicdata[j].Patient_Journey_End_Transaction_Temp_Id || TransactionIds[i] == strategicdata[j].Patient_Journey_Start_Transaction_Temp_Id)
                            {
                                deleteFlag = true;
                            }
                        }
                    }
                    if (deleteFlag == false)
                    {
                        for (int j = 0; j < TransactionIds.Count; j++)
                        {
                            Patient_Journey_Transactions_Temp patient_Journey_Transactions = new Patient_Journey_Transactions_Temp();
                            var orderedTransaction = entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Stages_Temp_Id == StageId).OrderBy(x => x.Transaction_Display_Order).ToList();
                            patient_Journey_Transactions.Patient_Journey_Transactions_Temp_Id = TransactionIds[j];
                            var currentTransaction = entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == patient_Journey_Transactions.Patient_Journey_Transactions_Temp_Id).FirstOrDefault();
                            for (int i = Convert.ToInt32(currentTransaction.Transaction_Display_Order); i < orderedTransaction.Count; i++)
                            {
                                orderedTransaction[i].Transaction_Display_Order = orderedTransaction[i].Transaction_Display_Order - 1;
                            }
                            var asscostid = entity.Patient_Journey_Transactions_AssociatedCosts_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == currentTransaction.Patient_Journey_Transactions_Temp_Id).ToList();
                            if (asscostid.Count > 0)
                            {
                                for (int k = 0; k < asscostid.Count; k++)
                                {
                                    entity.Patient_Journey_Transactions_AssociatedCosts_Temp.Remove(asscostid[k]);
                                }
                            }
                            var desiredid = entity.Patient_Journey_Transactions_DesiredOutcomes_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == currentTransaction.Patient_Journey_Transactions_Temp_Id).ToList();
                            if (desiredid.Count > 0)
                            {
                                for (int k = 0; k < desiredid.Count; k++)
                                {
                                    entity.Patient_Journey_Transactions_DesiredOutcomes_Temp.Remove(desiredid[k]);
                                }
                            }
                            var clininterid = entity.Patient_Journey_Trans_Clin_Interventions_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == currentTransaction.Patient_Journey_Transactions_Temp_Id).ToList();
                            if (clininterid.Count > 0)
                            {
                                for (int k = 0; k < clininterid.Count; k++)
                                {
                                    int clinid = clininterid[k].Patient_Journey_Trans_Clin_Interventions_Temp_Id;
                                    var subclininterid = entity.Patient_Journey_Trans_SubClin_Interventions_Temp.Where(x => x.Patient_Journey_Trans_Clin_Interventions_Temp_Id == clinid).ToList();
                                    if (subclininterid != null)
                                    {
                                        for (int l = 0; l < subclininterid.Count; l++)
                                        {
                                            entity.Patient_Journey_Trans_SubClin_Interventions_Temp.Remove(subclininterid[l]);
                                        }
                                    }
                                    entity.Patient_Journey_Trans_Clin_Interventions_Temp.Remove(clininterid[k]);
                                }
                            }
                            entity.Patient_Journey_Transactions_Temp.Remove(currentTransaction);
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

        public static Int32? AddVersionDetailsToTemp(Patient_Journey_VersionDetails_Temp versionDetails)
        {
            using (PJEntities entity = new PJEntities())
            {
                var currentJourney = entity.Patient_Journey_Temp.Where(x => x.Patient_Journey_Id == versionDetails.Patient_Journey_Temp_Id);
                entity.Patient_Journey_VersionDetails_Temp.Add(versionDetails);
                entity.SaveChanges();
                return versionDetails.Patient_Journey_VersionDetails_Temp_Id;
            }
        }

        public static Int32? RemoveJourneyFromTemp(int JourneyId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var journeyid = entity.Patient_Journey_Temp.Where(x => x.Patient_Journey_Id == JourneyId).FirstOrDefault();
                    if (journeyid != null)
                    {
                        var stageid = entity.Patient_Journey_Stages_Temp.Where(x => x.Patient_Journey_Temp_Id == journeyid.Patient_Journey_Temp_Id).ToList();
                        if (stageid.Count > 0)
                        {
                            for (int i = 0; i < stageid.Count; i++)
                            {
                                var transactionid = entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Temp_Id == journeyid.Patient_Journey_Temp_Id).ToList();

                                if (transactionid.Count > 0)
                                {
                                    for (int j = 0; j < transactionid.Count; j++)
                                    {
                                        var transid = transactionid[j].Patient_Journey_Transactions_Temp_Id;
                                        var asscostid = entity.Patient_Journey_Transactions_AssociatedCosts_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == transid).ToList();
                                        if (asscostid.Count > 0)
                                        {
                                            for (int k = 0; k < asscostid.Count; k++)
                                            {
                                                entity.Patient_Journey_Transactions_AssociatedCosts_Temp.Remove(asscostid[k]);
                                            }
                                        }
                                        var desiredid = entity.Patient_Journey_Transactions_DesiredOutcomes_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == transid).ToList();
                                        if (desiredid.Count > 0)
                                        {
                                            for (int k = 0; k < desiredid.Count; k++)
                                            {
                                                entity.Patient_Journey_Transactions_DesiredOutcomes_Temp.Remove(desiredid[k]);
                                            }
                                        }
                                        var clininterid = entity.Patient_Journey_Trans_Clin_Interventions_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == transid).ToList();
                                        if (clininterid.Count > 0)
                                        {
                                            for (int k = 0; k < clininterid.Count; k++)
                                            {

                                                int clinid = clininterid[k].Patient_Journey_Trans_Clin_Interventions_Temp_Id;
                                                var subclininterid = entity.Patient_Journey_Trans_SubClin_Interventions_Temp.Where(x => x.Patient_Journey_Trans_Clin_Interventions_Temp_Id == clinid).ToList();
                                                if (subclininterid != null)
                                                {
                                                    for (int l = 0; l < subclininterid.Count; l++)
                                                    {
                                                        entity.Patient_Journey_Trans_SubClin_Interventions_Temp.Remove(subclininterid[l]);
                                                    }
                                                }
                                                entity.Patient_Journey_Trans_Clin_Interventions_Temp.Remove(clininterid[k]);
                                            }
                                        }
                                        entity.Patient_Journey_Transactions_Temp.Remove(transactionid[j]);
                                    }
                                }
                                entity.Patient_Journey_Stages_Temp.Remove(stageid[i]);
                            }
                        }
                        entity.Patient_Journey_Temp.Remove(journeyid);
                        entity.SaveChanges();
                    }
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? RemoveAllStages(int JourneyId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var strategic = entity.Patient_Journey_Strategic_Moment.Where(x => x.Patient_Journey_Id == JourneyId).ToList();
                    for (int i = 0; i < strategic.Count; i++)
                    {
                        entity.Patient_Journey_Strategic_Moment.Remove(strategic[i]);
                    }
                    var journeystages = entity.Patient_Journey_Stages.Where(x => x.Patient_Journey_Id == JourneyId).ToList();
                    var transactionid = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Id == JourneyId).ToList();
                    if (transactionid.Count > 0)
                    {
                        for (int i = 0; i < transactionid.Count; i++)
                        {
                            var transid = transactionid[i].Patient_Journey_Transactions_Id;
                            var asscostid = entity.Patient_Journey_Transactions_AssociatedCosts.Where(x => x.Patient_Journey_Transactions_Id == transid).ToList();
                            if (asscostid.Count > 0)
                            {
                                for (int k = 0; k < asscostid.Count; k++)
                                {
                                    entity.Patient_Journey_Transactions_AssociatedCosts.Remove(asscostid[k]);
                                }
                            }
                            var desiredid = entity.Patient_Journey_Transactions_DesiredOutcomes.Where(x => x.Patient_Journey_Transactions_Id == transid).ToList();
                            if (desiredid.Count > 0)
                            {
                                for (int k = 0; k < desiredid.Count; k++)
                                {
                                    entity.Patient_Journey_Transactions_DesiredOutcomes.Remove(desiredid[k]);
                                }
                            }
                            var clininterid = entity.Patient_Journey_Trans_Clin_Interventions.Where(x => x.Patient_Journey_Transactions_Id == transid).ToList();
                            if (clininterid.Count > 0)
                            {
                                for (int k = 0; k < clininterid.Count; k++)
                                {
                                    int clinid = clininterid[k].Patient_Journey_Trans_Clin_Interventions_Id;
                                    var subclininterid = entity.Patient_Journey_Trans_SubClin_Interventions.Where(x => x.Patient_Journey_Trans_Clin_Interventions_Id == clinid).ToList();
                                    if (subclininterid != null)
                                    {
                                        for (int l = 0; l < subclininterid.Count; l++)
                                        {
                                            entity.Patient_Journey_Trans_SubClin_Interventions.Remove(subclininterid[l]);
                                        }
                                    }
                                    entity.Patient_Journey_Trans_Clin_Interventions.Remove(clininterid[k]);
                                }
                            }
                            entity.Patient_Journey_Transactions.Remove(transactionid[i]);
                        }
                    }
                    if (journeystages != null)
                    {
                        for (int j = 0; j < journeystages.Count; j++)
                        {
                            entity.Patient_Journey_Stages.Remove(journeystages[j]);
                        }
                    }

                    entity.SaveChanges();
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? ToBeApprovedJourney(int JourneyId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var journey = entity.Patient_Journey_Temp.Where(x => x.Patient_Journey_Temp_Id == JourneyId).FirstOrDefault();
                    journey.Status_Master_Id = 7;
                    var oldJourney = entity.Patient_Journey.Where(x => x.Patient_Journey_Id == journey.Patient_Journey_Id).FirstOrDefault();
                    oldJourney.Status_Master_Id = 7;
                    entity.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static List<Patient_Journey_Stages> GetPatientJourneyStages()
        {
            using (PJEntities _entity = new PJEntities())
            {
                var result = _entity.Patient_Journey_Stages.ToList();
                return result;
            }
        }

        public static List<Patient_Journey_Transactions> GetPatientJourneyTransactions()
        {
            using (PJEntities _entity = new PJEntities())
            {
                var result = _entity.Patient_Journey_Transactions.ToList();
                return result;
            }
        }

        public static Int32? GetAllTransactionCount(int JourneyId)
        {
            try
            {
                int? transactionCount = 0;
                using (PJEntities entity = new PJEntities())
                {
                    transactionCount = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Id == JourneyId).Count();
                    return transactionCount;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int? GetPreviousTransactionCount(int? StageDisplayOrder, int? JourneyId)
        {
            try
            {
                int? transactionCount = 0;
                using (PJEntities entity = new PJEntities())
                {
                    List<int> query = entity.Patient_Journey_Stages.Where(x => x.Stage_Display_Order < StageDisplayOrder
                        && x.Patient_Journey_Id == JourneyId)
                                .Select(x => x.Patient_Journey_Stages_Id).ToList();

                    for (int i = 0; i < query.Count(); i++)
                    {
                        int stageId = query[i];
                        int transaction = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Stages_Id == stageId)
                            .Select(x => x.Patient_Journey_Transactions_Id).Count();
                        transactionCount += transaction;
                    }

                    return transactionCount;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int? GetWidth(int? StartStageId, int? EndStageId, int? JourneyId, int? StartTransaction, int? EndTransaction)
        {
            try
            {
                int? transactionCount = 0;
                using (PJEntities entity = new PJEntities())
                {
                    var stages = from r in entity.Patient_Journey_Stages select r;

                    int? startStageDisplayorder = stages.Where(x => x.Patient_Journey_Stages_Id == StartStageId).Select(x => x.Stage_Display_Order).FirstOrDefault();
                    int? endStageDisplayorder = stages.Where(x => x.Patient_Journey_Stages_Id == EndStageId).Select(x => x.Stage_Display_Order).FirstOrDefault();

                    List<int> query = entity.Patient_Journey_Stages.Where(x => x.Stage_Display_Order >= startStageDisplayorder && x.Stage_Display_Order <= endStageDisplayorder
                        && x.Patient_Journey_Id == JourneyId)
                                .Select(x => x.Patient_Journey_Stages_Id).ToList();

                    for (int i = 0; i < query.Count(); i++)
                    {
                        int stageId = query[i];
                        int transaction = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Stages_Id == stageId
                            && x.Patient_Journey_Transactions_Id >= StartTransaction)
                            .Select(x => x.Patient_Journey_Transactions_Id).Count();
                        transactionCount += transaction;
                    }

                    return transactionCount;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? CopySMOMToMain(int JourneyId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var mainJourney = entity.Patient_Journey_Temp.Where(x => x.Patient_Journey_Id == JourneyId).FirstOrDefault();
                    var tempstrategic = entity.Patient_Journey_Strategic_Moment_Temp.Where(x => x.Patient_Journey_Temp_Id == mainJourney.Patient_Journey_Temp_Id).ToList();
                    Patient_Journey_Strategic_Moment strategic = null;
                    for (int i = 0; i < tempstrategic.Count; i++)
                    {
                        strategic = new Patient_Journey_Strategic_Moment();
                        strategic.Title = tempstrategic[i].Title;
                        strategic.Category = tempstrategic[i].Category;
                        strategic.Created_By = tempstrategic[i].Created_By;
                        strategic.Created_Date = tempstrategic[i].Created_Date;
                        strategic.Description = tempstrategic[i].Description;
                        strategic.Modified_By = tempstrategic[i].Modified_By;
                        strategic.Modified_Date = tempstrategic[i].Modified_Date;

                        var endStage = tempstrategic[i].Patient_Journey_End_Stage_Temp_Id;
                        var startStage = tempstrategic[i].Patient_Journey_Start_Stage_Temp_Id;
                        var endTransaction = tempstrategic[i].Patient_Journey_End_Transaction_Temp_Id;
                        var startTransaction = tempstrategic[i].Patient_Journey_Start_Transaction_Temp_Id;
                        strategic.Patient_Journey_Start_Stage_Id = entity.Patient_Journey_Stages.Where(x => x.Patient_Journey_Stages_Temp_Id == startStage).FirstOrDefault().Patient_Journey_Stages_Id;
                        strategic.Patient_Journey_End_Stage_Id = entity.Patient_Journey_Stages.Where(x => x.Patient_Journey_Stages_Temp_Id == endStage).FirstOrDefault().Patient_Journey_Stages_Id;
                        strategic.Patient_Journey_Start_Transaction_Id = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Transactions_Temp_Id == startTransaction).FirstOrDefault().Patient_Journey_Transactions_Id;
                        strategic.Patient_Journey_End_Transaction_Id = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Transactions_Temp_Id == endTransaction).FirstOrDefault().Patient_Journey_Transactions_Id;


                        strategic.Patient_Journey_Id = mainJourney.Patient_Journey_Id;
                        strategic.Patient_Journey_Strategic_Moment_Temp_Id = tempstrategic[i].Patient_Journey_Strategic_Moment_Temp_Id;
                        entity.Patient_Journey_Strategic_Moment.Add(strategic);
                    }
                    entity.SaveChanges();
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? CopySMOMToTemp(int JourneyId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var tempJourneyId = entity.Patient_Journey_Temp.Where(x => x.Patient_Journey_Id == JourneyId).FirstOrDefault();
                    var mainstrategic = entity.Patient_Journey_Strategic_Moment.Where(x => x.Patient_Journey_Id == JourneyId).ToList();
                    Patient_Journey_Strategic_Moment_Temp tempstrategic = null;
                    for (int i = 0; i < mainstrategic.Count; i++)
                    {
                        tempstrategic.Category = mainstrategic[i].Category;
                        tempstrategic.Created_By = mainstrategic[i].Created_By;
                        tempstrategic.Created_Date = mainstrategic[i].Created_Date;
                        tempstrategic.Description = mainstrategic[i].Description;
                        tempstrategic.Modified_By = mainstrategic[i].Modified_By;
                        tempstrategic.Modified_Date = mainstrategic[i].Modified_Date;

                        var startStage = mainstrategic[i].Patient_Journey_Start_Stage_Id;
                        var endStage = mainstrategic[i].Patient_Journey_End_Stage_Id;
                        var startTransaction = mainstrategic[i].Patient_Journey_Start_Transaction_Id;
                        var endTransaction = mainstrategic[i].Patient_Journey_End_Transaction_Id;
                        tempstrategic.Patient_Journey_Start_Stage_Temp_Id = entity.Patient_Journey_Stages_Temp.Where(x => x.Patient_Journey_Stages_Id == startStage).FirstOrDefault().Patient_Journey_Stages_Temp_Id;
                        tempstrategic.Patient_Journey_End_Stage_Temp_Id = entity.Patient_Journey_Stages_Temp.Where(x => x.Patient_Journey_Stages_Id == endStage).FirstOrDefault().Patient_Journey_Stages_Temp_Id;
                        tempstrategic.Patient_Journey_Start_Transaction_Temp_Id = entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Transactions_Id == startTransaction).FirstOrDefault().Patient_Journey_Transactions_Temp_Id;
                        tempstrategic.Patient_Journey_End_Transaction_Temp_Id = entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Transactions_Id == endTransaction).FirstOrDefault().Patient_Journey_Transactions_Temp_Id;


                        tempstrategic.Patient_Journey_Temp_Id = tempJourneyId.Patient_Journey_Temp_Id;
                        entity.Patient_Journey_Strategic_Moment_Temp.Add(tempstrategic);
                    }
                    entity.SaveChanges();
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? UpdateTransactionTempId(int TransactionId, int newTransactionId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var tempTrans = entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Transactions_Temp_Id == TransactionId).FirstOrDefault();
                    if (tempTrans != null)
                    {
                        tempTrans.Patient_Journey_Transactions_Id = newTransactionId;
                        entity.SaveChanges();
                    }
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? UpdateStageTempId(int StageId, int newStageId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var tempStage = entity.Patient_Journey_Stages_Temp.Where(x => x.Patient_Journey_Stages_Temp_Id == StageId).FirstOrDefault();
                    if (tempStage != null)
                    {
                        tempStage.Patient_Journey_Stages_Id = newStageId;
                        entity.SaveChanges();
                    }
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static string GetUserEmailId(string user511)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    string email = entity.Users.Where(x => x.User_511 == user511).FirstOrDefault().Email_Id;
                    return email;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Int32? GetCountryId(string CountryName)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var Id = entity.Country_Master.Where(x => x.Country_Name.ToLower() == CountryName.ToLower()).FirstOrDefault();
                    return Id.Country_Master_Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static List<String> GetAllAdminsByCountry(int CountryId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var UserEmails = (from users in entity.Users
                                      join roles in entity.User_Roles
                                      on users.User_Id equals roles.User_Id
                                      join country in entity.User_Country_Association
                                      on users.User_Id equals country.User_Id
                                      where roles.Role_Master_Id == 2 && country.Country_Master_Id == CountryId && users.Is_Active == true
                                      select users.Email_Id).Distinct().ToList();
                    return UserEmails;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Int32? RemoveAllVersion(int JourneyId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    List<Patient_Journey_VersionDetails> lstVersion = new List<Patient_Journey_VersionDetails>();
                    lstVersion = entity.Patient_Journey_VersionDetails.Where(x => x.Patient_Journey_Id == JourneyId).ToList();
                    if (lstVersion != null)
                    {
                        for(int i=0;i<lstVersion.Count;i++)
                        {
                            entity.Patient_Journey_VersionDetails.Remove(lstVersion[i]);
                        }
                    }
                    entity.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? VersionApproval(int JourneyId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    List<Patient_Journey_VersionDetails_Temp> lstVersion = new List<Patient_Journey_VersionDetails_Temp>();
                    lstVersion = entity.Patient_Journey_VersionDetails_Temp.Where(x => x.Patient_Journey_Temp_Id == JourneyId).ToList();
                    if (lstVersion != null)
                    {
                        for (int i = 0; i < lstVersion.Count; i++)
                        {
                            lstVersion[i].IsApproved = true;
                        }
                    }
                    entity.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }
}
