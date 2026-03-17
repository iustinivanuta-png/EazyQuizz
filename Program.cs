using System;
using KnowledgeTestSystem.Models;

namespace KnowledgeTestSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistem de Test de Evaluare a Cunoștințelor ===\n");
            Console.WriteLine("Selectează domeniu:");
            Console.WriteLine("1. Geografie");
            Console.WriteLine("2. Biologie");
            Console.WriteLine("3. Istorie");
            Console.WriteLine("4. Matematică");
            Console.Write("\nAlege opțiune (1-4): ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nAi selectat: Geografie");
                    break;
                case "2":
                    Console.WriteLine("\nAi selectat: Biologie");
                    break;
                case "3":
                    Console.WriteLine("\nAi selectat: Istorie");
                    break;
                case "4":
                    Console.WriteLine("\nAi selectat: Matematică");
                    break;
                default:
                    Console.WriteLine("\nOpțiune invalidă!");
                    break;
            }

            Console.WriteLine("\nTestul va fi implementat în următoarea etapă...");
        }
    }
}
