using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PatientJourney.BusinessModel.BuilderModels;
using PatientJourney.BusinessModel.BusinessModel;

namespace PatientJourney.GlobalConstants
{
    public class SessionHelper
    {
        public static UserModel LoggedinUser
        {
            get
            {
                return HttpContext.Current.Session["LoggedinUser"] as UserModel;
            }
            set
            {
                HttpContext.Current.Session["LoggedinUser"] = value;
            }
        }

        public static PatientJourneyModel PatientJourney
        {
            get
            {
                return HttpContext.Current.Session["PatientJourney"] as PatientJourneyModel;
            }
            set
            {
                HttpContext.Current.Session["PatientJourney"] = value;
            }
        }

        public static List<JourneyStage> JourneyStageList
        {
            get
            {
                return HttpContext.Current.Session["JourneyStage"] as List<JourneyStage>;
            }
            set
            {
                HttpContext.Current.Session["JourneyStage"] = value;
            }
        }

        public static List<AuditGridModel> ExportExcelList
        {
            get
            {
                return HttpContext.Current.Session["AuditGrid"] as List<AuditGridModel>;
            }
            set
            {
                HttpContext.Current.Session["AuditGrid"] = value;
            }
        }


        /* Session For Journey PDF */

        public static List<string> Base64ImageForPDF
        {
            get
            {
                return HttpContext.Current.Session["imagePDF"] as List<string>;
            }
            set
            {
                HttpContext.Current.Session["imagePDF"] = value;
            }
        }

        public static String Selected_Journey_Country
        {
            get
            {
                return HttpContext.Current.Session["SelectedJourneyCountry"] as String;
            }
            set
            {
                HttpContext.Current.Session["SelectedJourneyCountry"] = value;
            }
        }

        public static String Selected_Journey_Brand
        {
            get
            {
                return HttpContext.Current.Session["SelectedJourneyBrand"] as String;
            }
            set
            {
                HttpContext.Current.Session["SelectedJourneyBrand"] = value;
            }
        }

        public static String Selected_Journey_Year
        {
            get
            {
                return HttpContext.Current.Session["SelectedJourneyYear"] as String;
            }
            set
            {
                HttpContext.Current.Session["SelectedJourneyYear"] = value;
            }
        }

        public static JourneyPdfModel PdfJourney
        {
            get
            {
                return HttpContext.Current.Session["PdfJourney"] as JourneyPdfModel;
            }
            set
            {
                HttpContext.Current.Session["PdfJourney"] = value;
            }
        }

        public static List<StrategicMomentModel> StrategicMoment
        {
            get
            {
                return HttpContext.Current.Session["StrategicMoment"] as List<StrategicMomentModel>;
            }
            set
            {
                HttpContext.Current.Session["StrategicMoment"] = value;
            }
        }

        public static EntirePatientJourney EntirePatientJourney
        {
            get
            {
                return HttpContext.Current.Session["EntirePatientJourney"] as EntirePatientJourney;
            }
            set
            {
                HttpContext.Current.Session["EntirePatientJourney"] = value;
            }
        }

         public static bool? FirstLoad
        {
            get
            {
                return HttpContext.Current.Session["FirstLoad"] as bool?;
            }
            set
            {
                HttpContext.Current.Session["FirstLoad"] = value;
            }
        }

        /* Session For Journey PDF - End*/

    }
}
