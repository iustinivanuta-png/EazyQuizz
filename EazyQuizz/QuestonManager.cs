using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EazyQuizz
{
    class QuestionManager
    {
        static string filePath = "questions.txt";

        public static List<Question> GetDefaultQuestions()
        {
            List<Question> questions = new List<Question>();

            questions.Add(new Question
            {
                text = "Capitala Frantei?",
                domain = Domain.Geografie,
                difficulty = Difficulty.Usoara,
                answers = new Answer[]
                {
                    new Answer { text = "Madrid", correct = false },
                    new Answer { text = "Paris", correct = true },
                    new Answer { text = "Roma", correct = false },
                    new Answer { text = "Berlin", correct = false }
                }
            });

            questions.Add(new Question
            {
                text = "Cate continente exista?",
                domain = Domain.Geografie,
                difficulty = Difficulty.Usoara,
                answers = new Answer[]
                {
                    new Answer { text = "5", correct = false },
                    new Answer { text = "6", correct = false },
                    new Answer { text = "7", correct = true },
                    new Answer { text = "8", correct = false }
                }
            });

            questions.Add(new Question
            {
                text = "Cine a fost Alexandru Ioan Cuza?",
                domain = Domain.Istorie,
                difficulty = Difficulty.Medie,
                answers = new Answer[]
                {
                    new Answer { text = "Poet", correct = false },
                    new Answer { text = "Domnitor", correct = true },
                    new Answer { text = "Medic", correct = false },
                    new Answer { text = "Profesor", correct = false }
                }
            });

            return questions;
        }

        public static void ShowQuestions(List<Question> questions)
        {
            Console.WriteLine("=== Intrebari disponibile ===\n");

            for (int i = 0; i < questions.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + questions[i].text);
                Console.WriteLine("Domeniu: " + questions[i].domain);
                Console.WriteLine("Dificultate: " + questions[i].difficulty);
                Console.WriteLine();
            }
        }

        public static void SearchByDomain(List<Question> questions)
        {
            Console.Write("Introdu domeniul cautat: ");
            string domainText = Console.ReadLine();

            var results = questions
                .Where(q => q.domain.ToString().ToLower() == domainText.ToLower())
                .ToList();

            if (results.Count == 0)
            {
                Console.WriteLine("Nu exista intrebari pentru acest domeniu.");
                return;
            }

            ShowQuestions(results);
        }

        public static void AddQuestion(List<Question> questions)
        {
            Question q = new Question();

            Console.Write("Text intrebare: ");
            q.text = Console.ReadLine();

            Console.WriteLine("Domeniu: 0-Geografie, 1-Istorie, 2-Informatica, 3-Biologie");
            q.domain = (Domain)int.Parse(Console.ReadLine());

            Console.WriteLine("Dificultate: 0-Usoara, 1-Medie, 2-Grea");
            q.difficulty = (Difficulty)int.Parse(Console.ReadLine());

            q.answers = new Answer[4];

            for (int i = 0; i < 4; i++)
            {
                Console.Write("Raspuns " + (i + 1) + ": ");
                string answerText = Console.ReadLine();

                Console.Write("Este corect? da/nu: ");
                string correctText = Console.ReadLine();

                q.answers[i] = new Answer
                {
                    text = answerText,
                    correct = correctText.ToLower() == "da"
                };
            }

            questions.Add(q);
            Console.WriteLine("Intrebarea a fost adaugata.");
        }

        public static void DeleteQuestion(List<Question> questions)
        {
            ShowQuestions(questions);

            Console.Write("Introdu numarul intrebarii de sters: ");
            int index = int.Parse(Console.ReadLine()) - 1;

            if (index >= 0 && index < questions.Count)
            {
                questions.RemoveAt(index);
                Console.WriteLine("Intrebarea a fost stearsa.");
            }
            else
            {
                Console.WriteLine("Index invalid.");
            }
        }

        public static void SaveQuestions(List<Question> questions)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (Question q in questions)
                {
                    sw.WriteLine(q.text + "|" + q.domain + "|" + q.difficulty);
                }
            }

            Console.WriteLine("Intrebarile au fost salvate in fisier.");
        }
    }
}