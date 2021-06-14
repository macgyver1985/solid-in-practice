using System;
using System.IO;
using System.Net.Mail;

namespace Solid.SingleResponsibilityPrinciple.Correct
{
    public class SMTPService : ISMTPService
    {
        const string PATH_MAIL_SERVER = "./MailServer";

        public SMTPService()
        {
            if (!Directory.Exists(PATH_MAIL_SERVER))
                Directory.CreateDirectory(PATH_MAIL_SERVER);
        }

        public void SendMail(string from, string to, string subject, string body)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient()
                {
                    PickupDirectoryLocation = Path.GetFullPath(PATH_MAIL_SERVER),
                    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                    EnableSsl = false,
                    Host = "smtp-mail.outlook.com",
                    Port = 587
                })
                {
                    smtpClient.Send(from, to, subject, body);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}