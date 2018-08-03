using PatientJourney.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientJourney.DataAccess.DataAccess
{
    public class dbStrategicMoment
    {
        public static List<Patient_Journey_Strategic_Moment> GetStrategicMoment(int JourneyId)
        {
            List<Patient_Journey_Strategic_Moment> strategicMoment = new List<Patient_Journey_Strategic_Moment>();
            using (PJEntities entity = new PJEntities())
            {
                strategicMoment = entity.Patient_Journey_Strategic_Moment.Where(x => x.Patient_Journey_Id == JourneyId).OrderBy(x => x.Patient_Journey_Strategic_Moment_Id).ToList();
                return strategicMoment;
            }
        }

        public static List<Stage_Master> GetStages()
        {
            List<Stage_Master> stages = new List<Stage_Master>();
            using (PJEntities entity = new PJEntities())
            {
                stages = entity.Stage_Master.ToList();
                return stages;
            }
        }

        public static List<Transaction_Master> GetTransactions()
        {
            List<Transaction_Master> transactions = new List<Transaction_Master>();
            using (PJEntities entity = new PJEntities())
            {
                transactions = entity.Transaction_Master.ToList();
                return transactions;
            }
        }

        public static List<Patient_Journey_Stages> GetJourneyStages()
        {
            List<Patient_Journey_Stages> journeyStages = new List<Patient_Journey_Stages>();
            using (PJEntities entity = new PJEntities())
            {
                journeyStages = entity.Patient_Journey_Stages.ToList();
                return journeyStages;
            }
        }

        public static List<Patient_Journey_Transactions> GetJourneyTransactions()
        {
            List<Patient_Journey_Transactions> journeyTransactions = new List<Patient_Journey_Transactions>();
            using (PJEntities entity = new PJEntities())
            {
                journeyTransactions = entity.Patient_Journey_Transactions.ToList();
                return journeyTransactions;
            }
        }

        public static Int32? AddStrategicMoment(Patient_Journey_Strategic_Moment strategicMoments)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    entity.Patient_Journey_Strategic_Moment.Add(strategicMoments);
                    entity.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? AddStrategicMomentTemp(Patient_Journey_Strategic_Moment_Temp strategicMomentsTemp)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    entity.Patient_Journey_Strategic_Moment_Temp.Add(strategicMomentsTemp);
                    entity.SaveChanges();
                    entity.Entry(strategicMomentsTemp).GetDatabaseValues();
                    int id = strategicMomentsTemp.Patient_Journey_Strategic_Moment_Temp_Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? UpdateStrategicMoment(Patient_Journey_Strategic_Moment strategicMoments)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var currentStrategic = entity.Patient_Journey_Strategic_Moment.Where(s => s.Patient_Journey_Strategic_Moment_Id == strategicMoments.Patient_Journey_Strategic_Moment_Id).FirstOrDefault();
                    if (currentStrategic != null)
                    {
                        currentStrategic.Title = strategicMoments.Title;
                        currentStrategic.Patient_Journey_Id = strategicMoments.Patient_Journey_Id;
                        currentStrategic.Patient_Journey_Start_Stage_Id = strategicMoments.Patient_Journey_Start_Stage_Id;
                        currentStrategic.Patient_Journey_End_Stage_Id = strategicMoments.Patient_Journey_End_Stage_Id;
                        currentStrategic.Patient_Journey_Start_Transaction_Id = strategicMoments.Patient_Journey_Start_Transaction_Id;
                        currentStrategic.Patient_Journey_End_Transaction_Id = strategicMoments.Patient_Journey_End_Transaction_Id;
                        currentStrategic.Description = strategicMoments.Description;
                        currentStrategic.Category = strategicMoments.Category;
                        currentStrategic.Modified_By = strategicMoments.Modified_By;
                        currentStrategic.Modified_Date = strategicMoments.Modified_Date;
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

        public static Int32? UpdateStrategicMomentTemp(Patient_Journey_Strategic_Moment_Temp strategicMomentsTemp)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var currentStrategic = entity.Patient_Journey_Strategic_Moment_Temp.Where(s => s.Patient_Journey_Strategic_Moment_Temp_Id == strategicMomentsTemp.Patient_Journey_Strategic_Moment_Temp_Id).FirstOrDefault();
                    if (currentStrategic != null)
                    {
                        currentStrategic.Title = strategicMomentsTemp.Title;
                        currentStrategic.Patient_Journey_Temp_Id = strategicMomentsTemp.Patient_Journey_Temp_Id;
                        currentStrategic.Patient_Journey_Start_Stage_Temp_Id = strategicMomentsTemp.Patient_Journey_Start_Stage_Temp_Id;
                        currentStrategic.Patient_Journey_End_Stage_Temp_Id = strategicMomentsTemp.Patient_Journey_End_Stage_Temp_Id;
                        currentStrategic.Patient_Journey_Start_Transaction_Temp_Id = strategicMomentsTemp.Patient_Journey_Start_Transaction_Temp_Id;
                        currentStrategic.Patient_Journey_End_Transaction_Temp_Id = strategicMomentsTemp.Patient_Journey_End_Transaction_Temp_Id;
                        currentStrategic.Description = strategicMomentsTemp.Description;
                        currentStrategic.Category = strategicMomentsTemp.Category;
                        currentStrategic.Modified_By = strategicMomentsTemp.Modified_By;
                        currentStrategic.Modified_Date = strategicMomentsTemp.Modified_Date;
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

        public static Int32? DeleteStrategicMoment(int MomentId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var moment = entity.Patient_Journey_Strategic_Moment.Where(x => x.Patient_Journey_Strategic_Moment_Id == MomentId).FirstOrDefault();

                    if (moment != null)
                    {
                        entity.Patient_Journey_Strategic_Moment.Remove(moment);
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

        public static Int32? DeleteStrategicMomentTemp(int TempMomentId)
        {
            try
            {
                using (PJEntities entity = new PJEntities())
                {
                    var moment = entity.Patient_Journey_Strategic_Moment_Temp.Where(x => x.Patient_Journey_Strategic_Moment_Temp_Id == TempMomentId).FirstOrDefault();

                    if (moment != null)
                    {
                        entity.Patient_Journey_Strategic_Moment_Temp.Remove(moment);
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

        public static List<Patient_Journey_Temp> GetPatientJourneyTemp()
        {
            List<Patient_Journey_Temp> patientJourneyTemp = new List<Patient_Journey_Temp>();
            using (PJEntities entity = new PJEntities())
            {
                patientJourneyTemp = entity.Patient_Journey_Temp.ToList();
                return patientJourneyTemp;
            }
        }

        public static List<Patient_Journey_Stages_Temp> GetPatientJourneyStagesTemp()
        {
            List<Patient_Journey_Stages_Temp> patientJourneyStagesTemp = new List<Patient_Journey_Stages_Temp>();
            using (PJEntities entity = new PJEntities())
            {
                patientJourneyStagesTemp = entity.Patient_Journey_Stages_Temp.ToList();
                return patientJourneyStagesTemp;
            }
        }

        public static List<Patient_Journey_Transactions_Temp> GetPatientJourneyTransactionTemp()
        {
            List<Patient_Journey_Transactions_Temp> patientJourneyTransactionsTemp = new List<Patient_Journey_Transactions_Temp>();
            using (PJEntities entity = new PJEntities())
            {
                patientJourneyTransactionsTemp = entity.Patient_Journey_Transactions_Temp.ToList();
                return patientJourneyTransactionsTemp;
            }
        }
    }
}
