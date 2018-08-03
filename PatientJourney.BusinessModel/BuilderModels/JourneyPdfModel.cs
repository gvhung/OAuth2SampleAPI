using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientJourney.BusinessModel.BuilderModels
{
    public class JourneyPdfModel
    {
        public int PatientJourneyId { get; set; }
        public string JourneyTitle { get; set; }
        public int StagesCount { get; set; }
    }

    public class EntirePatientJourney
    {
        public JourneyPdfModel Journey { get; set; }
        public string IndicationName { get; set; }
        public int? StageCount { get; set; }
        public FullJourneyTransaction FullJourneyTransaction { get; set; }
        public List<JourneyStages> Stages { get; set; }
        public List<Feasibility> Feasibility { get; set; }
        public List<Viability> Viability { get; set; }
        public List<StrategicMomentAll> StrategicMoment { get; set; }
    }

    public class Feasibility
    {
        public string Color { get; set; }
        public string Description { get; set; }
    }

    public class Viability
    {
        public string Color { get; set; }
        public string Description { get; set; }
    }

    public class FullJourneyTransaction
    {
        public FullJourneyTransaction()
        {
            Transactions = new List<Journey_Transaction>();
            ClinicalInterventions = new List<Journey_ClinicalInterventions>();
            AssociatedCosts = new List<Journey_AssociatedCosts>();
            DesiredOutcomes = new List<Journey_DesiredOutcomes>();
        }
        public List<Journey_Transaction> Transactions { get; set; }
        public List<Journey_AssociatedCosts> AssociatedCosts { get; set; }
        public List<Journey_DesiredOutcomes> DesiredOutcomes { get; set; }
        public List<Journey_ClinicalInterventions> ClinicalInterventions { get; set; }
    }

    public class Journey_Transaction
    {
        public Journey_Transaction()
        {
            TransactionsDetails = new List<Journey_Transaction_Details>();
        }
        public int PatientStageId { get; set; }
        public string StageColor { get; set; }
        public string StageColorForLocation { get; set; }
        public List<Journey_Transaction_Details> TransactionsDetails { get; set; }
    }


    public class ChartDetails
    {
        public string DotColor { get; set; }
        public string LineHeight { get; set; }
        public string MarginLeft { get; set; }
        public string FontSize { get; set; }
    }

    public class Journey_Transaction_Details
    {
        public int PatientJourneyTransactionId { get; set; }
        public int TransactionMasterId { get; set; }
        public int? ImageMasterId { get; set; }
        public string ImagePath { get; set; }
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

        public List<ChartDetails> ChartDetails { get; set; }
        //public List<StrategicMomentAll> StrategicMomentAll { get; set; }
    }

    public class StrategicMomentAll
    {
        public int? JourneyId { get; set; }
        public int? IsStrategic { get; set; }
        public string SMOMDescription { get; set; }
        public string SMOMCategory { get; set; }
        public int? StartStage { get; set; }
        public int? EndStage { get; set; }
        public int? StartTransaction { get; set; }
        public int? EndTransaction { get; set; }
        public string SMOMColor { get; set; }
        public int? Stage_Display_Order { get; set; }
        public int? Transaction_Display_Order { get; set; }
        public string MarginLeft { get; set; }
        public string Width { get; set; }
        public string padding { get; set; }
    }

    public class Journey_AssociatedCosts
    {
        public Journey_AssociatedCosts()
        {
            AssociatedCostDetails = new List<Journey_AssociatedCosts_Details>();
        }
        public int TransactionId { get; set; }
        public string StageColor { get; set; }
        public string StageColorForLocation { get; set; }
        public List<Journey_AssociatedCosts_Details> AssociatedCostDetails { get; set; }
    }

    public class Journey_AssociatedCosts_Details
    {
        public int AssociatedCostId { get; set; }
        public string AssociatedCosts { get; set; }
        public string Description { get; set; }
        public int TransactionId { get; set; }
    }

    public class Journey_DesiredOutcomes
    {
        public Journey_DesiredOutcomes()
        {
            DesiredOutcomesDetails = new List<Journey_DesiredOutcomes_Details>();
        }
        public int TransactionId { get; set; }
        public string StageColor { get; set; }
        public string StageColorForLocation { get; set; }
        public List<Journey_DesiredOutcomes_Details> DesiredOutcomesDetails { get; set; }
    }

    public class Journey_DesiredOutcomes_Details
    {
        public int DesiredOutcomeId { get; set; }
        public string DesiredOutcomes { get; set; }
        public string Description { get; set; }
        public int TransactionId { get; set; }
    }

    public class Journey_ClinicalInterventions
    {
        public Journey_ClinicalInterventions()
        {
            ClinicalInterventionsDetails = new List<Journey_ClinicalInterventions_Details>();
        }
        public int TransactionId { get; set; }
        public string StageColor { get; set; }
        public string StageColorForLocation { get; set; }
        public List<Journey_ClinicalInterventions_Details> ClinicalInterventionsDetails { get; set; }
    }

    public class Journey_ClinicalInterventions_Details
    {
        public int ClinicalInterventionId { get; set; }
        public int? ClinicalInterventionMasterId { get; set; }
        public string ClinicalInterventionTitle { get; set; }
        public int ImageMasterId { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public int TransactionId { get; set; }
    }


    public class JourneyStages
    {
        public int PatientJourneyId { get; set; }
        public int PatientStageId { get; set; }
        public int StageMasterId { get; set; }
        public string StageTitle { get; set; }
        public string StageWidth { get; set; }
        public int TransactionCount { get; set; }
        public string StageColor { get; set; }
        public string Description { get; set; }
        public string FeasibilityColor { get; set; }
        public string ViabilityColor { get; set; }
        public int StageNumber { get; set; }
        public string TimeStats { get; set; }
        public string PopulationStats { get; set; }

        public int StageOrder { get; set; }
        public int PopulationStatistics { get; set; }
        public int TimeStatistics { get; set; }
        public int PopulationScale { get; set; }
        public int TimeScale { get; set; }
        public string DisplayTimeScale { get; set; }
        public int DispPopulationStatistics { get; set; }

    }
}
