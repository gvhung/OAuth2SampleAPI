using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientJourney.BusinessModel;
using PatientJourney.DataAccess.DataAccess;
using PatientJourney.DataAccess.Data;
using PatientJourney.BusinessModel.BuilderModels;
using System.Configuration;

namespace PatientJourney.Business
{
    public static class bsPatientAdministration
    {
        public static Int32? AddJourney(PatientJourneyModel journey, string User511)
        {
            Patient_Journey patientJourney = new Patient_Journey();
            patientJourney.Journey_Title = journey.JourneyTitle;
            patientJourney.Journey_Description = journey.JourneyDescription;
            patientJourney.Brand_Master_Id = journey.BrandID;
            patientJourney.Country_Master_Id = journey.CountryID;
            patientJourney.Status_Master_Id = journey.StatusID;
            patientJourney.Year = journey.Year;
            patientJourney.Created_By = User511;
            patientJourney.Created_Date = DateTime.Now;
            patientJourney.Modified_By = User511;
            patientJourney.Modified_Date = DateTime.Now;

            var response = dbPatientAdministration.AddJourney(patientJourney);
            return response;
        }

        public static Int32? RemoveJourney(string JourneyId)
        {
            var response = dbPatientAdministration.RemoveJourney(Convert.ToInt32(JourneyId));
            return response;
        }

        public static Int32? SubmitJourney(string JourneyId, string Comment, string User511, string SubmittedUserEmail)
        {
            Patient_Journey patientJourney = new Patient_Journey();
            patientJourney.Patient_Journey_Id = Convert.ToInt32(JourneyId);
            patientJourney.User_Comments = Comment;
            patientJourney.Status_Master_Id = 2;//Submit status
            patientJourney.Submitted_By = User511;
            patientJourney.Submitted_Date = DateTime.Now;
            patientJourney.Modified_By = User511;
            patientJourney.Modified_Date = DateTime.Now;
            var response = dbPatientAdministration.SubmitJourney(patientJourney);
            if (response != null)
            {
                var journey = dbPatientAdministration.GetJourneyDetails(Convert.ToInt32(JourneyId));
                bsAuthentication user = new bsAuthentication();
                var SubmittedUser = user.GetUserNameForDisplay(User511);
                var CreatedUser = user.GetUserNameForDisplay(journey.Created_By);
                var CreatedUserEmail = dbPatientAdministration.GetUserEmailId(journey.Created_By);
                EmailDynamicParams emailParams = new EmailDynamicParams();
                emailParams.FromAddress = PatientJourney.GlobalConstants.EmailTemplate.FromAddress;
                emailParams.Title = PatientJourney.GlobalConstants.EmailTemplate.SubmitTitle;
                emailParams.CC = CreatedUserEmail;
                List<String> listEmail=dbPatientAdministration.GetAllAdminsByCountry(Convert.ToInt32(journey.Country_Master_Id));
                if (ConfigurationManager.AppSettings["EmailFlag"].ToLower() != "true")
                {
                    listEmail.Remove("anne.najjar@abbvie.com");
                }
                emailParams.ToAddress = string.Join(",", listEmail.Select(n => n.ToString()).ToArray());
                emailParams.Subject = PatientJourney.GlobalConstants.EmailTemplate.Subject;
                emailParams.JourneyName = journey.Journey_Title;
                emailParams.BrandName = DefaultListBSForPJ.GetBrandName(journey.Brand_Master_Id.ToString());
                emailParams.CountryName = DefaultListBSForPJ.GetCountryName(journey.Country_Master_Id.ToString());
                emailParams.CreatedBy = CreatedUser.Userdetails.FirstName.First().ToString().ToUpper() + String.Join("", CreatedUser.Userdetails.FirstName.Skip(1)) + " " + CreatedUser.Userdetails.LastName.First().ToString().ToUpper() + String.Join("", CreatedUser.Userdetails.LastName.Skip(1));
                emailParams.CreatedDate = journey.Created_Date.Value.ToShortDateString();
                emailParams.ReviewedBy = SubmittedUser.Userdetails.FirstName.First().ToString().ToUpper() + String.Join("", SubmittedUser.Userdetails.FirstName.Skip(1)) + " " + SubmittedUser.Userdetails.LastName.First().ToString().ToUpper() + String.Join("", SubmittedUser.Userdetails.LastName.Skip(1));
                emailParams.Comment = journey.User_Comments;
                var emailresponse = SendEmailWithTemplate(emailParams);

            }
            return response;
        }

        public static bool SendEmailWithTemplate(EmailDynamicParams emailDynamicParams)
        {
            MailContents mailContents = new MailContents();
            Email email = new Email();

            try
            {
                
                    mailContents.fromAddress = emailDynamicParams.FromAddress;
                    mailContents.toAddress = emailDynamicParams.ToAddress;
                    mailContents.subject = emailDynamicParams.Subject;
                    mailContents.ccAddress = emailDynamicParams.CC;

                    string mailBodyContent = GlobalConstants.EmailTemplate.Email;
                    mailBodyContent = mailBodyContent.Replace("(Title)", emailDynamicParams.Title);
                    mailBodyContent = mailBodyContent.Replace("(JourneyName)", emailDynamicParams.JourneyName);
                    mailBodyContent = mailBodyContent.Replace("(BrandName)", emailDynamicParams.BrandName);
                    mailBodyContent = mailBodyContent.Replace("(CountryName)", emailDynamicParams.CountryName);
                    mailBodyContent = mailBodyContent.Replace("(CreatedBy)", emailDynamicParams.CreatedBy);
                    mailBodyContent = mailBodyContent.Replace("(CreatedDate)", emailDynamicParams.CreatedDate);
                    mailBodyContent = mailBodyContent.Replace("(CreatedBy)", emailDynamicParams.CreatedBy);
                    mailBodyContent = mailBodyContent.Replace("(ReviewedBy)", emailDynamicParams.ReviewedBy);
                    mailBodyContent = mailBodyContent.Replace("(Comment)", emailDynamicParams.Comment);

                    mailContents.body = mailBodyContent;

                   
                    bool emailout = email.SendEmail(mailContents);
               

                return emailout;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<JourneySearchResult> GetSearchResults(string CountryId, string BrandId, string Year, string RoleIds, string User511)
        {
            //Email email = new Email();
            //var status = email.SendEmail("shanthini.natesan@abbvie.com", "Shanthini",
            //    "karthikeyan.alagarsamy@abbvie.com", "Journey Approved", "Your Patient Journey was approved");
            List<JourneySearchResult> lstSearchResult = new List<JourneySearchResult>();
            try
            {
                List<Patient_Journey> result = new List<Patient_Journey>();

                result = dbPatientAdministration.GetSearchResults(Convert.ToInt32(CountryId), Convert.ToInt32(BrandId), Convert.ToInt32(Year));
                PJEntities entity = new PJEntities();

                JourneySearchResult firstresult = new JourneySearchResult();
                var first = result.Where(x => x.Status_Master_Id == 3 || x.Status_Master_Id == 7).FirstOrDefault();
                if (first != null)
                {
                    firstresult.JourneyNumber = 1;
                    firstresult.JourneyId = first.Patient_Journey_Id;
                    firstresult.JourneyName = first.Journey_Title;
                    firstresult.JourneyDescription = first.Journey_Description;
                    firstresult.BrandId = Convert.ToInt32(first.Brand_Master_Id);
                    firstresult.BrandName = entity.Brand_Master.Where(x => x.Brand_Master_Id == firstresult.BrandId).FirstOrDefault().Brand_Name;
                    firstresult.CountryId = Convert.ToInt32(first.Country_Master_Id);
                    firstresult.CountryName = entity.Country_Master.Where(x => x.Country_Master_Id == firstresult.CountryId).FirstOrDefault().Country_Name;
                    firstresult.CreatedBy = first.Created_By;
                    firstresult.CreatedDate = first.Created_Date;
                    firstresult.StrCreatedDate = first.Created_Date.Value.ToShortDateString();
                    firstresult.StatusId = Convert.ToInt32(first.Status_Master_Id);
                    firstresult.StatusName = entity.Status_Master.Where(x => x.Status_Master_Id == firstresult.StatusId).FirstOrDefault().Status_Name;
                    firstresult.ApproverComments = first.Approver_Comments;
                    firstresult.SubmitComments = first.User_Comments;
                    if (RoleIds.Contains("2"))
                    {
                        firstresult.CurrentUserRole = 2;
                    }
                    else
                    {
                        firstresult.CurrentUserRole = 1;
                    }
                    if (User511 == firstresult.CreatedBy && firstresult.CurrentUserRole == 1)
                    {
                        firstresult.isCurrentUserRecord = 2;
                    }
                    else if (firstresult.CurrentUserRole == 2)
                    {
                        firstresult.isCurrentUserRecord = 2;
                    }
                    else
                    {
                        firstresult.isCurrentUserRecord = 1;
                    }
                    lstSearchResult.Add(firstresult);
                }

                JourneySearchResult searchresult = null;
                int j = 1;
                for (int i = 0; i < result.Count(); i++)
                {
                    if (result[i].Status_Master_Id != 3 && result[i].Status_Master_Id != 7)
                    {
                        searchresult = new JourneySearchResult();
                        if (first == null && i == 0)
                        {
                            searchresult.JourneyNumber = j;
                        }
                        else
                        {
                            j = j + 1;
                            searchresult.JourneyNumber = j;
                        }
                        searchresult.JourneyId = result[i].Patient_Journey_Id;
                        searchresult.JourneyName = result[i].Journey_Title;
                        searchresult.JourneyDescription = result[i].Journey_Description;
                        searchresult.BrandId = Convert.ToInt32(result[i].Brand_Master_Id);
                        searchresult.BrandName = entity.Brand_Master.Where(x => x.Brand_Master_Id == searchresult.BrandId).FirstOrDefault().Brand_Name;
                        searchresult.CountryId = Convert.ToInt32(result[i].Country_Master_Id);
                        searchresult.CountryName = entity.Country_Master.Where(x => x.Country_Master_Id == searchresult.CountryId).FirstOrDefault().Country_Name;
                        searchresult.CreatedBy = result[i].Created_By;
                        searchresult.CreatedDate = result[i].Created_Date;
                        searchresult.StrCreatedDate = result[i].Created_Date.Value.ToShortDateString();
                        searchresult.StatusId = Convert.ToInt32(result[i].Status_Master_Id);
                        searchresult.StatusName = entity.Status_Master.Where(x => x.Status_Master_Id == searchresult.StatusId).FirstOrDefault().Status_Name;
                        searchresult.ApproverComments = result[i].Approver_Comments;
                        searchresult.SubmitComments = result[i].User_Comments;
                        if (RoleIds.Contains("2"))
                        {
                            searchresult.CurrentUserRole = 2;
                        }
                        else
                        {
                            searchresult.CurrentUserRole = 1;
                        }
                        if (User511 == searchresult.CreatedBy && searchresult.CurrentUserRole == 1)
                        {
                            searchresult.isCurrentUserRecord = 2;
                        }
                        else if (firstresult.CurrentUserRole == 2)
                        {
                            searchresult.isCurrentUserRecord = 2;
                        }
                        else
                        {
                            searchresult.isCurrentUserRecord = 1;
                        }
                        lstSearchResult.Add(searchresult);
                    }
                }
            }
            catch (Exception)
            {
            }
            return lstSearchResult;
        }

        public static int GetTempJourneyDetails(int JourneyId)
        {
            var response = dbPatientAdministration.GetTempJourneyDetails(JourneyId);
            return response.Patient_Journey_Temp_Id;
        }

        public static Int32? ApproveRejectSendbackJourney(string JourneyId, string Comment, int StatusId, string BrandId, string YearName, string CountryId, string User511, string ApprovedUserEmail)
        {
            Patient_Journey patientJourney = new Patient_Journey();
            var oldStatus = dbPatientAdministration.GetJourneyDetails(Convert.ToInt32(JourneyId));
            patientJourney.Patient_Journey_Id = Convert.ToInt32(JourneyId);
            patientJourney.Approver_Comments = Comment;
            patientJourney.Status_Master_Id = StatusId;
            if (StatusId == 3)
            {
                patientJourney.Approved_By = User511;
                patientJourney.Approved_Date = DateTime.Now;
            }
            patientJourney.Modified_By = User511;
            patientJourney.Modified_Date = DateTime.Now;
            patientJourney.Brand_Master_Id = Convert.ToInt32(BrandId);
            patientJourney.Country_Master_Id = Convert.ToInt32(CountryId);
            patientJourney.Year = Convert.ToInt32(YearName);
            Patient_Journey otherapproved = null;
            var response = dbPatientAdministration.ApproveRejectSendbackJourney(patientJourney, out otherapproved);


            if (response != null)
            {
                if (StatusId == 3)
                {
                    Patient_Journey_Temp tempJourney = dbPatientAdministration.GetTempJourneyDetails(Convert.ToInt32(JourneyId));
                    if(tempJourney!=null)
                    {
                        List<Patient_Journey_VersionDetails_Temp> oldVersion = dbPatientAdministration.GetVersionDetails(tempJourney.Patient_Journey_Temp_Id);
                        Patient_Journey_VersionDetails version = null;
                        if (oldVersion != null)
                        {
                            Int32? approveVersion = dbPatientAdministration.VersionApproval(tempJourney.Patient_Journey_Temp_Id);
                            Int32? delVersionResponse = dbPatientAdministration.RemoveAllVersion(Convert.ToInt32(JourneyId));
                            for (int i = 0; i < oldVersion.Count; i++)
                            {
                                version = new Patient_Journey_VersionDetails();
                                version.Created_By = oldVersion[i].Created_By;
                                version.Created_Date = oldVersion[i].Created_Date;
                                version.Modified_By = oldVersion[i].Modified_By;
                                version.Modified_Date = oldVersion[i].Modified_Date;
                                version.Patient_Journey_Id = Convert.ToInt32(JourneyId);
                                version.Version_Comments = oldVersion[i].Version_Comments;
                                version.Version_Title = oldVersion[i].Version_Title;
                                Int32? versionResponse = dbPatientAdministration.AddVersionDetails(version);
                            }
                            
                        }
                        
                    }

                }

                bsAuthentication user = new bsAuthentication();
                var ApprovedUser = user.GetUserNameForDisplay(User511);
                var CreatedUser = user.GetUserNameForDisplay(oldStatus.Created_By);
                var CreatedUserEmail = dbPatientAdministration.GetUserEmailId(oldStatus.Created_By);
                
                EmailDynamicParams emailParams = new EmailDynamicParams();
                emailParams.FromAddress = PatientJourney.GlobalConstants.EmailTemplate.FromAddress;
                if (StatusId == 3)
                {
                    List<String> listEmail = dbPatientAdministration.GetAllAdminsByCountry(Convert.ToInt32(oldStatus.Country_Master_Id));
                    if (ConfigurationManager.AppSettings["EmailFlag"].ToLower() != "true")
                    {
                        listEmail.Remove("anne.najjar@abbvie.com");
                    }
                    emailParams.ToAddress = string.Join(",", listEmail.Select(n => n.ToString()).ToArray());
                    emailParams.CC = CreatedUserEmail;
                }
                else
                {
                    emailParams.CC = ApprovedUserEmail;
                    emailParams.ToAddress = CreatedUserEmail;
                }
                emailParams.Subject = PatientJourney.GlobalConstants.EmailTemplate.Subject;
                emailParams.JourneyName = oldStatus.Journey_Title;
                emailParams.BrandName = DefaultListBSForPJ.GetBrandName(oldStatus.Brand_Master_Id.ToString());
                emailParams.CountryName = DefaultListBSForPJ.GetCountryName(oldStatus.Country_Master_Id.ToString());
                emailParams.CreatedBy = CreatedUser.Userdetails.FirstName.First().ToString().ToUpper() + String.Join("", CreatedUser.Userdetails.FirstName.Skip(1)) + " " + CreatedUser.Userdetails.LastName.First().ToString().ToUpper() + String.Join("", CreatedUser.Userdetails.LastName.Skip(1));
                emailParams.CreatedDate = oldStatus.Created_Date.Value.ToShortDateString();
                emailParams.ReviewedBy = ApprovedUser.Userdetails.FirstName.First().ToString().ToUpper() + String.Join("", ApprovedUser.Userdetails.FirstName.Skip(1)) + " " + ApprovedUser.Userdetails.LastName.First().ToString().ToUpper() + String.Join("", ApprovedUser.Userdetails.LastName.Skip(1));
                emailParams.Comment = Comment;
                if (StatusId == 3)
                {              
                    emailParams.Title = PatientJourney.GlobalConstants.EmailTemplate.ApproveTitle;
                }
                else if (StatusId == 4)
                {
                    emailParams.Title = PatientJourney.GlobalConstants.EmailTemplate.SentBackTitle;
                }
                if (StatusId == 5)
                {
                    emailParams.Title = PatientJourney.GlobalConstants.EmailTemplate.RejectTitle;
                }
                var emailresponse = SendEmailWithTemplate(emailParams);


                if (StatusId == 3)
                {
                    if (oldStatus.Status_Master_Id == 7)
                    {
                        var removeMailJourney = bsPatientAdministration.RemoveAllStages(JourneyId);
                        var journey = bsPatientAdministration.CopyJourneyToMain(Convert.ToInt32(JourneyId));
                        var smom = dbPatientAdministration.CopySMOMToMain(Convert.ToInt32(JourneyId));
                    }
                    else
                    {
                        var tempJourney = bsPatientAdministration.CopyJourneyToTemp(Convert.ToInt32(JourneyId));
                        var tempSmom = dbPatientAdministration.CopySMOMToTemp(Convert.ToInt32(JourneyId));
                    }

                    if (otherapproved != null)
                    {
                            EmailDynamicParams emailArchived = new EmailDynamicParams();
                            var CreatedEmail = dbPatientAdministration.GetUserEmailId(otherapproved.Created_By);
                            var Created = user.GetUserNameForDisplay(otherapproved.Created_By);
                            emailArchived.FromAddress = PatientJourney.GlobalConstants.EmailTemplate.FromAddress;
                            emailArchived.ToAddress = CreatedUserEmail;
                            emailArchived.Subject = PatientJourney.GlobalConstants.EmailTemplate.Subject;
                            emailArchived.JourneyName = otherapproved.Journey_Title;
                            emailArchived.BrandName = DefaultListBSForPJ.GetBrandName(otherapproved.Brand_Master_Id.ToString());
                            emailArchived.CountryName = DefaultListBSForPJ.GetCountryName(otherapproved.Country_Master_Id.ToString());
                            emailArchived.CreatedBy = Created.Userdetails.FirstName.First().ToString().ToUpper() + String.Join("", Created.Userdetails.FirstName.Skip(1)) + " " + Created.Userdetails.LastName.First().ToString().ToUpper() + String.Join("", Created.Userdetails.LastName.Skip(1));
                            emailArchived.CreatedDate = otherapproved.Created_Date.Value.ToShortDateString();
                            emailArchived.ReviewedBy = ApprovedUser.Userdetails.FirstName.First().ToString().ToUpper() + String.Join("", ApprovedUser.Userdetails.FirstName.Skip(1)) + " " + ApprovedUser.Userdetails.LastName.First().ToString().ToUpper() + String.Join("", ApprovedUser.Userdetails.LastName.Skip(1));
                            emailArchived.Comment = Comment;
                            emailArchived.Title = PatientJourney.GlobalConstants.EmailTemplate.ArchiveTitle;
                            var emailresult = SendEmailWithTemplate(emailArchived);
                    }
                }

                
            }
            return response;
        }

        public static Int32? ReorderStage(string[] StageOrders, string JourneyId, int StatusId, string User511)
        {
            try
            {
                int[] oldOrder = new int[StageOrders.Length];
                int[] newOrder = new int[StageOrders.Length];
                for (int i = 0; i < StageOrders.Length; i++)
                {
                    newOrder[i] = Convert.ToInt32(StageOrders[i].Split(':')[0]);
                    oldOrder[i] = Convert.ToInt32(StageOrders[i].Split(':')[1]);
                }
                var response = dbPatientAdministration.ReorderStage(newOrder, oldOrder, Convert.ToInt32(JourneyId));
                return response;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? AddJourneyStage(JourneyStage journeyStage, int StatusId, string User511)
        {
            PJEntities entity = new PJEntities();
            Patient_Journey_Stages patientJourneyStage = new Patient_Journey_Stages();
            int stageCount = entity.Patient_Journey_Stages.Where(x => x.Patient_Journey_Id == journeyStage.PatientJourneyId).Count();
            patientJourneyStage.Patient_Journey_Id = journeyStage.PatientJourneyId;
            patientJourneyStage.Stage_Master_Id = journeyStage.StageMasterId;
            patientJourneyStage.Stage_Title = journeyStage.StageTitle == null ? String.Empty : journeyStage.StageTitle.ToString();
            patientJourneyStage.Stage_Display_Order = stageCount + 1;
            patientJourneyStage.Time_Statistics = journeyStage.TimeStatistics * journeyStage.TimeScale;
            patientJourneyStage.Population_Statistics = journeyStage.PopulationStatistics * journeyStage.PopulationScale;
            patientJourneyStage.Created_By = User511;
            patientJourneyStage.Created_Date = DateTime.Now;
            patientJourneyStage.Modified_By = User511;
            patientJourneyStage.Modified_Date = DateTime.Now;
            var response = dbPatientAdministration.AddJourneyStage(patientJourneyStage);
            return response;
        }

        public static Int32? UpdateJourneyStage(JourneyStage journeyStage, int StatusId, string User511)
        {
            Patient_Journey_Stages patientJourneyStage = new Patient_Journey_Stages();
            patientJourneyStage.Patient_Journey_Id = journeyStage.PatientJourneyId; //Id added
            patientJourneyStage.Patient_Journey_Stages_Id = journeyStage.PatientStageId;
            if (journeyStage.StageOrder != 0)
            {
                patientJourneyStage.Stage_Display_Order = journeyStage.StageOrder;
            }
            patientJourneyStage.Patient_Journey_Id = journeyStage.PatientJourneyId;
            patientJourneyStage.Time_Statistics = journeyStage.TimeStatistics * journeyStage.TimeScale;
            patientJourneyStage.Population_Statistics = journeyStage.PopulationStatistics * journeyStage.PopulationScale;
            patientJourneyStage.Modified_By = User511;
            patientJourneyStage.Modified_Date = DateTime.Now;
            var response = dbPatientAdministration.UpdateJourneyStage(patientJourneyStage);
            return response;
        }

        public static Int32? RemoveJourneyStage(string JourneyId, string StageId, int StatusId, string User511)
        {
            var response = dbPatientAdministration.RemoveJourneyStage(Convert.ToInt32(JourneyId), Convert.ToInt32(StageId));
            return response;
        }

        public static List<JourneyStage> GetJourneyStage(string JourneyId)
        {
            List<JourneyStage> listJourneyStage = new List<JourneyStage>();
            PJEntities entity = new PJEntities();
            var stages = dbPatientAdministration.GetJourneyStage(Convert.ToInt32(JourneyId));
            for (int i = 0; i < stages.Count; i++)
            {
                JourneyStage journeyStage = new JourneyStage();
                journeyStage.PatientJourneyId = Convert.ToInt32(stages[i].Patient_Journey_Id);
                journeyStage.PatientStageId = stages[i].Patient_Journey_Stages_Id;
                journeyStage.DispPopulationStatistics = stages[i].Population_Statistics == 0 ? "" : stages[i].Population_Statistics.ToString();
                journeyStage.StageOrder = Convert.ToInt32(stages[i].Stage_Display_Order);
                journeyStage.StageTitle = stages[i].Stage_Title;

                journeyStage.TimeStatistics = stages[i].Time_Statistics;
                journeyStage.TimeScale = 1;
                if (stages[i].Time_Statistics == 0)
                {
                    journeyStage.TimeScale = 0;
                }
                else if (stages[i].Time_Statistics <= 24)
                {
                    journeyStage.DisplayTimeScale = (stages[i].Time_Statistics == 0) ? "" : (stages[i].Time_Statistics == 1) ? stages[i].Time_Statistics + " month".ToString() : stages[i].Time_Statistics + " months".ToString();
                }
                else if (stages[i].Time_Statistics > 24)
                {
                    string year = (stages[i].Time_Statistics / 12) + " years".ToString();
                    string month = stages[i].Time_Statistics % 12 == 0 ? "" : stages[i].Time_Statistics % 12 == 1 ? " and " + (stages[i].Time_Statistics % 12) + " month".ToString() : " and " + (stages[i].Time_Statistics % 12) + " months".ToString();
                    journeyStage.DisplayTimeScale = year + month;
                }

                if (stages[i].Population_Statistics >= 1000 && stages[i].Population_Statistics < 1000000)
                {
                    if (stages[i].Population_Statistics % 1000 == 0)
                    {
                        journeyStage.PopulationScale = 1000;
                        journeyStage.PopulationStatistics = stages[i].Population_Statistics / 1000;
                    }
                    else
                    {
                        journeyStage.PopulationScale = 100;
                        journeyStage.PopulationStatistics = stages[i].Population_Statistics / 100;
                    }
                }
                else if (stages[i].Population_Statistics >= 1000000)
                {
                    if (stages[i].Population_Statistics % 1000000 == 0)
                    {
                        journeyStage.PopulationScale = 1000000;
                        journeyStage.PopulationStatistics = stages[i].Population_Statistics / 1000000;
                    }
                    else if (stages[i].Population_Statistics % 1000 == 0)
                    {
                        journeyStage.PopulationScale = 1000;
                        journeyStage.PopulationStatistics = stages[i].Population_Statistics / 1000;
                    }
                    else
                    {
                        journeyStage.PopulationScale = 100;
                        journeyStage.PopulationStatistics = stages[i].Population_Statistics / 100;
                    }
                }
                else if (stages[i].Population_Statistics == 0)
                {
                    journeyStage.PopulationScale = 0;
                    journeyStage.PopulationStatistics = 0;
                }
                else
                {
                    journeyStage.PopulationScale = 100;
                    journeyStage.PopulationStatistics = stages[i].Population_Statistics / 100;
                }


                journeyStage.StageMasterId = Convert.ToInt32(stages[i].Stage_Master_Id);
                journeyStage.TransactionCount = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Id == journeyStage.PatientJourneyId && x.Patient_Journey_Stages_Id == journeyStage.PatientStageId).Count();
                listJourneyStage.Add(journeyStage);
            }
            return listJourneyStage;

        }

        public static List<JourneyStage> GetJourneyStageFromTemp(string JourneyId)
        {
            List<JourneyStage> listJourneyStage = new List<JourneyStage>();
            PJEntities entity = new PJEntities();
            var stages = dbPatientAdministration.GetJourneyStageFromTemp(Convert.ToInt32(JourneyId));
            for (int i = 0; i < stages.Count; i++)
            {
                JourneyStage journeyStage = new JourneyStage();
                journeyStage.PatientJourneyId = Convert.ToInt32(stages[i].Patient_Journey_Temp_Id);
                journeyStage.PatientStageId = stages[i].Patient_Journey_Stages_Temp_Id;
                journeyStage.DispPopulationStatistics = stages[i].Population_Statistics == 0 ? "" : stages[i].Population_Statistics.ToString();
                journeyStage.StageOrder = Convert.ToInt32(stages[i].Stage_Display_Order);
                journeyStage.StageTitle = stages[i].Stage_Title;

                journeyStage.TimeStatistics = stages[i].Time_Statistics;
                journeyStage.TimeScale = 1;
                if (stages[i].Time_Statistics == 0)
                {
                    journeyStage.TimeScale = 0;
                    journeyStage.DisplayTimeScale = "";
                }
                else if (stages[i].Time_Statistics <= 24)
                {
                    //journeyStage.DisplayTimeScale = (stages[i].Time_Statistics == 0 || stages[i].Time_Statistics == 1) ? stages[i].Time_Statistics + " month".ToString() : stages[i].Time_Statistics + " months".ToString();
                    journeyStage.DisplayTimeScale = (stages[i].Time_Statistics == 0) ? "" : (stages[i].Time_Statistics == 1) ? stages[i].Time_Statistics + " month".ToString() : stages[i].Time_Statistics + " months".ToString();
                }
                else if (stages[i].Time_Statistics > 24)
                {
                    string year = (stages[i].Time_Statistics / 12) + " years".ToString();
                    string month = stages[i].Time_Statistics % 12 == 0 ? "" : stages[i].Time_Statistics % 12 == 1 ? " and " + (stages[i].Time_Statistics % 12) + " month".ToString() : " and " + (stages[i].Time_Statistics % 12) + " months".ToString();
                    journeyStage.DisplayTimeScale = year + month;
                }

                if (stages[i].Population_Statistics >= 1000 && stages[i].Population_Statistics < 1000000)
                {
                    if (stages[i].Population_Statistics % 1000 == 0)
                    {
                        journeyStage.PopulationScale = 1000;
                        journeyStage.PopulationStatistics = stages[i].Population_Statistics / 1000;
                    }
                    else
                    {
                        journeyStage.PopulationScale = 100;
                        journeyStage.PopulationStatistics = stages[i].Population_Statistics / 100;
                    }
                }
                else if (stages[i].Population_Statistics >= 1000000)
                {
                    if (stages[i].Population_Statistics % 1000000 == 0)
                    {
                        journeyStage.PopulationScale = 1000000;
                        journeyStage.PopulationStatistics = stages[i].Population_Statistics / 1000000;
                    }
                    else if (stages[i].Population_Statistics % 1000 == 0)
                    {
                        journeyStage.PopulationScale = 1000;
                        journeyStage.PopulationStatistics = stages[i].Population_Statistics / 1000;
                    }
                    else
                    {
                        journeyStage.PopulationScale = 100;
                        journeyStage.PopulationStatistics = stages[i].Population_Statistics / 100;
                    }
                }
                else if (stages[i].Population_Statistics == 0)
                {
                    journeyStage.PopulationScale = 0;
                    journeyStage.PopulationStatistics = 0;
                }
                else
                {
                    journeyStage.PopulationScale = 100;
                    journeyStage.PopulationStatistics = stages[i].Population_Statistics / 100;
                }


                journeyStage.StageMasterId = Convert.ToInt32(stages[i].Stage_Master_Id);
                journeyStage.TransactionCount = entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Temp_Id == journeyStage.PatientJourneyId && x.Patient_Journey_Stages_Temp_Id == journeyStage.PatientStageId).Count();
                listJourneyStage.Add(journeyStage);
            }
            return listJourneyStage;

        }

        public static Int32? ReorderStageToTemp(string[] StageOrders, string JourneyId, int StatusId, string User511)
        {
            try
            {
                int[] oldOrder = new int[StageOrders.Length];
                int[] newOrder = new int[StageOrders.Length];
                for (int i = 0; i < StageOrders.Length; i++)
                {
                    newOrder[i] = Convert.ToInt32(StageOrders[i].Split(':')[0]);
                    oldOrder[i] = Convert.ToInt32(StageOrders[i].Split(':')[1]);
                }
                var response = dbPatientAdministration.ReorderStageToTemp(newOrder, oldOrder, Convert.ToInt32(JourneyId));
                if (response != null)
                {
                    Patient_Journey_VersionDetails_Temp versionDetails = new Patient_Journey_VersionDetails_Temp();
                    versionDetails.Patient_Journey_Temp_Id = Convert.ToInt32(JourneyId);
                    versionDetails.Version_Comments = GlobalConstants.VersionCommentsConstants.StageReorder;
                    versionDetails.Version_Title = GlobalConstants.VersionTitleConstants.StageReorder;
                    versionDetails.Created_By = User511;
                    versionDetails.Created_Date = DateTime.Now;
                    versionDetails.Modified_By = User511;
                    versionDetails.Modified_Date = DateTime.Now;
                    versionDetails.IsApproved = false;
                    var version = dbPatientAdministration.AddVersionDetailsToTemp(versionDetails);
                    var toBeApproved = dbPatientAdministration.ToBeApprovedJourney(Convert.ToInt32(JourneyId));

                }
                return response;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int32? AddJourneyStageToTemp(JourneyStage journeyStage, int StatusId, string User511)
        {
            PJEntities entity = new PJEntities();
            Patient_Journey_Stages_Temp patientJourneyStage = new Patient_Journey_Stages_Temp();
            int stageCount = entity.Patient_Journey_Stages_Temp.Where(x => x.Patient_Journey_Temp_Id == journeyStage.PatientJourneyId).Count();
            patientJourneyStage.Patient_Journey_Temp_Id = journeyStage.PatientJourneyId;
            patientJourneyStage.Stage_Master_Id = journeyStage.StageMasterId;
            patientJourneyStage.Stage_Title = journeyStage.StageTitle == null ? String.Empty : journeyStage.StageTitle.ToString();
            patientJourneyStage.Stage_Display_Order = stageCount + 1;
            patientJourneyStage.Time_Statistics = journeyStage.TimeStatistics * journeyStage.TimeScale;
            patientJourneyStage.Population_Statistics = journeyStage.PopulationStatistics * journeyStage.PopulationScale;
            patientJourneyStage.Created_By = User511;
            patientJourneyStage.Created_Date = DateTime.Now;
            patientJourneyStage.Modified_By = User511;
            patientJourneyStage.Modified_Date = DateTime.Now;
            var response = dbPatientAdministration.AddStageToTemp(patientJourneyStage);
            if (response != null)
            {
                Patient_Journey_VersionDetails_Temp versionDetails = new Patient_Journey_VersionDetails_Temp();
                versionDetails.Patient_Journey_Temp_Id = journeyStage.PatientJourneyId;
                versionDetails.Version_Comments = "New stage " + journeyStage.StageTitle + "  added";
                versionDetails.Version_Title = GlobalConstants.VersionTitleConstants.StageAdded;
                versionDetails.Created_By = User511;
                versionDetails.Created_Date = DateTime.Now;
                versionDetails.Modified_By = User511;
                versionDetails.Modified_Date = DateTime.Now;
                versionDetails.IsApproved = false;
                var version = dbPatientAdministration.AddVersionDetailsToTemp(versionDetails);
                var toBeApproved = dbPatientAdministration.ToBeApprovedJourney(journeyStage.PatientJourneyId);

            }
            return response;
        }

        public static Int32? UpdateJourneyStageToTemp(JourneyStage journeyStage, int StatusId, string User511)
        {
            Patient_Journey_Stages_Temp patientJourneyStage = new Patient_Journey_Stages_Temp();
            patientJourneyStage.Patient_Journey_Temp_Id = journeyStage.PatientJourneyId; //Id added
            patientJourneyStage.Patient_Journey_Stages_Temp_Id = journeyStage.PatientStageId;
            if (journeyStage.StageOrder != 0)
            {
                patientJourneyStage.Stage_Display_Order = journeyStage.StageOrder;
            }
            patientJourneyStage.Patient_Journey_Temp_Id = journeyStage.PatientJourneyId;
            patientJourneyStage.Time_Statistics = journeyStage.TimeStatistics * journeyStage.TimeScale;
            patientJourneyStage.Population_Statistics = journeyStage.PopulationStatistics * journeyStage.PopulationScale;
            patientJourneyStage.Modified_By = User511;
            patientJourneyStage.Modified_Date = DateTime.Now;
            var response = dbPatientAdministration.UpdateJourneyStageToTemp(patientJourneyStage);
            if (response != null)
            {
                Patient_Journey_VersionDetails_Temp versionDetails = new Patient_Journey_VersionDetails_Temp();
                versionDetails.Patient_Journey_Temp_Id = journeyStage.PatientJourneyId;
                versionDetails.Version_Comments = journeyStage.StageTitle + " updated";
                versionDetails.Version_Title = GlobalConstants.VersionTitleConstants.StageUpdated;
                versionDetails.Created_By = User511;
                versionDetails.Created_Date = DateTime.Now;
                versionDetails.Modified_By = User511;
                versionDetails.Modified_Date = DateTime.Now;
                versionDetails.IsApproved = false;
                var version = dbPatientAdministration.AddVersionDetailsToTemp(versionDetails);
                var toBeApproved = dbPatientAdministration.ToBeApprovedJourney(journeyStage.PatientJourneyId);

            }
            return response;
        }

        public static Int32? RemoveJourneyStageToTemp(string JourneyId, string StageId, int StatusId, string User511)
        {
            var response = dbPatientAdministration.RemoveJourneyStageToTemp(Convert.ToInt32(JourneyId), Convert.ToInt32(StageId));
            if (response == 1)
            {
                Patient_Journey_VersionDetails_Temp versionDetails = new Patient_Journey_VersionDetails_Temp();
                versionDetails.Patient_Journey_Temp_Id = Convert.ToInt32(JourneyId);
                versionDetails.Version_Comments = GlobalConstants.VersionCommentsConstants.StageRemoved;
                versionDetails.Version_Title = GlobalConstants.VersionTitleConstants.StageRemoved;
                versionDetails.Created_By = User511;
                versionDetails.Created_Date = DateTime.Now;
                versionDetails.Modified_By = User511;
                versionDetails.Modified_Date = DateTime.Now;
                versionDetails.IsApproved = false;
                var version = dbPatientAdministration.AddVersionDetailsToTemp(versionDetails);
                var toBeApproved = dbPatientAdministration.ToBeApprovedJourney(Convert.ToInt32(JourneyId));

            }
            return response;
        }

        public static Int32? ReorderTransaction(string[] TransactionOrders, string StageId, int JourneyId, int StatusId, string User511)
        {
            try
            {
                int[] oldOrder = new int[TransactionOrders.Length];
                int[] newOrder = new int[TransactionOrders.Length];
                for (int i = 0; i < TransactionOrders.Length; i++)
                {
                    newOrder[i] = Convert.ToInt32(TransactionOrders[i].Split(':')[0]);
                    oldOrder[i] = Convert.ToInt32(TransactionOrders[i].Split(':')[1]);
                }
                var response = dbPatientAdministration.ReorderTransaction(newOrder, oldOrder, Convert.ToInt32(StageId));
                return response;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static TransactionInfo AddTransaction(Transaction transaction, int StatusId, string User511)
        {
            PJEntities entity = new PJEntities();
            Patient_Journey_Transactions patientTransaction = new Patient_Journey_Transactions();
            int transactionCount = entity.Patient_Journey_Transactions.Where(x => x.Patient_Journey_Id == transaction.PatientJourneyId && x.Patient_Journey_Stages_Id == transaction.PatientStageId).Count();
            patientTransaction.Patient_Journey_Id = transaction.PatientJourneyId;
            patientTransaction.Patient_Journey_Stages_Id = transaction.PatientStageId;
            patientTransaction.Transaction_Master_Id = transaction.TransactionMasterId;
            patientTransaction.Transaction_Title = transaction.TransactionTitle;
            patientTransaction.Description = transaction.TransactionDescription;
            patientTransaction.Transaction_Display_Order = transactionCount + 1;
            patientTransaction.Transaction_Location_Master_Id = transaction.LocationId;
            patientTransaction.Transaction_Location_Title = transaction.LocationName;
            patientTransaction.HCP_Rating = transaction.HCPRating;
            patientTransaction.Payer_Rating = transaction.PayerRating;
            patientTransaction.Patient_Rating = transaction.PatientRating;
            patientTransaction.Feasibility_Rating = transaction.FeasibilityRating;
            patientTransaction.Viability_Rating = transaction.ViabilityRating;
            patientTransaction.HCP_Description = transaction.HCPDescription;
            patientTransaction.Payer_Description = transaction.PayerDescription;
            patientTransaction.Patient_Description = transaction.PatientDescription;
            patientTransaction.Feasibility_Description = transaction.FeasibilityDescription;
            patientTransaction.Viability_Description = transaction.ViabilityDescription;
            patientTransaction.Created_By = User511;
            patientTransaction.Created_Date = DateTime.Now;
            patientTransaction.Modified_By = User511;
            patientTransaction.Modified_Date = DateTime.Now;
            int TransactionCount;
            var listresponse = dbPatientAdministration.AddTransaction(patientTransaction, out TransactionCount);
            TransactionInfo transactionInfo = null;
            if (listresponse != null)
            {
                transactionInfo = new TransactionInfo();
                transactionInfo.Transactionid = listresponse;
                transactionInfo.TransactionCount = TransactionCount;
            }

            return transactionInfo;
        }

        public static TransactionInfo UpdateTransaction(Transaction transaction, int StatusId, string User511)
        {
            Patient_Journey_Transactions patientTransaction = new Patient_Journey_Transactions();
            patientTransaction.Patient_Journey_Transactions_Id = transaction.PatientJourneyTransactionId;
            patientTransaction.Patient_Journey_Stages_Id = transaction.PatientStageId;
            patientTransaction.Transaction_Master_Id = transaction.TransactionMasterId;
            patientTransaction.Transaction_Title = transaction.TransactionTitle;
            patientTransaction.Description = transaction.TransactionDescription;
            patientTransaction.Transaction_Location_Master_Id = transaction.LocationId;
            patientTransaction.Transaction_Location_Title = transaction.LocationName;
            patientTransaction.HCP_Rating = transaction.HCPRating;
            patientTransaction.Payer_Rating = transaction.PayerRating;
            patientTransaction.Patient_Rating = transaction.PatientRating;
            patientTransaction.Feasibility_Rating = transaction.FeasibilityRating;
            patientTransaction.Viability_Rating = transaction.ViabilityRating;
            patientTransaction.HCP_Description = transaction.HCPDescription;
            patientTransaction.Payer_Description = transaction.PayerDescription;
            patientTransaction.Patient_Description = transaction.PatientDescription;
            patientTransaction.Feasibility_Description = transaction.FeasibilityDescription;
            patientTransaction.Viability_Description = transaction.ViabilityDescription;
            patientTransaction.Modified_By = User511;
            patientTransaction.Modified_Date = DateTime.Now;
            int TransactionCount;
            var listresponse = dbPatientAdministration.UpdateTransaction(patientTransaction, out TransactionCount);

            TransactionInfo transactionInfo = null;
            if (listresponse != null)
            {
                transactionInfo = new TransactionInfo();
                transactionInfo.Transactionid = listresponse;
                transactionInfo.TransactionCount = TransactionCount;
            }

            return transactionInfo;
        }

        public static List<Transaction> GetTransactions(string StageId)
        {
            List<Transaction> listTransactions = new List<Transaction>();
            var transaction = dbPatientAdministration.GetTransactions(Convert.ToInt32(StageId));
            for (int i = 0; i < transaction.Count; i++)
            {
                Transaction patienttransaction = new Transaction();
                patienttransaction.PatientJourneyTransactionId = transaction[i].Patient_Journey_Transactions_Id;
                patienttransaction.PatientStageId = Convert.ToInt32(transaction[i].Patient_Journey_Stages_Id);
                patienttransaction.PatientJourneyId = Convert.ToInt32(transaction[i].Patient_Journey_Id);
                patienttransaction.TransactionMasterId = Convert.ToInt32(transaction[i].Transaction_Master_Id);
                patienttransaction.TransactionTitle = transaction[i].Transaction_Title;
                patienttransaction.LocationId = Convert.ToInt32(transaction[i].Transaction_Location_Master_Id);
                patienttransaction.LocationName = transaction[i].Transaction_Location_Title;
                patienttransaction.TransactionDescription = transaction[i].Description;
                patienttransaction.DisplayOrder = Convert.ToInt32(transaction[i].Transaction_Display_Order);
                patienttransaction.HCPDescription = transaction[i].HCP_Description;
                patienttransaction.HCPRating = transaction[i].HCP_Rating;
                patienttransaction.PatientDescription = transaction[i].Patient_Description;
                patienttransaction.PatientRating = transaction[i].Patient_Rating;
                patienttransaction.PayerDescription = transaction[i].Payer_Description;
                patienttransaction.PayerRating = transaction[i].Payer_Rating;
                patienttransaction.FeasibilityDescription = transaction[i].Feasibility_Description;
                patienttransaction.FeasibilityRating = transaction[i].Feasibility_Rating;
                patienttransaction.ViabilityDescription = transaction[i].Viability_Description;
                patienttransaction.ViabilityRating = transaction[i].Viability_Rating;
                patienttransaction.PatientEvidence = transaction[i].Patient_Evidence;
                patienttransaction.HCPEvidence = transaction[i].HCP_Evidence;
                patienttransaction.PayerEvidence = transaction[i].Payer_Evidence;
                patienttransaction.FeasibilityEvidence = transaction[i].Feasibility_Evidence;
                patienttransaction.ViabilityEvidence = transaction[i].Viability_Evidence;

                patienttransaction.ImageMasterId = Convert.ToInt32(dbPatientAdministration.GetImageMasterID(patienttransaction.TransactionMasterId));
                if (patienttransaction.ImageMasterId != 0)
                {
                    patienttransaction.ImagePath = dbPatientAdministration.GetImagePath(patienttransaction.ImageMasterId);
                }
                patienttransaction.LocationImageMasterId = Convert.ToInt32(dbPatientAdministration.GetLocationImageMasterID(patienttransaction.LocationId));
                if (patienttransaction.LocationImageMasterId != 0)
                {
                    patienttransaction.LocationImagePath = dbPatientAdministration.GetImagePath(patienttransaction.LocationImageMasterId);
                }
                listTransactions.Add(patienttransaction);
            }
            return listTransactions;
        }

        public static List<Transaction> GetTransactionsFromTemp(string StageId)
        {
            List<Transaction> listTransactions = new List<Transaction>();
            var transaction = dbPatientAdministration.GetTransactionsFromTemp(Convert.ToInt32(StageId));
            for (int i = 0; i < transaction.Count; i++)
            {
                Transaction patienttransaction = new Transaction();
                patienttransaction.PatientJourneyTransactionId = transaction[i].Patient_Journey_Transactions_Temp_Id;
                patienttransaction.PatientStageId = Convert.ToInt32(transaction[i].Patient_Journey_Stages_Temp_Id);
                patienttransaction.PatientJourneyId = Convert.ToInt32(transaction[i].Patient_Journey_Temp_Id);
                patienttransaction.TransactionMasterId = Convert.ToInt32(transaction[i].Transaction_Master_Id);
                patienttransaction.TransactionTitle = transaction[i].Transaction_Title;
                patienttransaction.LocationId = Convert.ToInt32(transaction[i].Transaction_Location_Master_Id);
                patienttransaction.LocationName = transaction[i].Transaction_Location_Title;
                patienttransaction.TransactionDescription = transaction[i].Description;
                patienttransaction.DisplayOrder = Convert.ToInt32(transaction[i].Transaction_Display_Order);
                patienttransaction.HCPDescription = transaction[i].HCP_Description;
                patienttransaction.HCPRating = transaction[i].HCP_Rating;
                patienttransaction.PatientDescription = transaction[i].Patient_Description;
                patienttransaction.PatientRating = transaction[i].Patient_Rating;
                patienttransaction.PayerDescription = transaction[i].Payer_Description;
                patienttransaction.PayerRating = transaction[i].Payer_Rating;
                patienttransaction.FeasibilityDescription = transaction[i].Feasibility_Description;
                patienttransaction.FeasibilityRating = transaction[i].Feasibility_Rating;
                patienttransaction.ViabilityDescription = transaction[i].Viability_Description;
                patienttransaction.ViabilityRating = transaction[i].Viability_Rating;
                patienttransaction.PatientEvidence = transaction[i].Patient_Evidence;
                patienttransaction.HCPEvidence = transaction[i].HCP_Evidence;
                patienttransaction.PayerEvidence = transaction[i].Payer_Evidence;
                patienttransaction.FeasibilityEvidence = transaction[i].Feasibility_Evidence;
                patienttransaction.ViabilityEvidence = transaction[i].Viability_Evidence;
                patienttransaction.ImageMasterId = Convert.ToInt32(dbPatientAdministration.GetImageMasterID(patienttransaction.TransactionMasterId));
                if (patienttransaction.ImageMasterId != 0)
                {
                    patienttransaction.ImagePath = dbPatientAdministration.GetImagePath(patienttransaction.ImageMasterId);
                }
                patienttransaction.LocationImageMasterId = Convert.ToInt32(dbPatientAdministration.GetLocationImageMasterID(patienttransaction.LocationId));
                if (patienttransaction.LocationImageMasterId != 0)
                {
                    patienttransaction.LocationImagePath = dbPatientAdministration.GetImagePath(patienttransaction.LocationImageMasterId);
                }
                listTransactions.Add(patienttransaction);
            }
            return listTransactions;
        }

        public static Int32? RemoveTransactions(string[] arrTransactionId, string StageId, int JourneyId, int StatusId, string User511)
        {
            List<int> lstTransactionId = new List<int>();
            for (int i = 0; i < arrTransactionId.Length; i++)
            {
                int temp = Convert.ToInt32(arrTransactionId[i].Trim());
                lstTransactionId.Add(temp);
            }
            var response = dbPatientAdministration.RemoveTransactions(lstTransactionId, Convert.ToInt32(StageId));
            return response;
        }

        public static Int32? ReorderTransactionToTemp(string[] TransactionOrders, string StageId, int JourneyId, int StatusId, string User511)
        {
            try
            {
                int[] oldOrder = new int[TransactionOrders.Length];
                int[] newOrder = new int[TransactionOrders.Length];
                for (int i = 0; i < TransactionOrders.Length; i++)
                {
                    newOrder[i] = Convert.ToInt32(TransactionOrders[i].Split(':')[0]);
                    oldOrder[i] = Convert.ToInt32(TransactionOrders[i].Split(':')[1]);
                }
                var response = dbPatientAdministration.ReorderTransactionToTemp(newOrder, oldOrder, Convert.ToInt32(StageId));
                if (response != null)
                {
                    Patient_Journey_VersionDetails_Temp versionDetails = new Patient_Journey_VersionDetails_Temp();
                    versionDetails.Patient_Journey_Temp_Id = Convert.ToInt32(JourneyId);
                    versionDetails.Version_Comments = GlobalConstants.VersionCommentsConstants.TransactionReorder;
                    versionDetails.Version_Title = GlobalConstants.VersionTitleConstants.TransactionReorder;
                    versionDetails.Created_By = User511;
                    versionDetails.Created_Date = DateTime.Now;
                    versionDetails.Modified_By = User511;
                    versionDetails.Modified_Date = DateTime.Now;
                    versionDetails.IsApproved = false;
                    var version = dbPatientAdministration.AddVersionDetailsToTemp(versionDetails);
                    var toBeApproved = dbPatientAdministration.ToBeApprovedJourney(Convert.ToInt32(JourneyId));

                }
                return response;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static TransactionInfo AddTransactionToTemp(Transaction transaction, int StatusId, string User511)
        {
            PJEntities entity = new PJEntities();
            Patient_Journey_Transactions_Temp patientTransaction = new Patient_Journey_Transactions_Temp();
            int transactionCount = entity.Patient_Journey_Transactions_Temp.Where(x => x.Patient_Journey_Temp_Id == transaction.PatientJourneyId && x.Patient_Journey_Stages_Temp_Id == transaction.PatientStageId).Count();
            patientTransaction.Patient_Journey_Temp_Id = transaction.PatientJourneyId;
            patientTransaction.Patient_Journey_Stages_Temp_Id = transaction.PatientStageId;
            patientTransaction.Transaction_Master_Id = transaction.TransactionMasterId;
            patientTransaction.Transaction_Title = transaction.TransactionTitle;
            patientTransaction.Description = transaction.TransactionDescription;
            patientTransaction.Transaction_Display_Order = transactionCount + 1;
            patientTransaction.Transaction_Location_Master_Id = transaction.LocationId;
            patientTransaction.Transaction_Location_Title = transaction.LocationName;
            patientTransaction.HCP_Rating = transaction.HCPRating;
            patientTransaction.Payer_Rating = transaction.PayerRating;
            patientTransaction.Patient_Rating = transaction.PatientRating;
            patientTransaction.Feasibility_Rating = transaction.FeasibilityRating;
            patientTransaction.Viability_Rating = transaction.ViabilityRating;
            patientTransaction.HCP_Description = transaction.HCPDescription;
            patientTransaction.Payer_Description = transaction.PayerDescription;
            patientTransaction.Patient_Description = transaction.PatientDescription;
            patientTransaction.Feasibility_Description = transaction.FeasibilityDescription;
            patientTransaction.Viability_Description = transaction.ViabilityDescription;
            patientTransaction.Created_By = User511;
            patientTransaction.Created_Date = DateTime.Now;
            patientTransaction.Modified_By = User511;
            patientTransaction.Modified_Date = DateTime.Now;
            int TransactionCount;
            var listresponse = dbPatientAdministration.AddTransactionToTemp(patientTransaction, out TransactionCount);
            if (listresponse != null)
            {
                Patient_Journey_VersionDetails_Temp versionDetails = new Patient_Journey_VersionDetails_Temp();
                versionDetails.Patient_Journey_Temp_Id = transaction.PatientJourneyId;
                versionDetails.Version_Comments = GlobalConstants.VersionCommentsConstants.TransactionAdded;
                versionDetails.Version_Title = GlobalConstants.VersionTitleConstants.TransactionAdded;
                versionDetails.Created_By = User511;
                versionDetails.Created_Date = DateTime.Now;
                versionDetails.Modified_By = User511;
                versionDetails.Modified_Date = DateTime.Now;
                versionDetails.IsApproved = false;
                var version = dbPatientAdministration.AddVersionDetailsToTemp(versionDetails);
                var toBeApproved = dbPatientAdministration.ToBeApprovedJourney(transaction.PatientJourneyId);

            }

            TransactionInfo transactionInfo = null;
            if (listresponse != null)
            {
                transactionInfo = new TransactionInfo();
                transactionInfo.Transactionid = listresponse;
                transactionInfo.TransactionCount = TransactionCount;
            }

            return transactionInfo;
        }

        public static TransactionInfo UpdateTransactionToTemp(Transaction transaction, int StatusId, string User511)
        {
            Patient_Journey_Transactions_Temp patientTransaction = new Patient_Journey_Transactions_Temp();
            patientTransaction.Patient_Journey_Transactions_Temp_Id = transaction.PatientJourneyTransactionId;
            patientTransaction.Patient_Journey_Stages_Temp_Id = transaction.PatientStageId;
            patientTransaction.Description = transaction.TransactionDescription;
            patientTransaction.Transaction_Master_Id = transaction.TransactionMasterId;
            patientTransaction.Transaction_Title = transaction.TransactionTitle;
            patientTransaction.Transaction_Location_Master_Id = transaction.LocationId;
            patientTransaction.Transaction_Location_Title = transaction.LocationName;
            patientTransaction.HCP_Rating = transaction.HCPRating;
            patientTransaction.Payer_Rating = transaction.PayerRating;
            patientTransaction.Patient_Rating = transaction.PatientRating;
            patientTransaction.Feasibility_Rating = transaction.FeasibilityRating;
            patientTransaction.Viability_Rating = transaction.ViabilityRating;
            patientTransaction.HCP_Description = transaction.HCPDescription;
            patientTransaction.Payer_Description = transaction.PayerDescription;
            patientTransaction.Patient_Description = transaction.PatientDescription;
            patientTransaction.Feasibility_Description = transaction.FeasibilityDescription;
            patientTransaction.Viability_Description = transaction.ViabilityDescription;
            patientTransaction.Modified_By = User511;
            patientTransaction.Modified_Date = DateTime.Now;
            int TransactionCount;
            var listresponse = dbPatientAdministration.UpdateTransactionToTemp(patientTransaction, out TransactionCount);
            if (listresponse != null)
            {
                Patient_Journey_VersionDetails_Temp versionDetails = new Patient_Journey_VersionDetails_Temp();
                versionDetails.Patient_Journey_Temp_Id = transaction.PatientJourneyId;
                versionDetails.Version_Comments = GlobalConstants.VersionCommentsConstants.TransactionUpdated;
                versionDetails.Version_Title = GlobalConstants.VersionTitleConstants.TransactionUpdated;
                versionDetails.Created_By = User511;
                versionDetails.Created_Date = DateTime.Now;
                versionDetails.Modified_By = User511;
                versionDetails.Modified_Date = DateTime.Now;
                versionDetails.IsApproved = false;
                var version = dbPatientAdministration.AddVersionDetailsToTemp(versionDetails);
                var toBeApproved = dbPatientAdministration.ToBeApprovedJourney(transaction.PatientJourneyId);

            }

            TransactionInfo transactionInfo = null;
            if (listresponse != null)
            {
                transactionInfo = new TransactionInfo();
                transactionInfo.Transactionid = listresponse;
                transactionInfo.TransactionCount = TransactionCount;
            }

            return transactionInfo;
        }

        public static Int32? RemoveTransactionsToTemp(string[] arrTransactionId, string StageId, int JourneyId, int StatusId, string User511)
        {
            List<int> lstTransactionId = new List<int>();
            for (int i = 0; i < arrTransactionId.Length; i++)
            {
                int temp = Convert.ToInt32(arrTransactionId[i].Trim());
                lstTransactionId.Add(temp);
            }
            var response = dbPatientAdministration.RemoveTransactionsToTemp(lstTransactionId, Convert.ToInt32(StageId));
            if (response == 1)
            {

                Patient_Journey_VersionDetails_Temp versionDetails = new Patient_Journey_VersionDetails_Temp();
                versionDetails.Patient_Journey_Temp_Id = Convert.ToInt32(JourneyId);
                versionDetails.Version_Comments = GlobalConstants.VersionCommentsConstants.TransactionRemoved;
                versionDetails.Version_Title = GlobalConstants.VersionTitleConstants.TransactionRemoved;
                versionDetails.Created_By = User511;
                versionDetails.Created_Date = DateTime.Now;
                versionDetails.Modified_By = User511;
                versionDetails.Modified_Date = DateTime.Now;
                versionDetails.IsApproved = false;
                var version = dbPatientAdministration.AddVersionDetailsToTemp(versionDetails);
                var toBeApproved = dbPatientAdministration.ToBeApprovedJourney(Convert.ToInt32(JourneyId));

            }

            return response;
        }

        public static List<JourneySearchResult> GetCloneJourneyList(string BrandId)
        {
            List<Patient_Journey> result = new List<Patient_Journey>();
            List<JourneySearchResult> listJourney = new List<JourneySearchResult>();
            PJEntities entity = new PJEntities();
            JourneySearchResult journey = null;
            result = dbPatientAdministration.GetCloneJourneyList(Convert.ToInt32(BrandId));
            if (result != null)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    journey = new JourneySearchResult();
                    journey.JourneyNumber = i + 1;
                    journey.JourneyId = result[i].Patient_Journey_Id;
                    journey.JourneyName = result[i].Journey_Title;
                    journey.BrandId = Convert.ToInt32(result[i].Brand_Master_Id);
                    journey.BrandName = entity.Brand_Master.Where(x => x.Brand_Master_Id == journey.BrandId).FirstOrDefault().Brand_Name;
                    journey.CountryId = Convert.ToInt32(result[i].Country_Master_Id);
                    journey.CountryName = entity.Country_Master.Where(x => x.Country_Master_Id == journey.CountryId).FirstOrDefault().Country_Name;
                    journey.StatusId = Convert.ToInt32(result[i].Status_Master_Id);
                    journey.StatusName = entity.Status_Master.Where(x => x.Status_Master_Id == journey.StatusId).FirstOrDefault().Status_Name;
                    journey.Year = result[i].Year;
                    listJourney.Add(journey);
                }
            }
            return listJourney;
        }

        public static Int32? CloneFromOtherJourney(PatientJourneyModel journey, string User511)
        {
            Patient_Journey newJourney = new Patient_Journey();
            Patient_Journey patientJourney = dbPatientAdministration.GetJourneyDetails(journey.PatientJourneyId);
            newJourney.Brand_Master_Id = patientJourney.Brand_Master_Id;
            newJourney.Country_Master_Id = journey.CountryID;
            newJourney.Created_By = User511;
            newJourney.Created_Date = DateTime.Now;
            newJourney.Modified_By = User511;
            newJourney.Modified_Date = DateTime.Now;
            newJourney.Journey_Description = patientJourney.Journey_Description;
            newJourney.Status_Master_Id = 1;
            using (PJEntities entity = new PJEntities())
            {
                var brand = entity.Brand_Master.Where(x => x.Brand_Master_Id == patientJourney.Brand_Master_Id).FirstOrDefault().Brand_Name;
                var country = entity.Country_Master.Where(x => x.Country_Master_Id == journey.CountryID).FirstOrDefault().Country_Name;
                var indicationid = entity.Brand_Master.Where(x => x.Brand_Master_Id == patientJourney.Brand_Master_Id).FirstOrDefault().Indication_Master_Id;
                var indication = entity.Indication_Master.Where(x => x.Indication_Master_Id == indicationid).FirstOrDefault().Indication_Name;
                newJourney.Journey_Title = indication + " - " + brand + " - " + country;
            }

            if (journey.isCountryClone == 2)
            {
                newJourney.Year = journey.Year;
            }
            else
            {
                newJourney.Year = patientJourney.Year;
            }
            var response = dbPatientAdministration.AddJourney(newJourney);

            List<Patient_Journey_Stages> patientStages = dbPatientAdministration.GetJourneyStage(journey.PatientJourneyId);
            Patient_Journey_Stages stage = null;
            if (patientStages != null)
            {
                for (int i = 0; i < patientStages.Count; i++)
                {
                    stage = new Patient_Journey_Stages();
                    stage.Patient_Journey_Id = response;
                    stage.Description = patientStages[i].Description;
                    stage.Population_Statistics = patientStages[i].Population_Statistics;
                    stage.Time_Statistics = patientStages[i].Time_Statistics;
                    stage.Stage_Display_Order = patientStages[i].Stage_Display_Order;
                    stage.Stage_Master_Id = patientStages[i].Stage_Master_Id;
                    stage.Stage_Title = patientStages[i].Stage_Title;
                    stage.Created_By = User511;
                    stage.Created_Date = DateTime.Now;
                    stage.Modified_By = User511;
                    stage.Modified_Date = DateTime.Now;
                    var stageresponse = dbPatientAdministration.AddJourneyStage(stage);
                    List<Patient_Journey_Transactions> patientTransactions = dbPatientAdministration.GetTransactions(patientStages[i].Patient_Journey_Stages_Id);
                    Patient_Journey_Transactions transaction = null;
                    if (patientTransactions != null)
                    {
                        for (int j = 0; j < patientTransactions.Count; j++)
                        {
                            transaction = new Patient_Journey_Transactions();
                            transaction.Patient_Journey_Id = response;
                            transaction.Patient_Journey_Stages_Id = stageresponse;
                            transaction.Description = patientTransactions[j].Description;
                            transaction.Feasibility_Description = patientTransactions[j].Feasibility_Description;
                            transaction.Feasibility_Rating = patientTransactions[j].Feasibility_Rating;
                            transaction.HCP_Description = patientTransactions[j].HCP_Description;
                            transaction.HCP_Rating = patientTransactions[j].HCP_Rating;
                            transaction.Patient_Description = patientTransactions[j].Patient_Description;
                            transaction.Patient_Rating = patientTransactions[j].Patient_Rating;
                            transaction.Payer_Description = patientTransactions[j].Payer_Description;
                            transaction.Payer_Rating = patientTransactions[j].Payer_Rating;
                            transaction.Transaction_Display_Order = patientTransactions[j].Transaction_Display_Order;
                            transaction.Transaction_Location_Master_Id = patientTransactions[j].Transaction_Location_Master_Id;
                            transaction.Transaction_Location_Title = patientTransactions[j].Transaction_Location_Title;
                            transaction.Transaction_Master_Id = patientTransactions[j].Transaction_Master_Id;
                            transaction.Transaction_Title = patientTransactions[j].Transaction_Title;
                            transaction.Viability_Description = patientTransactions[j].Viability_Description;
                            transaction.Viability_Rating = patientTransactions[j].Viability_Rating;
                            transaction.Created_By = User511;
                            transaction.Created_Date = DateTime.Now;
                            transaction.Modified_By = User511;
                            transaction.Modified_Date = DateTime.Now;
                            int TransactionCount;
                            var transactionresponse = dbPatientAdministration.AddTransaction(transaction, out TransactionCount);

                            Patient_Journey_Transactions_DesiredOutcomes desout = dbPatientAdministration.GetDesiredOutcome(patientTransactions[j].Patient_Journey_Transactions_Id);
                            if (desout != null)
                            {
                                Patient_Journey_Transactions_DesiredOutcomes dout = new Patient_Journey_Transactions_DesiredOutcomes();
                                dout.DesiredOutcomes = desout.DesiredOutcomes;
                                dout.Description = desout.Description;
                                dout.Patient_Journey_Transactions_Id = transactionresponse;
                                dout.Created_By = User511;
                                dout.Created_Date = DateTime.Now;
                                dout.MODIFIED_By = User511;
                                dout.MODIFIED_Date = DateTime.Now;
                                var desiredresponse = dbPatientAdministration.AddDesiredOutcome(dout);
                            }

                            Patient_Journey_Trans_Clin_Interventions clinint = dbPatientAdministration.GetClinicalIntervention(patientTransactions[j].Patient_Journey_Transactions_Id);
                            if (clinint != null)
                            {
                                Patient_Journey_Trans_Clin_Interventions cint = new Patient_Journey_Trans_Clin_Interventions();
                                cint.Clinical_Intervention_Master_Id = clinint.Clinical_Intervention_Master_Id;
                                cint.Description = clinint.Description;
                                cint.Patient_Journey_Transactions_Id = transactionresponse;
                                cint.Created_By = User511;
                                cint.Created_Date = DateTime.Now;
                                cint.Modified_By = User511;
                                cint.Modified_Date = DateTime.Now;
                                var clinresponse = dbPatientAdministration.AddClinicalIntervention(cint);
                                List<Patient_Journey_Trans_SubClin_Interventions> lstSubClinicalId = dbPatientJourney.GetSubClinicalIntervention(clinint.Patient_Journey_Trans_Clin_Interventions_Id);
                                if (lstSubClinicalId.Count > 0)
                                {
                                    List<Patient_Journey_Trans_SubClin_Interventions> lstsubclin = new List<Patient_Journey_Trans_SubClin_Interventions>();
                                    Patient_Journey_Trans_SubClin_Interventions subclin = null;
                                    for (int l = 0; l < lstSubClinicalId.Count; l++)
                                    {
                                        subclin = new Patient_Journey_Trans_SubClin_Interventions();
                                        subclin.Patient_Journey_Trans_Clin_Interventions_Id = Convert.ToInt32(clinresponse);
                                        subclin.SubClinical_Intervention_Master_Id = lstSubClinicalId[l].SubClinical_Intervention_Master_Id;
                                        subclin.Created_By = User511;
                                        subclin.Created_Date = DateTime.Now;
                                        subclin.Modified_By = User511;
                                        subclin.Modified_Date = DateTime.Now;
                                        lstsubclin.Add(subclin);
                                    }
                                    var subclinresponse = dbPatientAdministration.AddSubClinicalIntervention(lstsubclin);
                                }

                            }

                            Patient_Journey_Transactions_AssociatedCosts asscost = dbPatientAdministration.GetAssociatedCost(patientTransactions[j].Patient_Journey_Transactions_Id);
                            if (asscost != null)
                            {
                                Patient_Journey_Transactions_AssociatedCosts acost = new Patient_Journey_Transactions_AssociatedCosts();
                                acost.AssociatedCosts = asscost.AssociatedCosts;
                                acost.Description = asscost.Description;
                                acost.Patient_Journey_Transactions_Id = transactionresponse;
                                acost.Created_By = User511;
                                acost.Created_Date = DateTime.Now;
                                acost.Modified_By = User511;
                                acost.Modified_Date = DateTime.Now;
                                var associatedresponse = dbPatientAdministration.AddAssociatedCost(acost);
                            }
                        }
                    }
                }
            }
            return response;
        }

        public static Int32? CloneTemplateJourney(PatientJourneyModel journey, string User511)
        {
            Patient_Journey newJourney = new Patient_Journey();
            Int32? TemplateCountryId = dbPatientAdministration.GetCountryId("Global");
            Patient_Journey patientJourney = null;
            patientJourney = dbPatientJourney.GetApprovedJourney(Convert.ToInt32(TemplateCountryId), journey.BrandID, journey.Year);
            if (patientJourney == null)
            {
                List<YearMasterData> year = DefaultListBSForPJ.GetYearMasterData();
                if (year.Count > 1)
                {
                    for (int i = 0; i < year.Count; i++)
                    {
                        if (patientJourney == null)
                        {
                            patientJourney = dbPatientJourney.GetApprovedJourney(Convert.ToInt32(TemplateCountryId), journey.BrandID, year[i].YearName);
                        }
                        else
                        {
                            break;
                        }
                       
                    }
                }
            }
            if (patientJourney != null)
            {
                newJourney.Brand_Master_Id = patientJourney.Brand_Master_Id;
                newJourney.Country_Master_Id = journey.CountryID;
                newJourney.Created_By = User511;
                newJourney.Created_Date = DateTime.Now;
                newJourney.Modified_By = User511;
                newJourney.Modified_Date = DateTime.Now;
                newJourney.Journey_Description = patientJourney.Journey_Description;
                newJourney.Status_Master_Id = 1;
                newJourney.Year = journey.Year;
                using (PJEntities entity = new PJEntities())
                {
                    var brand = entity.Brand_Master.Where(x => x.Brand_Master_Id == patientJourney.Brand_Master_Id).FirstOrDefault().Brand_Name;
                    var country = entity.Country_Master.Where(x => x.Country_Master_Id == journey.CountryID).FirstOrDefault().Country_Name;
                    var indicationid = entity.Brand_Master.Where(x => x.Brand_Master_Id == patientJourney.Brand_Master_Id).FirstOrDefault().Indication_Master_Id;
                    var indication = entity.Indication_Master.Where(x => x.Indication_Master_Id == indicationid).FirstOrDefault().Indication_Name;
                    newJourney.Journey_Title = indication + " - " + brand + " - " + country;
                }

                var response = dbPatientAdministration.AddJourney(newJourney);

                List<Patient_Journey_Stages> patientStages = dbPatientAdministration.GetJourneyStage(patientJourney.Patient_Journey_Id);
                Patient_Journey_Stages stage = null;
                if (patientStages != null)
                {
                    for (int i = 0; i < patientStages.Count; i++)
                    {
                        stage = new Patient_Journey_Stages();
                        stage.Patient_Journey_Id = response;
                        stage.Description = patientStages[i].Description;
                        stage.Population_Statistics = patientStages[i].Population_Statistics;
                        stage.Time_Statistics = patientStages[i].Time_Statistics;
                        stage.Stage_Display_Order = patientStages[i].Stage_Display_Order;
                        stage.Stage_Master_Id = patientStages[i].Stage_Master_Id;
                        stage.Stage_Title = patientStages[i].Stage_Title;
                        stage.Created_By = User511;
                        stage.Created_Date = DateTime.Now;
                        stage.Modified_By = User511;
                        stage.Modified_Date = DateTime.Now;
                        var stageresponse = dbPatientAdministration.AddJourneyStage(stage);
                        List<Patient_Journey_Transactions> patientTransactions = dbPatientAdministration.GetTransactions(patientStages[i].Patient_Journey_Stages_Id);
                        Patient_Journey_Transactions transaction = null;
                        if (patientTransactions != null)
                        {
                            for (int j = 0; j < patientTransactions.Count; j++)
                            {
                                transaction = new Patient_Journey_Transactions();
                                transaction.Patient_Journey_Id = response;
                                transaction.Patient_Journey_Stages_Id = stageresponse;
                                transaction.Description = patientTransactions[j].Description;
                                transaction.Feasibility_Description = patientTransactions[j].Feasibility_Description;
                                transaction.Feasibility_Rating = patientTransactions[j].Feasibility_Rating;
                                transaction.HCP_Description = patientTransactions[j].HCP_Description;
                                transaction.HCP_Rating = patientTransactions[j].HCP_Rating;
                                transaction.Patient_Description = patientTransactions[j].Patient_Description;
                                transaction.Patient_Rating = patientTransactions[j].Patient_Rating;
                                transaction.Payer_Description = patientTransactions[j].Payer_Description;
                                transaction.Payer_Rating = patientTransactions[j].Payer_Rating;
                                transaction.Transaction_Display_Order = patientTransactions[j].Transaction_Display_Order;
                                transaction.Transaction_Location_Master_Id = patientTransactions[j].Transaction_Location_Master_Id;
                                transaction.Transaction_Location_Title = patientTransactions[j].Transaction_Location_Title;
                                transaction.Transaction_Master_Id = patientTransactions[j].Transaction_Master_Id;
                                transaction.Transaction_Title = patientTransactions[j].Transaction_Title;
                                transaction.Viability_Description = patientTransactions[j].Viability_Description;
                                transaction.Viability_Rating = patientTransactions[j].Viability_Rating;
                                transaction.Created_By = User511;
                                transaction.Created_Date = DateTime.Now;
                                transaction.Modified_By = User511;
                                transaction.Modified_Date = DateTime.Now;
                                int TransactionCount;
                                var transactionresponse = dbPatientAdministration.AddTransaction(transaction, out TransactionCount);

                                Patient_Journey_Transactions_DesiredOutcomes desout = dbPatientAdministration.GetDesiredOutcome(patientTransactions[j].Patient_Journey_Transactions_Id);
                                if (desout != null)
                                {
                                    Patient_Journey_Transactions_DesiredOutcomes dout = new Patient_Journey_Transactions_DesiredOutcomes();
                                    dout.DesiredOutcomes = desout.DesiredOutcomes;
                                    dout.Description = desout.Description;
                                    dout.Patient_Journey_Transactions_Id = transactionresponse;
                                    dout.Created_By = User511;
                                    dout.Created_Date = DateTime.Now;
                                    dout.MODIFIED_By = User511;
                                    dout.MODIFIED_Date = DateTime.Now;
                                    var desiredresponse = dbPatientAdministration.AddDesiredOutcome(dout);
                                }

                                Patient_Journey_Trans_Clin_Interventions clinint = dbPatientAdministration.GetClinicalIntervention(patientTransactions[j].Patient_Journey_Transactions_Id);
                                if (clinint != null)
                                {
                                    Patient_Journey_Trans_Clin_Interventions cint = new Patient_Journey_Trans_Clin_Interventions();
                                    cint.Clinical_Intervention_Master_Id = clinint.Clinical_Intervention_Master_Id;
                                    cint.Description = clinint.Description;
                                    cint.Patient_Journey_Transactions_Id = transactionresponse;
                                    cint.Created_By = User511;
                                    cint.Created_Date = DateTime.Now;
                                    cint.Modified_By = User511;
                                    cint.Modified_Date = DateTime.Now;
                                    var clinresponse = dbPatientAdministration.AddClinicalIntervention(cint);
                                    List<Patient_Journey_Trans_SubClin_Interventions> lstSubClinicalId = dbPatientJourney.GetSubClinicalIntervention(clinint.Patient_Journey_Trans_Clin_Interventions_Id);
                                    if (lstSubClinicalId.Count > 0)
                                    {
                                        List<Patient_Journey_Trans_SubClin_Interventions> lstsubclin = new List<Patient_Journey_Trans_SubClin_Interventions>();
                                        Patient_Journey_Trans_SubClin_Interventions subclin = null;
                                        for (int l = 0; l < lstSubClinicalId.Count; l++)
                                        {
                                            subclin = new Patient_Journey_Trans_SubClin_Interventions();
                                            subclin.Patient_Journey_Trans_Clin_Interventions_Id = Convert.ToInt32(clinresponse);
                                            subclin.SubClinical_Intervention_Master_Id = lstSubClinicalId[l].SubClinical_Intervention_Master_Id;
                                            subclin.Created_By = User511;
                                            subclin.Created_Date = DateTime.Now;
                                            subclin.Modified_By = User511;
                                            subclin.Modified_Date = DateTime.Now;
                                            lstsubclin.Add(subclin);
                                        }
                                        var subclinresponse = dbPatientAdministration.AddSubClinicalIntervention(lstsubclin);
                                    }

                                }

                                Patient_Journey_Transactions_AssociatedCosts asscost = dbPatientAdministration.GetAssociatedCost(patientTransactions[j].Patient_Journey_Transactions_Id);
                                if (asscost != null)
                                {
                                    Patient_Journey_Transactions_AssociatedCosts acost = new Patient_Journey_Transactions_AssociatedCosts();
                                    acost.AssociatedCosts = asscost.AssociatedCosts;
                                    acost.Description = asscost.Description;
                                    acost.Patient_Journey_Transactions_Id = transactionresponse;
                                    acost.Created_By = User511;
                                    acost.Created_Date = DateTime.Now;
                                    acost.Modified_By = User511;
                                    acost.Modified_Date = DateTime.Now;
                                    var associatedresponse = dbPatientAdministration.AddAssociatedCost(acost);
                                }
                            }
                        }
                    }
                }
                return response;
            }
            else
            {
                return 0;
            }
        }

        public static List<VersionDetails> GetVersionDetails(string JourneyId)
        {
            List<VersionDetails> version = new List<VersionDetails>();
            var tempJourneyId = dbPatientAdministration.GetTempJourneyDetails(Convert.ToInt32(JourneyId));
            var versionDetails = dbPatientAdministration.GetVersionDetails(tempJourneyId.Patient_Journey_Temp_Id);
            for (int i = 0; i < versionDetails.Count; i++)
            {
                VersionDetails currentVersion = new VersionDetails();
                currentVersion.VersionDate = versionDetails[i].Created_Date.Date.ToString("dd/MM/yyyy");
                currentVersion.VersionTime = versionDetails[i].Created_Date.ToString("HH:mm:ss tt");
                currentVersion.Comment = versionDetails[i].Version_Comments;
                if (versionDetails[i].IsApproved == true)
                {
                    currentVersion.IsApproved = "Approved";
                }
                else
                {
                    currentVersion.IsApproved = "To be approved";
                }
                version.Add(currentVersion);
            }
            return version;
        }

        public static Int32? CopyJourneyToTemp(int JourneyId)
        {
            var removeResponse = dbPatientAdministration.RemoveJourneyFromTemp(JourneyId);
            Patient_Journey journey = dbPatientAdministration.GetJourneyDetails(JourneyId);
            Patient_Journey_Temp journeyTemp = new Patient_Journey_Temp();
            if (journey != null)
            {
                journeyTemp.Patient_Journey_Id = journey.Patient_Journey_Id;
                journeyTemp.Journey_Title = journey.Journey_Title;
                journeyTemp.Journey_Description = journey.Journey_Description;
                journeyTemp.Brand_Master_Id = Convert.ToInt32(journey.Brand_Master_Id);
                journeyTemp.Country_Master_Id = Convert.ToInt32(journey.Country_Master_Id);
                journeyTemp.Status_Master_Id = Convert.ToInt32(journey.Status_Master_Id);
                journeyTemp.Year = journey.Year;
                journeyTemp.Created_By = journey.Created_By;
                journeyTemp.Created_Date = Convert.ToDateTime(journey.Created_Date);
                journeyTemp.Modified_By = journey.Modified_By;
                journeyTemp.Modified_Date = journey.Modified_Date;
                var tempJourneyId = dbPatientAdministration.AddJourneyToTemp(journeyTemp);

                if (tempJourneyId != null)
                {
                    List<Patient_Journey_Stages> stages = dbPatientAdministration.GetJourneyStage(JourneyId);
                    Patient_Journey_Stages_Temp stageTemp = null;
                    if (stages != null)
                    {
                        for (int i = 0; i < stages.Count; i++)
                        {
                            stageTemp = new Patient_Journey_Stages_Temp();
                            stageTemp.Patient_Journey_Temp_Id = Convert.ToInt32(tempJourneyId);
                            stageTemp.Patient_Journey_Stages_Id = stages[i].Patient_Journey_Stages_Id;
                            stageTemp.Description = stages[i].Description;
                            stageTemp.Population_Statistics = stages[i].Population_Statistics;
                            stageTemp.Time_Statistics = stages[i].Time_Statistics;
                            stageTemp.Stage_Display_Order = Convert.ToInt32(stages[i].Stage_Display_Order);
                            stageTemp.Stage_Master_Id = Convert.ToInt32(stages[i].Stage_Master_Id);
                            stageTemp.Stage_Title = stages[i].Stage_Title;
                            stageTemp.Created_By = stages[i].Created_By;
                            stageTemp.Created_Date = Convert.ToDateTime(stages[i].Created_Date);
                            stageTemp.Modified_By = stages[i].Modified_By;
                            stageTemp.Modified_Date = stages[i].Modified_Date;
                            var stageresponse = dbPatientAdministration.AddStageToTemp(stageTemp);

                            List<Patient_Journey_Transactions> transactions = dbPatientAdministration.GetTransactions(stages[i].Patient_Journey_Stages_Id);
                            Patient_Journey_Transactions_Temp transactionTemp = null;
                            if (transactions != null)
                            {
                                for (int j = 0; j < transactions.Count; j++)
                                {
                                    transactionTemp = new Patient_Journey_Transactions_Temp();
                                    transactionTemp.Patient_Journey_Temp_Id = Convert.ToInt32(tempJourneyId);
                                    transactionTemp.Patient_Journey_Transactions_Id = transactions[j].Patient_Journey_Transactions_Id;
                                    transactionTemp.Patient_Journey_Stages_Temp_Id = Convert.ToInt32(stageresponse);
                                    transactionTemp.Description = transactions[j].Description;
                                    transactionTemp.Feasibility_Description = transactions[j].Feasibility_Description;
                                    transactionTemp.Feasibility_Rating = transactions[j].Feasibility_Rating;
                                    transactionTemp.HCP_Description = transactions[j].HCP_Description;
                                    transactionTemp.HCP_Rating = transactions[j].HCP_Rating;
                                    transactionTemp.Patient_Description = transactions[j].Patient_Description;
                                    transactionTemp.Patient_Rating = transactions[j].Patient_Rating;
                                    transactionTemp.Payer_Description = transactions[j].Payer_Description;
                                    transactionTemp.Payer_Rating = transactions[j].Payer_Rating;
                                    transactionTemp.Transaction_Display_Order = Convert.ToInt32(transactions[j].Transaction_Display_Order);
                                    transactionTemp.Transaction_Location_Master_Id = Convert.ToInt32(transactions[j].Transaction_Location_Master_Id);
                                    transactionTemp.Transaction_Location_Title = transactions[j].Transaction_Location_Title;
                                    transactionTemp.Transaction_Master_Id = Convert.ToInt32(transactions[j].Transaction_Master_Id);
                                    transactionTemp.Transaction_Title = transactions[j].Transaction_Title;
                                    transactionTemp.Viability_Description = transactions[j].Viability_Description;
                                    transactionTemp.Viability_Rating = transactions[j].Viability_Rating;
                                    transactionTemp.Created_By = transactions[j].Created_By;
                                    transactionTemp.Created_Date = Convert.ToDateTime(transactions[j].Created_Date);
                                    transactionTemp.Modified_By = transactions[j].Modified_By;
                                    transactionTemp.Modified_Date = transactions[j].Modified_Date;
                                    int TransactionCount;
                                    var transactionresponse = dbPatientAdministration.AddTransactionToTemp(transactionTemp, out TransactionCount);

                                    Patient_Journey_Transactions_DesiredOutcomes desout = dbPatientAdministration.GetDesiredOutcome(transactions[j].Patient_Journey_Transactions_Id);
                                    if (desout != null)
                                    {
                                        Patient_Journey_Transactions_DesiredOutcomes_Temp dout = new Patient_Journey_Transactions_DesiredOutcomes_Temp();
                                        dout.DesiredOutcomes = desout.DesiredOutcomes;
                                        dout.Description = desout.Description;
                                        dout.Patient_Journey_Transactions_Temp_Id = Convert.ToInt32(transactionresponse);
                                        dout.Created_By = desout.Created_By;
                                        dout.Created_Date = Convert.ToDateTime(desout.Created_Date);
                                        dout.MODIFIED_By = desout.MODIFIED_By;
                                        dout.MODIFIED_Date = desout.MODIFIED_Date;
                                        var desiredresponse = dbPatientJourney.AddDesiredOutcomeToTemp(dout);
                                    }

                                    Patient_Journey_Trans_Clin_Interventions clinint = dbPatientAdministration.GetClinicalIntervention(transactions[j].Patient_Journey_Transactions_Id);
                                    if (clinint != null)
                                    {
                                        Patient_Journey_Trans_Clin_Interventions_Temp cint = new Patient_Journey_Trans_Clin_Interventions_Temp();
                                        cint.Clinical_Intervention_Master_Id = clinint.Clinical_Intervention_Master_Id;
                                        cint.Description = clinint.Description;
                                        cint.Patient_Journey_Transactions_Temp_Id = Convert.ToInt32(transactionresponse);
                                        cint.Created_By = clinint.Created_By;
                                        cint.Created_Date = Convert.ToDateTime(clinint.Created_Date);
                                        cint.Modified_By = clinint.Modified_By;
                                        cint.Modified_Date = clinint.Modified_Date;
                                        var clinresponse = dbPatientJourney.AddClinicalInterventionToTemp(cint, null);
                                        List<Patient_Journey_Trans_SubClin_Interventions> lstSubClinicalId = dbPatientJourney.GetSubClinicalIntervention(Convert.ToInt32(clinresponse));
                                        if (lstSubClinicalId.Count > 0)
                                        {
                                            List<Patient_Journey_Trans_SubClin_Interventions_Temp> lstsubclin = new List<Patient_Journey_Trans_SubClin_Interventions_Temp>();
                                            Patient_Journey_Trans_SubClin_Interventions_Temp subclin = null;
                                            for (int l = 0; l < lstSubClinicalId.Count; l++)
                                            {
                                                subclin = new Patient_Journey_Trans_SubClin_Interventions_Temp();
                                                subclin.Patient_Journey_Trans_Clin_Interventions_Temp_Id = Convert.ToInt32(clinresponse);
                                                subclin.SubClinical_Intervention_Master_Id = lstSubClinicalId[l].SubClinical_Intervention_Master_Id;
                                                subclin.Created_By = lstSubClinicalId[l].Created_By;
                                                subclin.Created_Date = Convert.ToDateTime(lstSubClinicalId[l].Created_Date);
                                                subclin.Modified_By = lstSubClinicalId[l].Modified_By;
                                                subclin.Modified_Date = lstSubClinicalId[l].Modified_Date;
                                                lstsubclin.Add(subclin);
                                            }
                                            var subclinresponse = dbPatientJourney.AddSubClinicalInterventionToTemp(lstsubclin);
                                        }

                                    }

                                    Patient_Journey_Transactions_AssociatedCosts asscost = dbPatientAdministration.GetAssociatedCost(transactions[j].Patient_Journey_Transactions_Id);
                                    if (asscost != null)
                                    {
                                        Patient_Journey_Transactions_AssociatedCosts_Temp acost = new Patient_Journey_Transactions_AssociatedCosts_Temp();
                                        acost.AssociatedCosts = asscost.AssociatedCosts;
                                        acost.Description = asscost.Description;
                                        acost.Patient_Journey_Transactions_Temp_Id = Convert.ToInt32(transactionresponse);
                                        acost.Created_By = asscost.Created_By;
                                        acost.Created_Date = Convert.ToDateTime(asscost.Created_Date);
                                        acost.Modified_By = asscost.Modified_By;
                                        acost.Modified_Date = asscost.Modified_Date;
                                        var associatedresponse = dbPatientJourney.AddAssociatedCostToTemp(acost);
                                    }
                                }

                            }
                        }

                    }
                }
            }
            return 1;
        }

        public static Int32? CopyJourneyToMain(int JourneyId)
        {
            Patient_Journey_Temp journey = dbPatientAdministration.GetTempJourneyDetails(JourneyId);
            Patient_Journey journeyTemp = new Patient_Journey();
            if (journey != null)
            {
                List<Patient_Journey_Stages_Temp> stages = dbPatientAdministration.GetJourneyStageFromTemp(journey.Patient_Journey_Temp_Id);
                Patient_Journey_Stages stageTemp = null;
                if (stages != null)
                {
                    for (int i = 0; i < stages.Count; i++)
                    {
                        stageTemp = new Patient_Journey_Stages();
                        stageTemp.Patient_Journey_Id = Convert.ToInt32(journey.Patient_Journey_Id);
                        stageTemp.Patient_Journey_Stages_Temp_Id = stages[i].Patient_Journey_Stages_Temp_Id;
                        stageTemp.Description = stages[i].Description;
                        stageTemp.Population_Statistics = stages[i].Population_Statistics;
                        stageTemp.Time_Statistics = stages[i].Time_Statistics;
                        stageTemp.Stage_Display_Order = Convert.ToInt32(stages[i].Stage_Display_Order);
                        stageTemp.Stage_Master_Id = Convert.ToInt32(stages[i].Stage_Master_Id);
                        stageTemp.Stage_Title = stages[i].Stage_Title;
                        stageTemp.Created_By = stages[i].Created_By;
                        stageTemp.Created_Date = Convert.ToDateTime(stages[i].Created_Date);
                        stageTemp.Modified_By = stages[i].Modified_By;
                        stageTemp.Modified_Date = stages[i].Modified_Date;
                        var stageresponse = dbPatientAdministration.AddJourneyStage(stageTemp);
                        var updateStage = dbPatientAdministration.UpdateStageTempId(stages[i].Patient_Journey_Stages_Temp_Id, Convert.ToInt32(stageresponse));

                        List<Patient_Journey_Transactions_Temp> transactions = dbPatientAdministration.GetTransactionsFromTemp(stages[i].Patient_Journey_Stages_Temp_Id);
                        Patient_Journey_Transactions transactionTemp = null;
                        if (transactions != null)
                        {
                            for (int j = 0; j < transactions.Count; j++)
                            {
                                transactionTemp = new Patient_Journey_Transactions();
                                transactionTemp.Patient_Journey_Id = Convert.ToInt32(journey.Patient_Journey_Id);
                                transactionTemp.Patient_Journey_Transactions_Temp_Id = transactions[j].Patient_Journey_Transactions_Temp_Id;
                                transactionTemp.Patient_Journey_Stages_Id = Convert.ToInt32(stageresponse);
                                transactionTemp.Description = transactions[j].Description;
                                transactionTemp.Feasibility_Description = transactions[j].Feasibility_Description;
                                transactionTemp.Feasibility_Rating = transactions[j].Feasibility_Rating;
                                transactionTemp.HCP_Description = transactions[j].HCP_Description;
                                transactionTemp.HCP_Rating = transactions[j].HCP_Rating;
                                transactionTemp.Patient_Description = transactions[j].Patient_Description;
                                transactionTemp.Patient_Rating = transactions[j].Patient_Rating;
                                transactionTemp.Payer_Description = transactions[j].Payer_Description;
                                transactionTemp.Payer_Rating = transactions[j].Payer_Rating;
                                transactionTemp.Transaction_Display_Order = Convert.ToInt32(transactions[j].Transaction_Display_Order);
                                transactionTemp.Transaction_Location_Master_Id = Convert.ToInt32(transactions[j].Transaction_Location_Master_Id);
                                transactionTemp.Transaction_Location_Title = transactions[j].Transaction_Location_Title;
                                transactionTemp.Transaction_Master_Id = Convert.ToInt32(transactions[j].Transaction_Master_Id);
                                transactionTemp.Transaction_Title = transactions[j].Transaction_Title;
                                transactionTemp.Viability_Description = transactions[j].Viability_Description;
                                transactionTemp.Viability_Rating = transactions[j].Viability_Rating;
                                transactionTemp.Created_By = transactions[j].Created_By;
                                transactionTemp.Created_Date = Convert.ToDateTime(transactions[j].Created_Date);
                                transactionTemp.Modified_By = transactions[j].Modified_By;
                                transactionTemp.Modified_Date = transactions[j].Modified_Date;
                                int TransactionCount;
                                var transactionresponse = dbPatientAdministration.AddTransaction(transactionTemp, out TransactionCount);
                                var updateTrans = dbPatientAdministration.UpdateTransactionTempId(transactions[j].Patient_Journey_Transactions_Temp_Id, Convert.ToInt32(transactionresponse));

                                Patient_Journey_Transactions_DesiredOutcomes_Temp desout = dbPatientJourney.GetDesiredOutcomeFromTemp(transactions[j].Patient_Journey_Transactions_Temp_Id);
                                if (desout != null)
                                {
                                    Patient_Journey_Transactions_DesiredOutcomes dout = new Patient_Journey_Transactions_DesiredOutcomes();
                                    dout.DesiredOutcomes = desout.DesiredOutcomes;
                                    dout.Description = desout.Description;
                                    dout.Patient_Journey_Transactions_Id = Convert.ToInt32(transactionresponse);
                                    dout.Created_By = desout.Created_By;
                                    dout.Created_Date = Convert.ToDateTime(desout.Created_Date);
                                    dout.MODIFIED_By = desout.MODIFIED_By;
                                    dout.MODIFIED_Date = desout.MODIFIED_Date;
                                    var desiredresponse = dbPatientAdministration.AddDesiredOutcome(dout);
                                }

                                Patient_Journey_Trans_Clin_Interventions_Temp clinint = dbPatientJourney.GetClinicalInterventionFromTemp(transactions[j].Patient_Journey_Transactions_Temp_Id);
                                if (clinint != null)
                                {
                                    Patient_Journey_Trans_Clin_Interventions cint = new Patient_Journey_Trans_Clin_Interventions();
                                    cint.Clinical_Intervention_Master_Id = clinint.Clinical_Intervention_Master_Id;
                                    cint.Description = clinint.Description;
                                    cint.Patient_Journey_Transactions_Id = Convert.ToInt32(transactionresponse);
                                    cint.Created_By = clinint.Created_By;
                                    cint.Created_Date = Convert.ToDateTime(clinint.Created_Date);
                                    cint.Modified_By = clinint.Modified_By;
                                    cint.Modified_Date = clinint.Modified_Date;
                                    var clinresponse = dbPatientAdministration.AddClinicalIntervention(cint);
                                    List<Patient_Journey_Trans_SubClin_Interventions_Temp> lstSubClinicalId = dbPatientJourney.GetSubClinicalInterventionFromTemp(clinint.Patient_Journey_Trans_Clin_Interventions_Temp_Id);
                                    if (lstSubClinicalId.Count > 0)
                                    {
                                        List<Patient_Journey_Trans_SubClin_Interventions> lstsubclin = new List<Patient_Journey_Trans_SubClin_Interventions>();
                                        Patient_Journey_Trans_SubClin_Interventions subclin = null;
                                        for (int l = 0; l < lstSubClinicalId.Count; l++)
                                        {
                                            subclin = new Patient_Journey_Trans_SubClin_Interventions();
                                            subclin.Patient_Journey_Trans_Clin_Interventions_Id = Convert.ToInt32(clinresponse);
                                            subclin.SubClinical_Intervention_Master_Id = lstSubClinicalId[l].SubClinical_Intervention_Master_Id;
                                            subclin.Created_By = lstSubClinicalId[l].Created_By;
                                            subclin.Created_Date = Convert.ToDateTime(lstSubClinicalId[l].Created_Date);
                                            subclin.Modified_By = lstSubClinicalId[l].Modified_By;
                                            subclin.Modified_Date = lstSubClinicalId[l].Modified_Date;
                                            lstsubclin.Add(subclin);
                                        }
                                        var subclinresponse = dbPatientAdministration.AddSubClinicalIntervention(lstsubclin);
                                    }

                                }

                                Patient_Journey_Transactions_AssociatedCosts_Temp asscost = dbPatientJourney.GetAssociatedCostFromTemp(transactions[j].Patient_Journey_Transactions_Temp_Id);
                                if (asscost != null)
                                {
                                    Patient_Journey_Transactions_AssociatedCosts acost = new Patient_Journey_Transactions_AssociatedCosts();
                                    acost.AssociatedCosts = asscost.AssociatedCosts;
                                    acost.Description = asscost.Description;
                                    acost.Patient_Journey_Transactions_Id = Convert.ToInt32(transactionresponse);
                                    acost.Created_By = asscost.Created_By;
                                    acost.Created_Date = Convert.ToDateTime(asscost.Created_Date);
                                    acost.Modified_By = asscost.Modified_By;
                                    acost.Modified_Date = asscost.Modified_Date;
                                    var associatedresponse = dbPatientAdministration.AddAssociatedCost(acost);
                                }
                            }

                        }
                    }

                }

            }
            return 1;
        }

        public static Int32? RemoveAllStages(string JourneyId)
        {
            var response = dbPatientAdministration.RemoveAllStages(Convert.ToInt32(JourneyId));
            return response;
        }

        public static List<Int32?> GetPageWidthAndHeight(int CountryId, int BrandId, int Year)
        {
            List<Int32?> _list = new List<Int32?>();

            var journey = dbPatientJourney.GetApprovedJourney(CountryId, BrandId, Year);
            Int32? stagesCount = dbPatientJourney.GetStagesCount(Convert.ToInt32(journey.Patient_Journey_Id.ToString()));
            Int32? transactionCount = dbPatientJourney.GetTransactionCount(Convert.ToInt32(journey.Patient_Journey_Id.ToString()));
            Int32? smomCount = dbPatientJourney.GetSmomCount(Convert.ToInt32(journey.Patient_Journey_Id.ToString()));

            int PageWidth = 0; int PageHeight = 0;

            if (stagesCount >= 8)
            {
                PageWidth = 550;
                PageHeight = 1550;
            }

            if (stagesCount >= 0 && stagesCount < 8)
            {
                PageWidth = 500;
                PageHeight = 1500;
            }

            _list.Add(PageWidth);
            _list.Add(PageHeight);
            return _list;
        }
    }
}
