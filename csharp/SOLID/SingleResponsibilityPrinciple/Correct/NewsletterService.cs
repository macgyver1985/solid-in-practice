namespace Solid.SingleResponsibilityPrinciple.Correct
{
    /// <summary>
    /// Aqui temos uma classe que exemplifica um serviço de domínio.
    /// Normalmente essas classes tem a responsabilidade de realizar um fluxo de negócio ou um caso de uso por assim dizer.
    /// 
    /// Esse serviço deve registrar um e-mail para receber boletins de notícias e em seguida enviar um e-mail ao destinatário confirmando sua inscrição.
    /// Aqui a responsabilidade da classe é orquestrar e não implementar a persistência ou o envio de e-mail, podemos ver que ela não tem mais
    /// de uma responsabilidade, portanto não viola o SPR - single responsibility principle (Princípio de Responsabilidade Única).
    /// </summary>
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