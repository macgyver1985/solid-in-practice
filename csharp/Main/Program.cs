﻿using System;
using Incorrect = Solid.SingleResponsibilityPrinciple.Incorrect;

namespace main
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("##################################################################");
            Console.WriteLine("# Execução do código que viola o Single Responsibility Principle #");
            Console.WriteLine("##################################################################");
            Console.WriteLine(".");
            Console.WriteLine("Informe o e-mail para se registrar na newsletter.");

            string email = Console.ReadLine();

            Console.WriteLine(".");

            try
            {
                Incorrect.NewsletterService service = new Incorrect.NewsletterService();

                service.Register(email);

                Console.WriteLine("Registro realizado com sucesso.");
            }
            catch (Exception)
            {
                Console.WriteLine("Ocorreu um erro inesperado.");
            }
        }
    }
}
