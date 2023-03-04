using System;
using RegForm.Models;
using System.Net;
using System.Net.Mail;
using RegForm.Interfaces;

namespace RegForm.Services
{
    public class EmailService : IEmail
    {
        public void SendEmail(EmailRequest EmailBody)
        {
            string username = EmailBody.from;
            string password = ConfigManager.Appsettings["Password"];
            string MailTo = EmailBody.to;

            string[] words = MailTo.Split(',');
            foreach (var item in words)
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.To.Add(item);
                    mail.From = new MailAddress(EmailBody.from);
                    mail.Subject = EmailBody.subject;
                    mail.Body = EmailBody.messagebody;
                    mail.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient(ConfigManager.Appsettings["SMTP"], Convert.ToInt32(ConfigManager.Appsettings["SMTPValue"])))
                    {
                        smtp.Credentials = new NetworkCredential
                           (username, password);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
        }
    }
}

