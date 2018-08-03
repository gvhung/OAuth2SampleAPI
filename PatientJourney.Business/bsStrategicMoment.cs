using PatientJourney.BusinessModel.BuilderModels;
using PatientJourney.DataAccess.Data;
using PatientJourney.DataAccess.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientJourney.Business
{
    public class bsStrategicMoment
    {
        public static StrategicMoment GetAllStrategicMoment(string CountryId, string BrandId, string Year)
        {
            StrategicMoment lstJourney = new StrategicMoment();
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
                    lstJourney.JourneyId = result.Patient_Journey_Id;
                    List<StrategicMomentModel> strategicMoment = bsStrategicMoment.GetStrategicMoment(result.Patient_Journey_Id.ToString());
                    lstJourney.Strategic_Moment = strategicMoment;
                }
            }
            catch (Exception)
            {

            }
            return lstJourney;
        }

        public static List<StrategicMomentModel> GetStrategicMoment(string JourneyId)
        {
            List<StrategicMomentModel> listStrategicMoment = new List<StrategicMomentModel>();
            PJEntities entity = new PJEntities();
            var momentList = dbStrategicMoment.GetStrategicMoment(Convert.ToInt32(JourneyId));
            var stagesMasterList = dbStrategicMoment.GetStages();
            var transactionMasterList = dbStrategicMoment.GetTransactions();
            var journeyStagesList = dbStrategicMoment.GetJourneyStages();
            var journeyTransactionsList = dbStrategicMoment.GetJourneyTransactions();

            for (int i = 0; i < momentList.Count; i++)
            {
                StrategicMomentModel strategicMoment = new StrategicMomentModel();
                strategicMoment.Id = i + 1;
                strategicMoment.Title = momentList[i].Title;
                strategicMoment.StrategicMomentId = Convert.ToInt32(momentList[i].Patient_Journey_Strategic_Moment_Id);
                strategicMoment.StrategicMomentTempId = Convert.ToInt32(momentList[i].Patient_Journey_Strategic_Moment_Temp_Id);
                strategicMoment.StartStageId = Convert.ToInt32(momentList[i].Patient_Journey_Start_Stage_Id);
                strategicMoment.StartStageMasterId = journeyStagesList.Where(x => x.Patient_Journey_Stages_Id == strategicMoment.StartStageId).Select(x => x.Stage_Master_Id).FirstOrDefault();
                strategicMoment.StartStage = stagesMasterList.Where(x => x.Stage_Master_Id == strategicMoment.StartStageMasterId).Select(x => x.Stage_Name).FirstOrDefault().ToString();
                strategicMoment.EndStageId = Convert.ToInt32(momentList[i].Patient_Journey_End_Stage_Id);
                strategicMoment.EndStageMasterId = journeyStagesList.Where(x => x.Patient_Journey_Stages_Id == strategicMoment.EndStageId).Select(x => x.Stage_Master_Id).FirstOrDefault();
                strategicMoment.EndStage = stagesMasterList.Where(x => x.Stage_Master_Id == strategicMoment.EndStageMasterId).Select(x => x.Stage_Name).FirstOrDefault().ToString(); ;
                strategicMoment.Description = momentList[i].Description;
                strategicMoment.Category = momentList[i].Category;
                strategicMoment.StartTransactionId = momentList[i].Patient_Journey_Start_Transaction_Id;
                strategicMoment.EndTransactionId = momentList[i].Patient_Journey_End_Transaction_Id;
                strategicMoment.StartTransactionMasterId = journeyTransactionsList.Where(x => x.Patient_Journey_Transactions_Id == strategicMoment.StartTransactionId).Select(x => x.Transaction_Master_Id).FirstOrDefault();
                strategicMoment.EndTransactionMasterId = journeyTransactionsList.Where(x => x.Patient_Journey_Transactions_Id == strategicMoment.EndTransactionId).Select(x => x.Transaction_Master_Id).FirstOrDefault();
                strategicMoment.StartTransaction = transactionMasterList.Where(x => x.Transaction_Master_Id == strategicMoment.StartTransactionMasterId).Select(x => x.Transaction_Name).FirstOrDefault().ToString();
                strategicMoment.EndTransaction = transactionMasterList.Where(x => x.Transaction_Master_Id == strategicMoment.EndTransactionMasterId).Select(x => x.Transaction_Name).FirstOrDefault().ToString();
                listStrategicMoment.Add(strategicMoment);
            }
            return listStrategicMoment;
        }

        public static NewStrategicMoment GetStages(string JourneyId)
        {
            NewStrategicMoment lstMaster = new NewStrategicMoment();
            try
            {
                Patient_Journey result = new Patient_Journey();
                PJEntities entity = new PJEntities();
                if (result != null)
                {
                    List<Stages_Moment> stages = bsStrategicMoment.GetJourneyStageNames(JourneyId);
                    lstMaster.Stages_Moment = stages;
                }
            }
            catch (Exception)
            {

            }
            return lstMaster;
        }

        public static NewStrategicMoment GetEndStage(string JourneyId, string StageId)
        {
            NewStrategicMoment lstMaster = new NewStrategicMoment();
            try
            {
                Patient_Journey result = new Patient_Journey();
                PJEntities entity = new PJEntities();
                if (result != null)
                {
                    List<Stages_Moment> stages = bsStrategicMoment.GetEndStages(JourneyId, StageId);
                    lstMaster.Stages_Moment = stages;
                }
            }
            catch (Exception)
            {

            }
            return lstMaster;
        }

        public static List<Stages_Moment> GetJourneyStageNames(string JourneyId)
        {
            List<Stages_Moment> listJourneyStage = new List<Stages_Moment>();
            PJEntities entity = new PJEntities();
            var stages = dbPatientAdministration.GetJourneyStage(Convert.ToInt32(JourneyId));
            for (int i = 0; i < stages.Count; i++)
            {
                Stages_Moment journeyStage = new Stages_Moment();
                journeyStage.JourneyId = Convert.ToInt32(JourneyId);
                journeyStage.PatientStageId = stages[i].Patient_Journey_Stages_Id;
                journeyStage.StageTitle = stages[i].Stage_Title.ToUpper();
                journeyStage.StageMasterId = Convert.ToInt32(stages[i].Stage_Master_Id);
                journeyStage.StageDisplayOrder = Convert.ToInt32(stages[i].Stage_Display_Order);
                listJourneyStage.Add(journeyStage);
            }
            listJourneyStage = listJourneyStage.OrderBy(x => x.StageDisplayOrder).ToList();
            return listJourneyStage;
        }


        public static List<Stages_Moment> GetEndStages(string JourneyId, string StageId)
        {
            List<Stages_Moment> listJourneyStage = new List<Stages_Moment>();
            PJEntities entity = new PJEntities();
            var stages = dbPatientAdministration.GetJourneyStage(Convert.ToInt32(JourneyId));

            int? currentStageDisplayorder = stages.Where(x => x.Patient_Journey_Stages_Id == Convert.ToInt32(StageId)).Select(x => x.Stage_Display_Order).FirstOrDefault();

            var filteredStages = stages.Where(x => x.Stage_Display_Order >= Convert.ToInt32(currentStageDisplayorder)).ToList();

            for (int i = 0; i < filteredStages.Count; i++)
            {
                Stages_Moment journeyStage = new Stages_Moment();
                journeyStage.JourneyId = Convert.ToInt32(JourneyId);
                journeyStage.PatientStageId = filteredStages[i].Patient_Journey_Stages_Id;
                journeyStage.StageTitle = filteredStages[i].Stage_Title.ToUpper();
                journeyStage.StageMasterId = Convert.ToInt32(filteredStages[i].Stage_Master_Id);
                journeyStage.StageDisplayOrder = Convert.ToInt32(filteredStages[i].Stage_Display_Order);
                listJourneyStage.Add(journeyStage);
            }
            listJourneyStage = listJourneyStage.OrderBy(x => x.StageDisplayOrder).ToList();
            return listJourneyStage;
        }

        public static Int32? AddStrategicMoment(StrategicMomentModel strategicMoment, string User511)
        {
            var patientJourneyTemp = dbStrategicMoment.GetPatientJourneyTemp();
            var patientJourneyStagesTemp = dbStrategicMoment.GetPatientJourneyStagesTemp();
            var patientJourneyTransactionTemp = dbStrategicMoment.GetPatientJourneyTransactionTemp();

            Patient_Journey_Strategic_Moment_Temp strategicMomentsTemp = new Patient_Journey_Strategic_Moment_Temp();
            strategicMomentsTemp.Title = strategicMoment.Title;
            strategicMomentsTemp.Patient_Journey_Temp_Id = patientJourneyTemp.Where(x => x.Patient_Journey_Id == strategicMoment.JourneyId).Select(x => x.Patient_Journey_Temp_Id).FirstOrDefault();
            strategicMomentsTemp.Patient_Journey_Start_Stage_Temp_Id = patientJourneyStagesTemp.Where(x => x.Patient_Journey_Stages_Id == strategicMoment.StartStageId).Select(x => x.Patient_Journey_Stages_Temp_Id).FirstOrDefault();
            strategicMomentsTemp.Patient_Journey_End_Stage_Temp_Id = patientJourneyStagesTemp.Where(x => x.Patient_Journey_Stages_Id == strategicMoment.EndStageId).Select(x => x.Patient_Journey_Stages_Temp_Id).FirstOrDefault();
            strategicMomentsTemp.Patient_Journey_Start_Transaction_Temp_Id = patientJourneyTransactionTemp.Where(x => x.Patient_Journey_Transactions_Id == strategicMoment.StartTransactionId).Select(x => x.Patient_Journey_Transactions_Temp_Id).FirstOrDefault();
            strategicMomentsTemp.Patient_Journey_End_Transaction_Temp_Id = patientJourneyTransactionTemp.Where(x => x.Patient_Journey_Transactions_Id == strategicMoment.EndTransactionId).Select(x => x.Patient_Journey_Transactions_Temp_Id).FirstOrDefault();
            strategicMomentsTemp.Category = strategicMoment.Category;
            strategicMomentsTemp.Description = strategicMoment.Description;
            strategicMomentsTemp.Created_By = User511;
            strategicMomentsTemp.Created_Date = DateTime.Now;
            strategicMomentsTemp.Modified_By = User511;
            strategicMomentsTemp.Modified_Date = DateTime.Now;
            var responseTemp = dbStrategicMoment.AddStrategicMomentTemp(strategicMomentsTemp);

            if (responseTemp != 0)
            {
                Patient_Journey_Strategic_Moment strategicMoments = new Patient_Journey_Strategic_Moment();
                strategicMoments.Title = strategicMoment.Title;
                strategicMoments.Patient_Journey_Id = strategicMoment.JourneyId;
                strategicMoments.Patient_Journey_Start_Stage_Id = strategicMoment.StartStageId;
                strategicMoments.Patient_Journey_End_Stage_Id = strategicMoment.EndStageId;
                strategicMoments.Patient_Journey_Start_Transaction_Id = strategicMoment.StartTransactionId;
                strategicMoments.Patient_Journey_End_Transaction_Id = strategicMoment.EndTransactionId;
                strategicMoments.Patient_Journey_Strategic_Moment_Temp_Id = responseTemp;
                strategicMoments.Category = strategicMoment.Category;
                strategicMoments.Description = strategicMoment.Description;
                strategicMoments.Created_By = User511;
                strategicMoments.Created_Date = DateTime.Now;
                strategicMoments.Modified_By = User511;
                strategicMoments.Modified_Date = DateTime.Now;

                var response = dbStrategicMoment.AddStrategicMoment(strategicMoments);
                return response;
            }
            else
            {
                return 0;
            }
        }


        public static Int32? UpdateStrategicMoment(StrategicMomentModel strategicMoment, string User511)
        {
            var patientJourneyTemp = dbStrategicMoment.GetPatientJourneyTemp();
            var patientJourneyStagesTemp = dbStrategicMoment.GetPatientJourneyStagesTemp();
            var patientJourneyTransactionTemp = dbStrategicMoment.GetPatientJourneyTransactionTemp();

            Patient_Journey_Strategic_Moment_Temp strategicMomentsTemp = new Patient_Journey_Strategic_Moment_Temp();
            strategicMomentsTemp.Title = strategicMoment.Title;
            strategicMomentsTemp.Patient_Journey_Strategic_Moment_Temp_Id = strategicMoment.StrategicMomentTempId;
            strategicMomentsTemp.Patient_Journey_Temp_Id = patientJourneyTemp.Where(x => x.Patient_Journey_Id == strategicMoment.JourneyId).Select(x => x.Patient_Journey_Temp_Id).FirstOrDefault();
            strategicMomentsTemp.Patient_Journey_Start_Stage_Temp_Id = patientJourneyStagesTemp.Where(x => x.Patient_Journey_Stages_Id == strategicMoment.StartStageId).Select(x => x.Patient_Journey_Stages_Temp_Id).FirstOrDefault();
            strategicMomentsTemp.Patient_Journey_End_Stage_Temp_Id = patientJourneyStagesTemp.Where(x => x.Patient_Journey_Stages_Id == strategicMoment.EndStageId).Select(x => x.Patient_Journey_Stages_Temp_Id).FirstOrDefault();
            strategicMomentsTemp.Patient_Journey_Start_Transaction_Temp_Id = patientJourneyTransactionTemp.Where(x => x.Patient_Journey_Transactions_Id == strategicMoment.StartTransactionId).Select(x => x.Patient_Journey_Transactions_Temp_Id).FirstOrDefault();
            strategicMomentsTemp.Patient_Journey_End_Transaction_Temp_Id = patientJourneyTransactionTemp.Where(x => x.Patient_Journey_Transactions_Id == strategicMoment.EndTransactionId).Select(x => x.Patient_Journey_Transactions_Temp_Id).FirstOrDefault();
            strategicMomentsTemp.Category = strategicMoment.Category;
            strategicMomentsTemp.Description = strategicMoment.Description;
            strategicMomentsTemp.Created_By = User511;
            strategicMomentsTemp.Created_Date = DateTime.Now;
            strategicMomentsTemp.Modified_By = User511;
            strategicMomentsTemp.Modified_Date = DateTime.Now;
            var responseTemp = dbStrategicMoment.UpdateStrategicMomentTemp(strategicMomentsTemp);

            if (responseTemp == 1)
            {
                Patient_Journey_Strategic_Moment strategicMoments = new Patient_Journey_Strategic_Moment();
                strategicMoments.Title = strategicMoment.Title;
                strategicMoments.Patient_Journey_Strategic_Moment_Id = strategicMoment.StrategicMomentId;
                strategicMoments.Patient_Journey_Id = strategicMoment.JourneyId;
                strategicMoments.Patient_Journey_Start_Stage_Id = strategicMoment.StartStageId;
                strategicMoments.Patient_Journey_End_Stage_Id = strategicMoment.EndStageId;
                strategicMoments.Patient_Journey_Start_Transaction_Id = strategicMoment.StartTransactionId;
                strategicMoments.Patient_Journey_End_Transaction_Id = strategicMoment.EndTransactionId;
                strategicMoments.Category = strategicMoment.Category;
                strategicMoments.Description = strategicMoment.Description;
                strategicMoments.Created_By = User511;
                strategicMoments.Created_Date = DateTime.Now;
                strategicMoments.Modified_By = User511;
                strategicMoments.Modified_Date = DateTime.Now;

                var response = dbStrategicMoment.UpdateStrategicMoment(strategicMoments);
                return response;
            }
            else
            {
                return 0;
            }
        }

        public static Int32? DeleteStrategicMoment(string MomentId, string TempMomentId)
        {
            var response = dbStrategicMoment.DeleteStrategicMoment(Convert.ToInt32(MomentId));

            if (response == 1)
            {
                var responseTemp = dbStrategicMoment.DeleteStrategicMomentTemp(Convert.ToInt32(TempMomentId));
                return responseTemp;
            }
            else
            {
                return 0;
            }
        }
    }
}
