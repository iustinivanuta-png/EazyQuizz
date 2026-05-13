using System;
using System.Collections.Generic;

namespace EazyQuizz
{
    class Program
    {
        static List<Question> questions = QuestionManager.GetDefaultQuestions();

        static void Main(string[] args)
        {
        
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== EazyQuizz ===\n");
                Console.WriteLine("1. Student nou");
                Console.WriteLine("2. Student existent");
                Console.WriteLine("3. Conectare + Start Quiz");
                Console.WriteLine("4. Afisare scoruri");
                Console.WriteLine("5. Afisare intrebari");
                Console.WriteLine("6. Cautare intrebari dupa domeniu");
                Console.WriteLine("7. Adaugare intrebare");
                Console.WriteLine("8. Stergere intrebare");
                Console.WriteLine("9. Salvare intrebari in fisier");
                Console.WriteLine("10. Iesire");

                Console.Write("\nAlege optiune: ");
                string c = Console.ReadLine();

                if (c == "1")
                {
                    bool ok = Student.Register();

                    if (ok)
                    {
                        string nume = Student.LoginAfterRegister();

                        if (nume != null)
                        {
                            RunQuiz(nume);
                        }
                    }

                    Pause();
                }
                else if (c == "2")
                {
                    Student.ShowLoginOnly();
                    Pause();
                }
                else if (c == "3")
                {
                    string nume = Student.Login();

                    if (nume != null)
                    {
                        RunQuiz(nume);
                    }

                    Pause();
                }
                else if (c == "4")
                {
                    Student.ShowScores();
                    Pause();
                }
                else if (c == "5")
                {
                    QuestionManager.ShowQuestions(questions);
                    Pause();
                }
                else if (c == "6")
                {
                    QuestionManager.SearchByDomain(questions);
                    Pause();
                }
                else if (c == "7")
                {
                    QuestionManager.AddQuestion(questions);
                    Pause();
                }
                else if (c == "8")
                {
                    QuestionManager.DeleteQuestion(questions);
                    Pause();
                }
                else if (c == "9")
                {
                    QuestionManager.SaveQuestions(questions);
                    Pause();
                }
                else if (c == "10")
                {
                    Console.WriteLine("La revedere!");
                    break;
                }
                else
                {
                    Console.WriteLine("Optiune invalida!");
                    Pause();
                }
            }
        }

        static void RunQuiz(string nume)
        {
            Quiz quiz = new Quiz();

            foreach (Question q in questions)
            {
                quiz.questions.Add(q);
            }

            quiz.Start(nume);
        }

        static void Pause()
        {
            Console.WriteLine("\nApasa o tasta pentru meniu...");
            Console.ReadKey();
        }
    }
}