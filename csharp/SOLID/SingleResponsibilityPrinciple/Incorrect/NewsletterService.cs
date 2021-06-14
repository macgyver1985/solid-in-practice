using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.Net.Mail;

namespace Solid.SingleResponsibilityPrinciple.Incorrect
{
    /// <summary>
    /// Aqui temos uma classe que exemplifica um serviço de domínio.
    /// Normalmente essas classes tem a responsabilidade de realizar um fluxo de negócio ou um caso de uso por assim dizer.
    /// 
    /// Esse serviço deve registrar um e-mail para receber boletins de notícias e em seguida enviar um e-mail ao destinatário confirmando sua inscrição.
    /// Aqui a responsabilidade da classe é orquestrar e não implementar a persistência ou o envio de e-mail, porém podemos ver que ela tem mais
    /// de uma responsabilidade e viola o SPR - single responsibility principle (Princípio de Responsabilidade Única).
    /// </summary>
    public class NewsletterService
    {
        public void Register(string email)
        {
            const string PATH_DATA_BASE = "./DataSource";

            if (!Directory.Exists(PATH_DATA_BASE))
                Directory.CreateDirectory(PATH_DATA_BASE);

            FileInfo db = new FileInfo($"{PATH_DATA_BASE}/newsletter.json");
            List<string> content = new List<string>();

            using (FileStream stream = db.Open(FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (StreamReader read = new StreamReader(stream, Encoding.UTF8))
                {
                    var temp = JsonConvert.DeserializeObject<IEnumerable<string>>(read.ReadToEnd());

                    if (temp?.FirstOrDefault(t => t?.ToLower() == email) != null)
                        return;

                    if (temp != null && temp.Any())
                        content.AddRange(temp);
                }
            }

            using (FileStream stream = db.Create())
            {
                content.Add(email);

                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    var temp = JsonConvert.SerializeObject(content, Formatting.Indented);

                    writer.Write(temp);
                }
            }

            this.SendMail(email);
        }

        private void SendMail(string email)
        {
            const string PATH_MAIL_SERVER = "./MailServer";

            if (!Directory.Exists(PATH_MAIL_SERVER))
                Directory.CreateDirectory(PATH_MAIL_SERVER);

            using (SmtpClient smtpClient = new SmtpClient()
            {
                PickupDirectoryLocation = Path.GetFullPath(PATH_MAIL_SERVER),
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                EnableSsl = false,
                Host = "smtp-mail.outlook.com",
                Port = 587
            })
            {
                smtpClient.Send(
                    "newsletter@news.com.br",
                    email,
                    "Confirmação de Inscrição",
                    "Sua inscrição na newsletter foi feita com sucesso."
                );
            }
        }
    }
}