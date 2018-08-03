using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientJourney.BusinessModel.BuilderModels
{
    public class StrategicMomentModel
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public int StrategicMomentId { get; set; }
        public int? StartStageMasterId { get; set; }
        public int StartStageId { get; set; }
        public string StartStage { get; set; }
        public int? EndStageMasterId { get; set; }
        public int EndStageId { get; set; }
        public string EndStage { get; set; }
        public string Description { get; set; }
        public int? StartTransactionId { get; set; }
        public int? EndTransactionId { get; set; }
        public string Category { get; set; }
        public int JourneyId { get; set; }
        public int? StartTransactionMasterId { get; set; }
        public int? EndTransactionMasterId { get; set; }
        public string StartTransaction { get; set; }
        public string EndTransaction { get; set; }
        public int StrategicMomentTempId { get; set; }
        public string user511id { get; set; }
    }

    public class StrategicMoment
    {
        public int JourneyId { get; set; }
        public List<StrategicMomentModel> Strategic_Moment { get; set; }
        public int IsCurrentUserCountry { get; set; }
    }

    public class NewStrategicMoment
    {
        public List<Stages_Moment> Stages_Moment { get; set; }
        public List<Transaction_Moment> Transaction_Moment { get; set; }
    }


    public class Stages_Moment
    {
        public int JourneyId { get; set; }
        public int PatientStageId { get; set; }
        public int StageMasterId { get; set; }
        public string StageTitle { get; set; }
        public int StageDisplayOrder { get; set; }
    }

    public class Transaction_Moment
    {
        public int JourneyId { get; set; }
        public int PatientJourneyTransactionId { get; set; }
        public int TransactionMasterId { get; set; }
        public string TransactionTitle { get; set; }
    }
}
