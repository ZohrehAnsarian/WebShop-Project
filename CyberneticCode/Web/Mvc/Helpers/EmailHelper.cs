using System;
using System.Net.Mail;
using System.Text;

namespace CyberneticCode.Web.Mvc.Helpers
{
    public class EmailHelper
    {
        MailMessage MailToUserMessage = new MailMessage();
        SmtpClient smtp = new SmtpClient();

        public string[] EmailList { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public bool SendInBcc { get; set; }
        public string SpecialEmail { get; set; }
        public string ErrorMessage { get; set; }

        public bool Send()
        {

            bool result = true;
            try
            {
                MailToUserMessage.BodyEncoding = Encoding.UTF8;
                MailToUserMessage.SubjectEncoding = Encoding.UTF8;
                MailToUserMessage.Subject = Subject;
                MailToUserMessage.IsBodyHtml = IsBodyHtml;
                MailToUserMessage.Body = Body;

                var i = 0;

                foreach (string email in EmailList)
                {
                    if (!string.IsNullOrWhiteSpace(email))
                    {

                        if (SendInBcc)
                        {
                            MailToUserMessage.Bcc.Add(email);
                        }
                        else
                        {
                            MailToUserMessage.To.Add(email);
                        }
                    }

                    i++;

                    if (i % 5 == 0)
                    {

                        result = NewMethod(result);

                        MailToUserMessage.Bcc.Clear();
                        MailToUserMessage.To.Clear();
                    }
                }
                if (MailToUserMessage.Bcc.Count > 0 || MailToUserMessage.To.Count > 0)
                {
                    result = NewMethod(result);
                }
            }
            catch (Exception ex)
            {
                result = false;

            }


            return result;
        }

        private bool NewMethod(bool result)
        {

            try
            {
                MailToUserMessage.Bcc.Add(SpecialEmail);
                smtp.EnableSsl = false;
                smtp.Send(MailToUserMessage);
            }
            catch (Exception ex1)
            {
                ErrorMessage = ex1.Message + ((ex1.InnerException != null) ? ex1.InnerException.Message : "") + "\n";

                try
                {
                    smtp.EnableSsl = true;
                    smtp.Send(MailToUserMessage);
                }
                catch (Exception ex2)
                {
                    result = false;
                    ErrorMessage += ex2.Message + ((ex2.InnerException != null) ? ex2.InnerException.Message : "") + "\n";
                }
            }

            return result;
        }
    }
}