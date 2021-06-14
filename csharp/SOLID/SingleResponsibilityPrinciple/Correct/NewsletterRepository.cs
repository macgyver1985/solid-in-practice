using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Solid.SingleResponsibilityPrinciple.Correct
{
    public class NewsletterRepository : INewsletterRepository
    {
        const string PATH_DATA_BASE = "./DataSource";

        public NewsletterRepository()
        {
            if (!Directory.Exists(PATH_DATA_BASE))
                Directory.CreateDirectory(PATH_DATA_BASE);
        }

        public bool Register(string email)
        {
            try
            {
                FileInfo db = new FileInfo($"{PATH_DATA_BASE}/newsletter.json");
                List<string> content = new List<string>();

                using (FileStream stream = db.Open(FileMode.OpenOrCreate, FileAccess.Read))
                {
                    using (StreamReader read = new StreamReader(stream, Encoding.UTF8))
                    {
                        var temp = JsonConvert.DeserializeObject<IEnumerable<string>>(read.ReadToEnd());

                        if (temp?.FirstOrDefault(t => t?.ToLower() == email) != null)
                            return false;

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

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}