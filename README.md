# ÍNDICE

- [INTRODUÇÃO](#introdução)
- [PRINCÍPIOS S.O.L.I.D.](#princípios-solid)
	- [Single Responsiblity Principle](#single-responsiblity-principle)
		- [Exemplo de SRP](#exemplo-de-srp)
			- [Aplicação em CSharp](#aplicação-em-csharp)
				- [Violação da SRP](#violação-da-srp)
				- [Aplicação correta da SRP](#aplicação-correta-da-srp)
- [CÓDIGOS DE EXEMPLO](#códigos-de-exemplo)
	- [CSharp](#csharp)
		- [Pré-requisitos](#pré-requisitos)
		- [Executando](#executando)
- [GLOSSÁRIO](#glossário)
- [REFERÊNCIAS](#referências)

# INTRODUÇÃO

Costumo dizer que conhecer os diversos conceitos de arquitetura de software e design de código é mais importante do que tecnologias em si, e penso assim pelas seguintes razões:

1. Os conceitos costumam ser atemporáis;
1. A grande parte dos conceitos podem ser aplicados em qualquer tecnologia;
1. Aplicam-se tanto no back-end quanto no front-end;
1. Proporcionam a construção de aplicações com uma melhor organização, manutenibilidade, reastreabilidade, portabilidade e etc...

SOLID foi proposto por Robert C. (ou Uncle Bob) por volta do ano 2000 e trata-se de um acrônimo dos cinco princípios da programação orientada a objetos.

O repositório "solid-in-practice" tem como proposito explorar e mostrar algumas aplicações práticas dos conceitos em questão com mais de uma linguagem de programação.

# PRINCÍPIOS S.O.L.I.D.

Segue cada um dos cinco princípios e o seu propósito.

| Sigla | Nome |
| :------------ | :------------ |
| **S** | Single Responsiblity Principle |
| **O** | Open-Closed Principle |
| **L** | Liskov Substitution Principle |
| **I** | Interface Segregation Principle |
| **D** | Dependency Inversion Principle |

##  Single Responsiblity Principle

Uma classe deve ser especializada em um únido assunto e ter apenas uma responsabilidade.
Construir uma *God Class** é mais fácil e rápido, porem com o passar do tempo a manuteção e reaproveitamento desse código torna-se impraticável.

### Exemplo de SRP

Imagine uma classe de serviço de domínio que deve registrar o e-mail para receber boletins de notícias e em seguida enviar um e-mail ao destinatário confirmando sua inscrição. A responsabilidade desse serviço é orquestrar e não implementar a persistência ou o envio de e-mail.

#### Aplicação em CSharp

##### Violação da SRP

Podemos ver que a classe *NewsletterService*  tem mais de uma responsabilidade e viola o SPR - single responsibility principle (Princípio de Responsabilidade Única), pois não faz somente o papel de orquestrar, mas contém a implementação para registrar e enviar o e-mail de confirmação.

```csharp
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.Net.Mail;

namespace Solid.SingleResponsibilityPrinciple.Incorrect
{
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
```

##### Aplicação correta da SRP

Podemos ver que a classe *NewsletterService*  não viola o SPR - single responsibility principle (Princípio de Responsabilidade Única).
O registro e o envio de e-mail fica a cargo de outras classes cujas referências das instâncias são passadas pelo construtor.

```csharp
namespace Solid.SingleResponsibilityPrinciple.Correct
{
    public class NewsletterService
    {

        private readonly INewsletterRepository repository;
        private readonly ISMTPService service;

        public NewsletterService(
            INewsletterRepository repository,
            ISMTPService service
        )
        {
            this.repository = repository;
            this.service = service;
        }

        public void Register(string email)
        {
            bool result = this.repository
                .Register(email);

            if (result)
                this.service
                    .SendMail(
                        "newsletter@news.com.br",
                        email,
                        "Confirmação de Inscrição",
                        "Sua inscrição na newsletter foi feita com sucesso."
                    );
        }
    }
}
```

> Abaixo interface **INewsletterRepository**

```csharp
namespace Solid.SingleResponsibilityPrinciple.Correct
{
    public interface INewsletterRepository
    {
        bool Register(string email);
    }
}
```

> Abaixo interface **ISMTPService**

```csharp
namespace Solid.SingleResponsibilityPrinciple.Correct
{
    public interface ISMTPService
    {
        void SendMail(string from, string to, string subject, string body);
    }
}
```

> Abaixo implementação concreta **NewsletterRepository**

```csharp
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
```

> Abaixo implementação concreta **SMTPService**

```csharp
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
```

# CÓDIGOS DE EXEMPLO

Para executar os códigos de exemplo bastas seguir as orientações abaixo após clonar o baixar o repositório.

## CSharp

### Pré-requisitos

- Asp Net Core 3.1
- Visual Studio Code

### Executando

Abra a pasta *csharp* no Visual Studio Code e execute os comandos abaixo:

    $ dotnet restore
    $ dotnet build
    $ dotner run --project .\Main\

# GLOSSÁRIO

- ***God Class**: Na programação orientada a objetos, é uma classe que sabe demais ou faz demais.

# REFERÊNCIAS

- http://butunclebob.com/ArticleS.UncleBob.PrinciplesOfOod
- https://www.c-sharpcorner.com/UploadFile/damubetha/solid-principles-in-C-Sharp/
