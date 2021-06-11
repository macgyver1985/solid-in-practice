using System;
using Solid.SingleResponsibilityPrinciple.Incorrect;

namespace main
{
    class Program
    {
        static void Main(string[] args)
        {
            NewsletterService service = new NewsletterService();

            service.Register("teste@teste.com.br");
        }
    }
}
