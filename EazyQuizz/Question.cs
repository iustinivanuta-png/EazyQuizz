using System.Collections.Generic;

namespace EazyQuizz
{
    public enum Domain
    {
        Geografie,
        Istorie,
        Informatica,
        Biologie
    }

    public enum Difficulty
    {
        Usoara,
        Medie,
        Grea
    }

    [System.Flags]
    public enum QuestionType
    {
        Text = 1,
        Imagine = 2,
        RaspunsMultiplu = 4
    }

    public class Question
    {
        public string text { get; set; }
        public string imagePath { get; set; }
        public Domain domain { get; set; }
        public Difficulty difficulty { get; set; }
        public QuestionType type { get; set; }
        public List<Answer> answers { get; set; } = new List<Answer>();
    }
}