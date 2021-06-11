using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

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
            FileInfo db = new FileInfo("./csharp/DataSource/newsletter.json");
            List<string> content = new List<string>();

            using (FileStream stream = db.Open(FileMode.Open, FileAccess.Read))
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
        }
    }
}