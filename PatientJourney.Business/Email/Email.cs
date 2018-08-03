using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using PatientJourney.BusinessModel.BuilderModels;
using System.Web.Hosting;


namespace PatientJourney.Business
{
    /// <summary>
    /// EmailDynamicParams
    /// </summary>
    public class EmailDynamicParams
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string NewMailAddress { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }
        public string JourneyName { get; set; }
        public string BrandName { get; set; }
        public string CountryName { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ReviewedBy { get; set; }
        public string Comment { get; set; }
    }

    /// <summary>
    /// Email
    /// </summary>
    public class Email
    {
        /// <summary>
        /// SendEmail
        /// </summary>
        /// <param name="mailFrom"></param>
        /// <param name="mailFromName"></param>
        /// <param name="mailTo"></param>
        /// <param name="mailSubject"></param>
        /// <param name="mailBody"></param>
        /// <returns></returns>
        public bool SendEmail(MailContents mailContents)
        {
            bool blnStatus = false;
            LinkedResource inlineLogo;
            LinkedResource inlineHeader;
            LinkedResource inlineFooter;
            try
            {
                MailMessage mail = new MailMessage(mailContents.fromAddress, mailContents.toAddress);

                if (!string.IsNullOrEmpty(mailContents.ccAddress))
                {
                    MailAddress copy = new MailAddress(mailContents.ccAddress);
                    mail.CC.Add(copy);
                }

                mail.Subject = mailContents.subject;

                if (!string.IsNullOrEmpty(mailContents.body))
                {
                    mail.Body = mailContents.body;
                    mail.IsBodyHtml = true;

                    inlineLogo = new LinkedResource(HostingEnvironment.MapPath("~/Content/images/logo_png.png"), System.Net.Mime.MediaTypeNames.Image.Gif);
                    //Header
                    inlineHeader = new LinkedResource(HostingEnvironment.MapPath("~/Content/images/mailbanner.png"), System.Net.Mime.MediaTypeNames.Image.Gif);
                    //Footer
                    inlineFooter = new LinkedResource(HostingEnvironment.MapPath("~/Content/images/AbbVieLogo_Preferred_White_sm.png"), System.Net.Mime.MediaTypeNames.Image.Gif);

                    inlineLogo.ContentId = "(PSLogo)";
                    inlineHeader.ContentId = "(PSHeader)";
                    inlineFooter.ContentId = "(PSFooter)";

                    mailContents.body = mailContents.body.Replace("(PSLogo)", inlineLogo.ContentId);
                    mailContents.body = mailContents.body.Replace("(PSHeader)", inlineHeader.ContentId);
                    mailContents.body = mailContents.body.Replace("(PSFooter)", inlineFooter.ContentId);

                    var view = AlternateView.CreateAlternateViewFromString(mailContents.body, null, System.Net.Mime.MediaTypeNames.Text.Html);

                    view.LinkedResources.Add(inlineLogo);
                    view.LinkedResources.Add(inlineHeader);
                    view.LinkedResources.Add(inlineFooter);

                    mail.AlternateViews.Add(view);
                }

                var smtpClient = new SmtpClient();
                smtpClient.Send(mail);
                blnStatus = true;
                return blnStatus;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
