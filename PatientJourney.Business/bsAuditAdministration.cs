using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientJourney.DataAccess.Data;
using PatientJourney.DataAccess.DataAccess;
using PatientJourney.BusinessModel.BuilderModels;
using PatientJourney.BusinessModel.Builders;
using PatientJourney.GlobalConstants;
using PatientJourney.BusinessModel;

namespace PatientJourney.Business
{
    public class bsAuditAdministration
    {
        // Method to bind the Audit details in Grid
        public static List<AuditGridModel> GetAuditHistoryForGrid()
        {
            var _finalList = dbAuditAdministration.GetAuditHistoryForGrid();
            return _finalList;
        }

        // Method to bind the Search Audit details in Grid
        public static List<AuditGridModel> GetSearchResultsForGrid(String SearchText)
        {
            var searchResults = dbAuditAdministration.GetAuditforSearchCriteria(SearchText);

            List<AuditGridModel> _finalList = new List<AuditGridModel>();
            AuditGridModel _audit;

            for (int i = 0; i < searchResults.Count; i++)
            {
                _audit = new AuditGridModel();
                string userId = searchResults[i].User_511;
                string dateString1 = searchResults[i].Logon_Client_Date.ToString();
                dateString1 = dateString1.Substring(0, Math.Min(dateString1.Length, 21));
                string dateString2 = searchResults[i].Logon_UTC_Date.ToString();
                dateString2 = dateString2.Substring(0, Math.Min(dateString2.Length, 21));

                _audit.FirstName = searchResults[i].First_Name;
                _audit.LastName = searchResults[i].Last_Name;
                _audit.MiddleInitial = searchResults[i].Middle_Initial;
                _audit.UPI = searchResults[i].UPI;
                _audit.FullName = searchResults[i].First_Name + " " + searchResults[i].Last_Name;
                _audit.Email = searchResults[i].Email_Id;
                _audit.User511 = searchResults[i].User_511;
                _audit.LogonClientDate = dateString1;
                _audit.LogonUTCDate = dateString2;
                _audit.LogonClientTimeZone = searchResults[i].Logon_Client_TimeZone;
                _finalList.Add(_audit);
            }

            return _finalList;
        }
    }
}
