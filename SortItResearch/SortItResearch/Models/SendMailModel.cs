using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace SortItResearch.Models
{
    public class SendMailModel
    {
        public static void SendMail(string emailTo,string body,string Subject)
        {
            try
            {
                
                string EmailFrom = WebConfigurationManager.AppSettings["EmailFrom"];
                string EmailFromPassword = WebConfigurationManager.AppSettings["EmailFromPassword"];

                MailMessage mail = new MailMessage();
                mail.To.Add(emailTo);
                mail.From = new MailAddress(EmailFrom);
                mail.Subject = Subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(EmailFrom, EmailFromPassword); // Enter seders User name and password   
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}