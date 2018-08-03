using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientJourney.DataAccess.Data;

namespace PatientJourney.DataAccess.DataAccess
{
    public class dbVisualJourney
    {
        public List<Country_Master> GetCountryDetails()
        {
            using (PJEntities _entity = new PJEntities())
            {
                var result = _entity.Country_Master.ToList();
                return result;
            }
        }

        public List<Brand_Master> GetProductDetails()
        {
            using (PJEntities _entity = new PJEntities())
            {
                var result = _entity.Brand_Master.ToList();
                return result;
            }
        }

        public List<Patient_Journey> GetPatientJourney()
        {
            using (PJEntities _entity = new PJEntities())
            {
                var result = _entity.Patient_Journey.ToList();
                return result;
            }
        }

        public List<Patient_Journey_Stages> GetPatientJourneyStages()
        {
            using (PJEntities _entity = new PJEntities())
            {
                var result = _entity.Patient_Journey_Stages.ToList();
                return result;
            }
        }

        public List<Patient_Journey_Transactions> GetPatientJourneyTransactions()
        {
            using (PJEntities _entity = new PJEntities())
            {
                var result = _entity.Patient_Journey_Transactions.ToList();
                return result;
            }
        }

        public List<Stage_Master> GetStagesMasterData()
        {
            using (PJEntities _entity = new PJEntities())
            {
                var result = _entity.Stage_Master.ToList();
                return result;
            }
        }
        public int? GetJourneyStatus(int JourneyId)
        {
            int? JourneyStatus = 0;
            try
            {
                using (PJEntities _entity = new PJEntities())
                {
                    JourneyStatus = _entity.Patient_Journey.Where(x => x.Patient_Journey_Id == JourneyId).Select(x => x.Status_Master_Id).FirstOrDefault();
                }
                return JourneyStatus;
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }
}
