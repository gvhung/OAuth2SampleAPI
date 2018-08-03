using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientJourney.BusinessModel.BuilderModels
{
    public class PatientJourneyModel
    {
        public int PatientJourneyId { get; set; }
        public string JourneyTitle { get; set; }
        public string JourneyDescription { get; set; } 
        public int TherapeuticID { get; set; }
        public int SubTherapeuticID { get; set; }
        public int IndicationID { get; set; }
        public int BrandID { get; set; }
        public int ArchetypeID { get; set; }
        public int AreaID { get; set; }
        public int CountryID { get; set; }
        public int StatusID { get; set; }
        public int Year { get; set; }
        public string TherapeuticArea { get; set; }
        public string SubTherapeuticArea { get; set; }
        public string Indication { get; set; }
        public string Brand { get; set; }
        public string Area { get; set; }
        public string Country { get; set; }
        public int isCountryClone { get; set; }
        public int TempJourneyId { get; set; }
        public string User511id { get; set; }
    }

    public class JourneyStage
    {
        public int PatientJourneyId { get; set; }
        public int PatientStageId { get; set; }
        public string StageTitle { get; set; }
        public int StageOrder { get; set; }
        public int PopulationStatistics { get; set; }
        public int TimeStatistics { get; set; }
        public int PopulationScale { get; set; }
        public int TimeScale { get; set; }
        public int StageMasterId { get; set; }
        public int TransactionCount { get; set; }
        public string DisplayTimeScale { get; set; }
        public string DispPopulationStatistics { get; set; }
        public string user511id { get; set; }
        public int StatusID { get; set; }
}

    public class Transaction
    {
        public int PatientJourneyTransactionId { get; set; }
        public int TransactionMasterId { get; set; }
        public string TransactionTitle { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int PatientJourneyId { get; set; }
        public int PatientStageId { get; set; }
        public string TransactionDescription { get; set; }
        public string HCPDescription { get; set; }
        public string PayerDescription { get; set; }
        public string PatientDescription { get; set; }
        public string FeasibilityDescription { get; set; }
        public string ViabilityDescription { get; set; }
        public int HCPRating { get; set; }
        public int PayerRating { get; set; }
        public int PatientRating { get; set; }
        public int FeasibilityRating { get; set; }
        public int ViabilityRating { get; set; }
        public int DisplayOrder { get; set; }
        public int ImageMasterId { get; set; }
        public string ImagePath { get; set; }
        public int LocationImageMasterId { get; set; }
        public string LocationImagePath { get; set; }
        public string PatientEvidence { get; set; }
        public string HCPEvidence { get; set; }
        public string PayerEvidence { get; set; }
        public string FeasibilityEvidence { get; set; }
        public string ViabilityEvidence { get; set; }
        public List<StrategicMomentDetails> StrategicMoment { get; set; }
        public int StatusID { get; set; }
        public string user511id { get; set; }
    }

    public class StrategicMomentDetails
    {
        public int IsStrategic { get; set; }
        public string SMOMDescription { get; set; }
        public string SMOMTitle { get; set; }
        public string SMOMCategory { get; set; }
        public string StartStage { get; set; }
        public string EndStage { get; set; }
        public string StartTransaction { get; set; }
        public string EndTransaction { get; set; }
    }

    public class VersionDetails
    {
        public string Comment { get; set; }
        public string VersionDate { get; set; }
        public string VersionTime { get; set; }
        public string IsApproved { get; set; }
    }

    public class FullPatientJounrney
    {
        public PatientJourneyModel Journey { get; set; }
        public List<JourneyStage> Stage { get; set; }
        public int IsCurrentUserCountry { get; set; }
    }

    public class AssociatedCost
    {
        public int AssociatedCostId { get; set; }
        public string AssociatedCosts { get; set; }
        public string Description { get; set; }
        public string Evidence { get; set; }
        public int TransactionId { get; set; }
        public int PatientJourneyId { get; set; }
        public string user511id { get; set; }
    }

    public class DesiredOutcome
    {
        public int DesiredOutcomeId { get; set; }
        public string DesiredOutcomes { get; set; }
        public string Description { get; set; }
        public string Evidence { get; set; }
        public int TransactionId { get; set; }
        public int PatientJourneyId { get; set; }
        public string user511id { get; set; }
    }

    public class ClinicalIntervention
    {
        public int ClinicalInterventionId { get; set; }
        public int? ClinicalInterventionMasterId { get; set; }
        public string ClinicalInterventionTitle { get; set; }
        public int[] SubClinicalId { get; set; }
        public string[] SubClinicalImage { get; set; }
        public string[] SubClinicalTitle { get; set; }
        public string Description { get; set; }
        public string Evidence { get; set; }
        public int TransactionId { get; set; }
        public int ImageMasterId { get; set; }
        public string ImagePath { get; set; }
        public int PatientJourneyId { get; set; }
        public string user511id { get; set; }
    }

    public class FullPatientTransaction
    {
        public List<Transaction> Transactions { get; set; }
        public List<AssociatedCost> AssociatedCosts { get; set; }
        public List<DesiredOutcome> DesiredOutcomes { get; set; }
        public List<ClinicalIntervention> ClinicalInterventions { get; set; }
    }

    public class TransactionInfo
    {
        public Int32? Transactionid { get; set; }
        public int TransactionCount { get; set; }
    }

    public class MailContents
    {
        public string fromAddress { get; set; }
        public string toAddress { get; set; }
        public string ccAddress { get; set; }
        public string body { get; set; }
        public string subject { get; set; }
        //public Attachment attachment { get; set; }
    }
}
