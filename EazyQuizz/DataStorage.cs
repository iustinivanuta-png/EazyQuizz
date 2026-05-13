using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EazyQuizz
{
    class DataStorage
    {
        static string questionsFile = "questions.txt";
        static string studentsFile = "students.txt";
        static string scoresFile = "scores.txt";

        public static List<Question> LoadQuestions()
        {
            List<Question> questions = new List<Question>();

            if (!File.Exists(questionsFile))
            {
                questions.Add(new Question
                {
                    text = "Capitala Frantei?",
                    domain = Domain.Geografie,
                    difficulty = Difficulty.Usoara,
                    type = QuestionType.Text | QuestionType.RaspunsMultiplu,
                    imagePath = "",
                    answers = new List<Answer>
                    {
                        new Answer { text = "Madrid", correct = false },
                        new Answer { text = "Paris", correct = true },
                        new Answer { text = "Roma", correct = false },
                        new Answer { text = "Berlin", correct = false }
                    }
                });

                questions.Add(new Question
                {
                    text = "Cine a fost Alexandru Ioan Cuza?",
                    domain = Domain.Istorie,
                    difficulty = Difficulty.Medie,
                    type = QuestionType.Text | QuestionType.RaspunsMultiplu,
                    imagePath = "",
                    answers = new List<Answer>
                    {
                        new Answer { text = "Poet", correct = false },
                        new Answer { text = "Domnitor", correct = true },
                        new Answer { text = "Medic", correct = false },
                        new Answer { text = "Profesor", correct = false }
                    }
                });

                SaveQuestions(questions);
                return questions;
            }

            foreach (string line in File.ReadAllLines(questionsFile))
            {
                string[] p = line.Split('|');

                if (p.Length >= 7)
                {
                    Question q = new Question();
                    q.text = p[0];
                    q.domain = Enum.Parse<Domain>(p[1]);
                    q.difficulty = Enum.Parse<Difficulty>(p[2]);
                    q.type = Enum.Parse<QuestionType>(p[3]);
                    q.imagePath = p[4];

                    for (int i = 5; i < p.Length; i++)
                    {
                        string[] a = p[i].Split(';');

                        if (a.Length == 2)
                        {
                            q.answers.Add(new Answer
                            {
                                text = a[0],
                                correct = bool.Parse(a[1])
                            });
                        }
                    }

                    questions.Add(q);
                }
            }

            return questions;
        }

        public static void SaveQuestions(List<Question> questions)
        {
            using StreamWriter sw = new StreamWriter(questionsFile);

            foreach (Question q in questions)
            {
                string line = q.text + "|" + q.domain + "|" + q.difficulty + "|" + q.type + "|" + q.imagePath;

                foreach (Answer a in q.answers)
                {
                    line += "|" + a.text + ";" + a.correct;
                }

                sw.WriteLine(line);
            }
        }

        public static bool RegisterStudent(string name, string password)
        {
            if (File.Exists(studentsFile))
            {
                foreach (string line in File.ReadAllLines(studentsFile))
                {
                    if (line.StartsWith(name + "_"))
                        return false;
                }
            }

            File.AppendAllText(studentsFile, name + "_" + password + Environment.NewLine);
            return true;
        }

        public static bool LoginStudent(string name, string password)
        {
            if (!File.Exists(studentsFile))
                return false;

            return File.ReadAllLines(studentsFile).Any(line => line == name + "_" + password);
        }

        public static void SaveScore(string name, int score, int total)
        {
            File.AppendAllText(scoresFile, name + "_" + score + "_" + total + Environment.NewLine);
        }

        public static List<QuizResult> LoadScores()
        {
            List<QuizResult> results = new List<QuizResult>();

            if (!File.Exists(scoresFile))
                return results;

            foreach (string line in File.ReadAllLines(scoresFile))
            {
                string[] p = line.Split('_');

                if (p.Length == 3)
                {
                    results.Add(new QuizResult
                    {
                        studentName = p[0],
                        score = int.Parse(p[1]),
                        total = int.Parse(p[2])
                    });
                }
            }

            return results;
        }
    }
}