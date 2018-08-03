using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientJourney.DataAccess.Data;
using PatientJourney.BusinessModel.BuilderModels;
using PatientJourney.BusinessModel.Builders;
using System.Globalization;

namespace PatientJourney.DataAccess.DataAccess
{
    public class dbAuditAdministration
    {
        public static List<AuditGridModel> GetAuditHistoryForGrid()
        {
            var _allAuditHistory = dbAuditAdministration.GetAllUsersAudit();

            try
            {
                List<AuditGridModel> _finalList = new List<AuditGridModel>();
                AuditGridModel _audit;

                for (int i = 0; i < _allAuditHistory.Count; i++)
                {
                    _audit = new AuditGridModel();
                    string userId = _allAuditHistory[i].User_511;
                    string dateString1 = _allAuditHistory[i].Logon_Client_Date.ToString();
                    dateString1 = dateString1.Substring(0, Math.Min(dateString1.Length, 21));
                    string dateString2 = _allAuditHistory[i].Logon_UTC_Date.ToString();
                    dateString2 = dateString2.Substring(0, Math.Min(dateString2.Length, 21));

                    _audit.FirstName = _allAuditHistory[i].First_Name;
                    _audit.LastName = _allAuditHistory[i].Last_Name;
                    _audit.MiddleInitial = _allAuditHistory[i].Middle_Initial;
                    _audit.UPI = _allAuditHistory[i].UPI;
                    _audit.FullName = _allAuditHistory[i].First_Name + " " + _allAuditHistory[i].Last_Name;
                    _audit.Email = _allAuditHistory[i].Email_Id;
                    _audit.User511 = _allAuditHistory[i].User_511;
                    _audit.LogonClientDate = dateString1;
                    _audit.LogonUTCDate = dateString2;
                    _audit.LogonClientTimeZone = _allAuditHistory[i].Logon_Client_TimeZone;
                    _finalList.Add(_audit);
                }
                return _finalList;
            }
            catch
            {
                return null;
            }
        }

        public static List<User_Logon_Audit> GetAuditforSearchCriteria(String SearchText)
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    List<User_Logon_Audit> _auditFilter = new List<User_Logon_Audit>();

                    var auditList = _entity.User_Logon_Audit.ToList();

                    SearchText = SearchText.ToLower();

                    _auditFilter = (from h in auditList
                                   where h.First_Name.Contains(SearchText)
                                   || h.Last_Name.Contains(SearchText)
                                   || h.UPI.ToString().Contains(SearchText)
                                   || h.User_511.Contains(SearchText)
                                   || h.Email_Id.Contains(SearchText)
                                   || h.Logon_Client_TimeZone.Contains(SearchText)
                                   select h).ToList();
                    return _auditFilter;
                }
                catch(Exception ex)
                {
                    
                    throw ex;
                }
            }
        }

        public static List<User_Logon_Audit> GetAllUsersAudit()
        {
            using (PJEntities _entity = new PJEntities())
            {
                try
                {
                    var allAuditlist = _entity.User_Logon_Audit.ToList();
                    return allAuditlist;
                }
                catch
                {
                    return null;
                }
            }
        }

    }
}
